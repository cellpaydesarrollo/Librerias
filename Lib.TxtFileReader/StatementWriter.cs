using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.DBConnection;

namespace Lib.TxtFileReader
{
    public class StatementWriter
    {
        BD conexion;

        public StatementWriter()
        {
            conexion = new BD();
        }

        public List<Statement> ObtenerPosiciones()
        {
            List<Statement> lst = new List<Statement>();
            DataTable dt = new DataTable();
            string nombreSP = "PA_STATEMENT_CObtenerPosicionesStatement";
            List<SQLParametro> lstParametros = new List<SQLParametro>();
            SQLParametro parametro = new SQLParametro();
            parametro.NombreParametro = "@pDetalle";
            parametro.ValorParametro = 0;   //0 Encabezado, 1 Detalle
            parametro.TipoParametro = SqlDbType.Int;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pTipoRegistro";
            parametro.ValorParametro = 0;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 1;
            lstParametros.Add(parametro);
            dt = conexion.EjecutarComando(nombreSP, lstParametros);
            if (dt != null && dt.Rows.Count > 0)
            {
                Statement st = new Statement();
                string pTipoRegistro = string.Empty;
                string pDescripcion = string.Empty;
                int pInicio = 0;
                int pFin = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    st = new Statement();
                    //int.TryParse(dr["TipoRegistro"].ToString(), out pTipoRegistro);
                    pTipoRegistro = dr["TipoRegistro"].ToString();
                    pDescripcion = dr["Descripcion"].ToString();
                    int.TryParse(dr["Inicio"].ToString(), out pInicio);
                    int.TryParse(dr["Fin"].ToString(), out pFin);
                    st.TipoRegistro = pTipoRegistro;
                    st.Descripcion = pDescripcion;
                    st.Inicio = pInicio - 1;
                    st.Fin = pFin;
                    lst.Add(st);
                }
            }
            return lst;
        }
        public List<StatementDetail> ObtenerDetallePosiciones(string pTipoRegistro)
        {
            List<StatementDetail> lstDetalle = new List<StatementDetail>();
            DataTable dt = new DataTable();
            string nombreSP = "PA_STATEMENT_CObtenerPosicionesStatement";
            List<SQLParametro> lstParametros = new List<SQLParametro>();
            SQLParametro parametro = new SQLParametro();
            parametro.NombreParametro = "@pDetalle";
            parametro.ValorParametro = 1;   //0 Encabezado, 1 Detalle
            parametro.TipoParametro = SqlDbType.Int;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pTipoRegistro";
            parametro.ValorParametro = pTipoRegistro;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 1;
            lstParametros.Add(parametro);
            dt = conexion.EjecutarComando(nombreSP, lstParametros);
            if (dt != null && dt.Rows.Count > 0)
            {
                StatementDetail stDetail = new StatementDetail();
                int tipoRegistro = 0;
                int posicion = 0;
                string campo = string.Empty;
                int longitud = 0;
                string tipoValorAux = string.Empty;
                TipoValor tipoValor = TipoValor.Numerico;
                foreach (DataRow dr in dt.Rows)
                {
                    stDetail = new StatementDetail();
                    int.TryParse(dr["TipoRegistro"].ToString(), out tipoRegistro);
                    int.TryParse(dr["Posicion"].ToString(), out posicion);
                    campo = dr["Campo"].ToString();
                    int.TryParse(dr["Longitud"].ToString(), out longitud);
                    tipoValorAux = dr["TipoValor"].ToString();
                    Enum.TryParse<TipoValor>(tipoValorAux, out tipoValor);
                    stDetail.TipoRegistro = tipoRegistro;
                    stDetail.Posicion = posicion;
                    stDetail.Campo = campo;
                    stDetail.Longitud = longitud;
                    stDetail.TipoValor = tipoValor;
                    lstDetalle.Add(stDetail);
                }
            }
            return lstDetalle;
        }
        public string ExisteStatement(string pDescripcionArchivo)
        {
            string respuesta = string.Empty;
            DataTable dt = new DataTable();
            string nombreSP = "PA_STATEMENT_CExisteStatement";
            List<SQLParametro> lstParametros = new List<SQLParametro>();
            SQLParametro parametro = new SQLParametro();
            parametro.NombreParametro = "@pDescripcionArchivo";
            parametro.ValorParametro = pDescripcionArchivo;   //0 Encabezado, 1 Detalle
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 50;
            lstParametros.Add(parametro);            
            dt = conexion.EjecutarComando(nombreSP, lstParametros);
            if (dt != null && dt.Rows.Count > 0)
            {
                respuesta = dt.Rows[0]["Respuesta"].ToString();
            }
            return respuesta;
        }
        public DataTable InsertarEncabezado(bool pActualizar, int pIdArchivo, string pDescripcionArchivo, int pNumLineasArchivo, int pNumMovimientos, short pAnno, short pMes, string pTipoRegistro, string pFechaProceso, string pNumeroSecuencialArchivo, string pCodigoProducto, string pDescripcionProducto)
        {
            List<StatementDetail> lstDetalle = new List<StatementDetail>();
            DataTable dt = new DataTable();
            string nombreSP = "PA_STATEMENT_IGuardarEncabezado";
            List<SQLParametro> lstParametros = new List<SQLParametro>();
            SQLParametro parametro = new SQLParametro();
            parametro.NombreParametro = "@pActualizar";
            parametro.ValorParametro = pActualizar;
            parametro.TipoParametro = SqlDbType.Bit;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pIdArchivo";
            parametro.ValorParametro = pIdArchivo;
            parametro.TipoParametro = SqlDbType.Int;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pDescripcionArchivo";
            parametro.ValorParametro = pDescripcionArchivo;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 50;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pNumLineasArchivo";
            parametro.ValorParametro = pNumLineasArchivo;
            parametro.TipoParametro = SqlDbType.Int;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pNumMovimientos";
            parametro.ValorParametro = pNumMovimientos;
            parametro.TipoParametro = SqlDbType.Int;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pAnno";
            parametro.ValorParametro = pAnno;
            parametro.TipoParametro = SqlDbType.SmallInt;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pMes";
            parametro.ValorParametro = pMes;
            parametro.TipoParametro = SqlDbType.SmallInt;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);            
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pTipoRegistro";
            parametro.ValorParametro = pTipoRegistro;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 1;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pFechaProceso";
            parametro.ValorParametro = pFechaProceso;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 8;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pNumeroSecuencialArchivo";
            parametro.ValorParametro = pNumeroSecuencialArchivo;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 5;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pCodigoProducto";
            parametro.ValorParametro = pCodigoProducto;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 2;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pDescripcionProducto";
            parametro.ValorParametro = pDescripcionProducto;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 30;
            lstParametros.Add(parametro);
            dt = conexion.EjecutarComando(nombreSP, lstParametros);
            //if (dt != null && dt.Rows.Count > 0)
            //{

            //}
            return dt;
        }
        public DataTable InsertarDetalle(int pIdArchivo, int pIdMovimiento, string pTipoRegistro, string pValores)
        {
            List<StatementDetail> lstDetalle = new List<StatementDetail>();
            DataTable dt = new DataTable();
            string nombreSP = "PA_STATEMENT_IGuardarDetalle";
            List<SQLParametro> lstParametros = new List<SQLParametro>();
            SQLParametro parametro = new SQLParametro();
            parametro.NombreParametro = "@pIdArchivo";
            parametro.ValorParametro = pIdArchivo;
            parametro.TipoParametro = SqlDbType.Int;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pIdMovimiento";
            parametro.ValorParametro = pIdMovimiento;
            parametro.TipoParametro = SqlDbType.Int;
            parametro.LongitudParametro = 0;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pTipoRegistro";
            parametro.ValorParametro = pTipoRegistro;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 1;
            lstParametros.Add(parametro);
            parametro = new SQLParametro();
            parametro.NombreParametro = "@pValor";
            parametro.ValorParametro = pValores;
            parametro.TipoParametro = SqlDbType.VarChar;
            parametro.LongitudParametro = 2000;
            lstParametros.Add(parametro);
            dt = conexion.EjecutarComando(nombreSP, lstParametros);
            return dt;
        }

    }
}
