using System;

namespace FilPan.Web.Models
{
    public class FilPanItem
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string DataCid { get; set; }

        public string WxpayKey { get; set; }

        public string AlipayKey { get; set; }

        public string FileName { get; set; }

        public long FileSize { get; set; }

        public bool Secret { get; set; }

        public DateTime Created { get; set; }
    }
}
