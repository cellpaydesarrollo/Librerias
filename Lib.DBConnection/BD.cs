using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Lib.DBConnection
{
    public class BD
    {
        private SqlConnection sqlCX { get; set; }
        private string CadenaCX { get; set; }

        public BD()
        {
            CadenaCX = ConfigurationManager.ConnectionStrings["cxLocal"].ConnectionString;
        }

        public DataTable EjecutarComando(string pComando, List<SQLParametro> pParametros)
        {
            DataTable dt = new DataTable();
            using (SqlCommand comando = new SqlCommand())
            {
                using (sqlCX = new SqlConnection(CadenaCX))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandText = pComando;
                    comando.Connection = sqlCX;
                    foreach (SQLParametro p in pParametros)
                    {
                        comando.Parameters.Add(p.NombreParametro, p.TipoParametro, p.LongitudParametro).Value = p.ValorParametro;
                    }
                    sqlCX.Open();
                    using (SqlDataReader dr = comando.ExecuteReader())
                    {
                        dt.Load(dr);
                    }
                    sqlCX.Close();
                }
            }
            return dt;
        }
    }
}
