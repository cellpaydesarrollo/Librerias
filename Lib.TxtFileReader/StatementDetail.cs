using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.TxtFileReader
{
    public class StatementDetail
    {
        public int TipoRegistro { get; set; }
        public int Posicion { get; set; }
        public string Campo { get; set; }
        public int Longitud { get; set; }
        public TipoValor TipoValor { get; set; }
    }
}
