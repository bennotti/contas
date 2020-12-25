using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Contas.Core.Extensions
{
    public class JsonContentExtensions : StringContent
    {
        public JsonContentExtensions(object obj) : base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json") { }

    }
}
