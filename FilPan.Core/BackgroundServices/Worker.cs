using FilPan.Sdk;
using FilPan.Entities;
using FilPan.Managers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace FilPan.BackgroundServices
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly LotusClient _lotusClient;
        private readonly LotusClientSetting _lotusClientSetting;
        private readonly FilPanManager _filPanManager;

        public Worker(ILogger<Worker> logger, LotusClient lotusClient, LotusClientSetting lotusClientSetting, FilPanManager filPanManager)
        {
            _logger = logger;
            _lotusClient = lotusClient;
            _lotusClientSetting = lotusClientSetting;
            _filPanManager = filPanManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                int limit = 10;
                int skip = 0;
                while (true)
                {
                    var fileCids = await _filPanManager.GetFileCids(FileCidStatus.None, skip, limit);
                    if (fileCids.Count() == 0)
                    {
                        break;
                    }
                    foreach (var fileCid in fileCids)
                    {
                        var path = _filPanManager.GetStoragePath(fileCid.Id);
                        if (File.Exists(path))
                        {
                            var result = await _lotusClient.ClientImport(new ClientImportRequest
                            {
                                Path = path,
                                IsCAR = false
                            });
                            if (result.Success)
                            {
                                await _filPanManager.UpdateFileCid(fileCid.Id, result.Result.Root.Value, FileCidStatus.Success);
                            }
                        }
                    }
                    skip += limit;
                }
                try
                {
                    var imports = await _lotusClient.ClientListImports();
                    if (!imports.Success)
                    {
                        continue;
                    }
                    if (imports.Result.Length > 0)
                    {
                        var rootCids = imports.Result.Where(i => i.Root != null)
                            .Select(i => i.Root.Value).Distinct().ToDictionary(i => i, i => imports.Result.First(j => j.Root?.Value == i).Key);
                        foreach (var item in imports.Result)
                        {
                            if (item.Root == null)
                            {
                                continue;
                            }
                            if (rootCids[item.Root.Value] != item.Key)
                            {
                                await _lotusClient.ClientRemoveImport(item.Key);
                                _logger.LogWarning("remove  duplicate import {key}", item.Key);
                            }
                        }
                        rootCids = null;

                        imports = await _lotusClient.ClientListImports();
                        if (!imports.Success)
                        {
                            continue;
                        }
                        var minDealDuration = 180 * LotusConstants.EpochsInDay;

                        foreach (var item in imports.Result)
                        {
                            if (item.Root == null)
                                continue;
                            var queryOffers = await _lotusClient.ClientFindData(new ClientFindDataRequest { Root = item.Root });
                            if (!queryOffers.Success)
                                continue;
                            if (queryOffers.Result.Length > 0)
                                continue;
                            if (!File.Exists(item.FilePath))
                                continue;
                            var size = new FileInfo(item.FilePath).Length;
                            var miner = _lotusClientSetting.GetMinerByFileSize(size);
                            if (string.IsNullOrEmpty(miner))
                                continue;
                            var minerInfo = await _lotusClient.StateMinerInfo(new StateMinerInfoRequest { Miner = miner });
                            if (!minerInfo.Success)
                                continue;
                            var ask = await _lotusClient.ClientQueryAsk(new ClientQueryAskRequest
                            {
                                PeerId = minerInfo.Result.PeerId,
                                Miner = miner
                            });
                            if (!ask.Success)
                                continue;
                            var dealParams = await CreateTTGraphsyncClientStartDealParams(new ClientStartDealRequest
                            {
                                DataCid = item.Root.Value,
                                Miner = miner,
                                Price = ask.Result.Ask.VerifiedPrice,
                                Duration = minDealDuration
                            });
                            if (dealParams == null)
                                continue;
                            var dealCid = await _lotusClient.ClientStartDeal(dealParams);
                            if (dealCid.Success)
                            {
                                _logger.LogInformation("ClientStartDeal Result: {datecid}(datecid) - {dealcid}(dealcid)", item.Root.Value, dealCid.Result.Value);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(0, ex, "Worker error");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task<ClientStartDealParams> CreateTTGraphsyncClientStartDealParams(ClientStartDealRequest model)
        {
            var dataRef = new TransferDataRef()
            {
                TransferType = TransferType.graphsync.ToString(),
                Root = new Cid() { Value = model.DataCid },
            };

            var walletAddress = await _lotusClient.WalletDefaultAddress();
            if (!walletAddress.Success)
            {
                return null;
            }
            var dealParams = new ClientStartDealParams()
            {
                Data = dataRef,
                Miner = model.Miner,
                MinBlocksDuration = (ulong)model.Duration,
                EpochPrice = model.Price,
                Wallet = walletAddress.Result,
                VerifiedDeal = false,
                ProviderCollateral = "0"
            };
            return dealParams;
        }
    }
}