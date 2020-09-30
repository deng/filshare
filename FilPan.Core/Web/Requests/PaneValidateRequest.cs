using System.ComponentModel.DataAnnotations;

namespace FilPan.Web.Requests
{
    public class PanValidateRequest
    {
        [Required]
        public string Password { get; set; }
    }
}
