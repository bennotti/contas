using Newtonsoft.Json;

namespace Contas.Core.ViewModels
{
    public class ResultResponse
    {
        public ResultResponse()
        {
            this.success = true;
        }

        public ResultResponse(string message)
        {
            this.success = true;
            this.message = message;
        }

        public bool success { get; set; }
        public string message { get; set; }

        public object response { get; set; }

        [JsonProperty(PropertyName = "errors", NullValueHandling = NullValueHandling.Ignore)]
        public string errors { get; set; }


        public string codigo { get; set; }
        public int? id { get; set; }
    }
}
