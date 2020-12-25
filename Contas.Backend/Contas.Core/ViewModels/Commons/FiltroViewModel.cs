using System;
using System.Collections.Generic;
using System.Text;

namespace Contas.Core.ViewModels.Commons
{
    public class FiltroViewModel
    {
        public FiltroViewModel() { }

        public bool Paginar { get; set; }
        public int Pagina { get; set; }
        public int PorPagina { get; set; }
    }
}
