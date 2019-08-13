using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.TxtFileReader
{
    public class Statement
    {
        public string TipoRegistro { get; set; }
        public string Descripcion { get; set; }
        public int Inicio { get; set; }
        public int Fin { get; set; }
    }
}
