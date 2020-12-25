using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Contas.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetConnectionStringEnv(this IConfiguration config, string name)
        {
            var c = config.GetSectionEnv("ConnectionStrings");

            return c[name];
        }
        public static IConfigurationSection GetSectionEnv(this IConfiguration config, string name)
        {
            var va = Environment.GetEnvironmentVariable(name);
            if (!string.IsNullOrEmpty(va)) return new CustomConfigurationSection("", va);
            return config.GetSection(name);
        }
    }

    public class CustomConfigurationSection : IConfigurationSection
    {
        private Dictionary<string, dynamic> _valores;
        private string _valor;
        private string _chave;
        public CustomConfigurationSection(string chave, string valor)
        {
            _chave = chave;
            _valor = valor;

            if (IsValidJson(valor)){
                _valores = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(valor);
            } else
            {
                _valores = new Dictionary<string, dynamic>();
            }
        }

        private static bool IsValidJson(string Input)
        {
            Input = Input.Trim();
            if ((Input.StartsWith("{") && Input.EndsWith("}")) || //For object
            (Input.StartsWith("[") && Input.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(Input);
                    return true;
                }
                /*catch //(JsonReaderException jex)
                {
                    //Exception in parsing json
                    //Console.WriteLine(jex.Message);
                    return false;
                }*/
                catch //(Exception ex) //some other exception
                {
                    //Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public string this[string key] { 
            get {
                return _valores[key].ToString();
            } 
            set { } 
        }

        public string Key => _chave;

        public string Path => "";

        public string Value { get => _valor; set => _valor = value; }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _valores.Select(p => new CustomConfigurationSection(p.Key, p.Value)).AsEnumerable();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            return new CustomConfigurationSection(key, this[key]);
        }
    }
}
