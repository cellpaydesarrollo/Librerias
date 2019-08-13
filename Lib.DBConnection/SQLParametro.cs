using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.DBConnection
{
    public class SQLParametro
    {
        public string NombreParametro { get; set; }
        public object ValorParametro { get; set; }
        public SqlDbType TipoParametro { get; set; }
        public int LongitudParametro { get; set; }
    }
}
