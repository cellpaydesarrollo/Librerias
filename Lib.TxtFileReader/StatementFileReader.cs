using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.TxtFileReader
{
    public class StatementFileReader
    {
        private string CarpetaTemporal { get; set; }
        private string NombreArchivo { get; set; }
        public StatementFileReader(string pNombreArchivo)
        {
            //this.CarpetaTemporal = pCarpetaTemporal;
            this.NombreArchivo = pNombreArchivo;
        }
        
        public string ReadStatement()
        {
            List<string> lstRuta = NombreArchivo.Split('\\').ToList<string>();
            List<string> lstNombreSeparado = lstRuta[lstRuta.Count - 1].Split('_').ToList<string>();
            List<string> lstNombre = lstNombreSeparado[lstNombreSeparado.Count - 1].Split('.').ToList<string>();
            StringBuilder respuesta = new StringBuilder();
            string resp = ExisteStatement(lstNombre[0]);
            if (string.IsNullOrEmpty(resp))
            {
                
                respuesta.AppendLine("Hora Inicio: " + DateTime.Now.ToString());
                //string CarpetaTemporal = Server.MapPath("Temp") + @"\";
                //CarpetaTemporal = Directory.GetCurrentDirectory();
                //CarpetaTemporal = @"C:\Users\52553\OneDrive\Escritorio\D\Trabajo\Proyectos\LayOutsTXT\STATEMENT\Statement\Archivos Prosa\";
                //string Ruta = CarpetaTemporal + NombreArchivo;
                string Ruta = NombreArchivo;
                string tipoRegistroEncabezado = "0";
                string tipoRegistroStatementHeader = "1";
                char[] caracterLF = new[] { '\n' }; //Salto de linea, LF Line Feed
                char caracterFinArchivoStatement = 'T';
                List<string> lineasSeparadas = File.ReadAllText(Ruta, Encoding.Default).Split(caracterLF, StringSplitOptions.None).ToList<string>();
                int contadorLineas = 0;
                int contadorRegistros = 0;
                string cadenaEncabezado = string.Empty;
                string TipoRegistroAux = string.Empty;
                string cadena = string.Empty;
                StatementWriter cs = new StatementWriter();
                List<Statement> lst = cs.ObtenerPosiciones();
                Statement encabezado = (from d in lst.AsEnumerable<Statement>()
                                        where d.TipoRegistro == tipoRegistroEncabezado //Encabezado
                                        select d).FirstOrDefault();
                Statement detalle = new Statement();
                int IdArchivo = 0;
                cadenaEncabezado = lineasSeparadas[0];
                IdArchivo = InterpretarEncabezado(NombreArchivo, lineasSeparadas.Count - 2, tipoRegistroEncabezado, cadenaEncabezado, encabezado);
                lineasSeparadas.RemoveAt(0);

                foreach (string s in lineasSeparadas)
                {
                    if (s.Length > 0 && s[0] != caracterFinArchivoStatement)
                    {
                        TipoRegistroAux = s[0].ToString();
                        if (TipoRegistroAux == tipoRegistroStatementHeader)
                        {
                            contadorRegistros++;
                        }
                        detalle = (from d in lst.AsEnumerable()
                                   where d.TipoRegistro == TipoRegistroAux
                                   select d).FirstOrDefault();
                        InterpretarLinea(TipoRegistroAux, s, detalle, string.Empty, IdArchivo, contadorRegistros);
                        contadorLineas++;
                    }
                    else
                    {
                        if (s.Length > 0 && s[0] == 'T')
                        {
                            //Fin de archivo
                            //btnArchivo.Visible = false;
                            //lblMensajes.Text += "<br/>Archivo " + NombreArchivo + " procesado.";
                            //lblMensajes.Text += "<br/>Numero de Lineas " + contadorLineas + " .";
                            //lblMensajes.Text += "<br/>Hora Fin: " + DateTime.Now.ToString();
                            respuesta.AppendLine("Archivo " + NombreArchivo + " procesado.");
                            respuesta.AppendLine("Numero de Lineas " + contadorLineas + " .");
                            respuesta.AppendLine("Hora Fin: " + DateTime.Now.ToString());
                            DataTable dt = cs.InsertarEncabezado(true, IdArchivo, string.Empty, contadorLineas, contadorRegistros, 0, 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                        }
                        else
                        {
                            if (s.Length == 0)
                            {

                            }
                        }
                    }
                }
            }
            else
            {
                respuesta.AppendLine(resp);
            }
            return respuesta.ToString();
        }
        private string ExisteStatement(string pDescripcionArchivo)
        {
            string respuesta = string.Empty;
            StatementWriter cs = new StatementWriter();
            respuesta = cs.ExisteStatement(pDescripcionArchivo);
            return respuesta;
        }
        private int InterpretarEncabezado(string pNombreArchivo, int pNumLineasArchivo, string pTipoRegistro, string pCadena, Statement pEncabezado)
        {
            CultureInfo ci = new CultureInfo("es-MX");
            int idArchivo = 0;
            List<string> lstRuta = pNombreArchivo.Split('\\').ToList<string>();
            List<string> lstNombreSeparado = lstRuta[lstRuta.Count - 1].Split('_').ToList<string>();
            List<string> lstNombre = lstNombreSeparado[lstNombreSeparado.Count - 1].Split('.').ToList<string>();
            string fechaArchivo112 = lstNombre[0];
            DateTime fechaArchivo;
            DateTime.TryParseExact(fechaArchivo112, "yyyyMMdd", ci, DateTimeStyles.None, out fechaArchivo);
            short Anno = 0;
            short Mes = 0;
            short.TryParse(fechaArchivo.Year.ToString(), out Anno);
            short.TryParse(fechaArchivo.Month.ToString(), out Mes);
            StatementWriter cs = new StatementWriter();
            if (pCadena.Length >= pEncabezado.Fin - pEncabezado.Inicio)
            {
                List<StatementDetail> lstDetalle = cs.ObtenerDetallePosiciones(pTipoRegistro);
                StringBuilder Valores = new StringBuilder();
                string cadenaAux = string.Empty;
                int posicionAux = 0;
                string TipoRegistro = string.Empty;
                string FechaProceso = string.Empty;
                string NumeroSecuencialArchivo = string.Empty;
                string CodigoProducto = string.Empty;
                string DescripcionProducto = string.Empty;
                foreach (StatementDetail detalle in lstDetalle)
                {
                    cadenaAux = pCadena.Substring(posicionAux, detalle.Longitud);
                    cadenaAux = cadenaAux.Replace(" ", "¬");
                    cadenaAux = cadenaAux.Replace("&", "&amp;");
                    switch (detalle.Posicion)
                    {
                        case 1:
                            TipoRegistro = cadenaAux;
                            break;
                        case 2:
                            FechaProceso = cadenaAux;
                            break;
                        case 3:
                            NumeroSecuencialArchivo = cadenaAux;
                            break;
                        case 4:
                            CodigoProducto = cadenaAux;
                            break;
                        case 5:
                            DescripcionProducto = cadenaAux;
                            break;
                        default:
                            break;
                    }
                    posicionAux += detalle.Longitud;
                }
                //Insertar en tabla
                DataTable dt = cs.InsertarEncabezado(false, 0, lstRuta[lstRuta.Count-1], 0, 0, Anno, Mes, TipoRegistro, FechaProceso, NumeroSecuencialArchivo, CodigoProducto, DescripcionProducto);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0]["IdArchivo"].ToString(), out idArchivo);
                }
            }
            else
            {
                //Insertar en log de errores
            }
            return idArchivo;
        }
        private void InterpretarLinea(string pTipoRegistro, string pCadena, Statement pLinea, string pNumCuenta, int pIdArchivo, int pIdMovimiento)
        {
            StatementWriter cs = new StatementWriter();
            DataTable resultado = new DataTable();
            if (pCadena.Length >= pLinea.Fin - pLinea.Inicio)
            {
                List<StatementDetail> lstDetalle = cs.ObtenerDetallePosiciones(pTipoRegistro);
                StringBuilder Valores = new StringBuilder();
                string cadenaAux = string.Empty;
                int posicionAux = 0;
                foreach (StatementDetail detalle in lstDetalle)
                {
                    cadenaAux = pCadena.Substring(posicionAux, detalle.Longitud);
                    cadenaAux = cadenaAux.Replace(" ", "¬");
                    cadenaAux = cadenaAux.Replace("&", "&amp;");
                    Valores.Append(cadenaAux);
                    Valores.Append("|");
                    posicionAux += detalle.Longitud;
                }
                if (Valores.ToString().EndsWith("|"))
                {
                    Valores.Remove(Valores.Length - 1, 1);
                }
                //Insertar en tabla
                resultado = cs.InsertarDetalle(pIdArchivo, pIdMovimiento, pTipoRegistro, Valores.ToString());
                if (resultado != null && resultado.Rows.Count > 0)
                {
                    //resultado.Rows[0][""].ToString();
                }
            }
            else
            {
                //Insertar en log de errores
            }
        }
    }
}
