using System;
using System.Collections.Generic;
using System.Text;

namespace Contas.Core.Models
{
    public class JwtTokens : TbBase
    {

        public int JwtTokenId { get; set; }
        public string UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string IpCriacao { get; set; }
        public DateTime? DataRevogacao { get; set; }
        public string IpRevogacao { get; set; }

        public virtual AspNetUsers Usuario { get; set; }


    }
}
