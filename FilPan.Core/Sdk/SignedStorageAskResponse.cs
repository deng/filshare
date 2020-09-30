using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace FilPan.Sdk
{
    public class SignedStorageAskResponse
    {
        public StorageAsk Ask { get; set; }

        public Signature Signature { get; set; }
    }

    public class Signature
    {
        public byte Type { get; set; }

        public byte[] Data { get; set; }
    }

    public class StorageAsk
    {
        public string Price { get; set; }

        public string VerifiedPrice { get; set; }

        public ulong MinPieceSize { get; set; }

        public ulong MaxPieceSize { get; set; }

        public string Miner { get; set; }

        public long Timestamp { get; set; }

        public long Expiry { get; set; }

        public ulong SeqNo { get; set; }
    }
}
