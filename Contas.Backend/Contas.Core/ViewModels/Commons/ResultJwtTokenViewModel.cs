using System;
using System.Collections.Generic;
using System.Text;

namespace Contas.Core.ViewModels.Commons
{
    public class ResultJwtTokenViewModel
    {
        public int JwtTokenId { get; set; }
        public int UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
