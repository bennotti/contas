using System.Net;

namespace Contas.Core.ViewModels
{
    public class ErrorResponse
    {
        public string error { get; set; }
        public HttpStatusCode code { get; set; }
    }

}
