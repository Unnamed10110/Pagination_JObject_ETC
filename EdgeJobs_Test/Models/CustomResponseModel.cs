using System.Security.Principal;
using System.Text.Json.Nodes;

namespace EdgeJobs_Test.Models
{
    public class CustomResponseModel
    {
        public string ResponseStatus { get; set; }

        public JsonNode Response{ get; set; }
    }
}
