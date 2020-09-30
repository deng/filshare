using Newtonsoft.Json;

namespace FilPan.Sdk
{
    public class ClientFindDataRequest
    {
        public Cid Root { get; set; }

        public Cid Piece { get; set; }
    }
}
