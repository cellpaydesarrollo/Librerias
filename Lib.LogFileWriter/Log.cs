using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;

namespace Lib.LogFileWriter
{
    public class Log
    {
        private string Nombre { get; set; }
        private string Ruta { get; set; }
        private string NombrePrefijo { get; set; }
        private string NombreFecha { get; set; }
        private string NombrePath { get; set; }
        private static Logger logger;
        public Log(string pNombre, string pRuta)
        {
            this.NombrePrefijo = pNombre;
            this.Ruta = pRuta;
            this.NombreFecha = DateTime.Now.ToString("yyyyMMdd");
            this.Nombre = "Log_" + NombrePrefijo + "_" + NombreFecha + ".txt";
            this.NombrePath = Ruta + @"\" + Nombre;
            //if (!File.Exists(NombrePath))
            //{
            //    File.Create(NombrePath);
            //}

            logger = LogManager.GetCurrentClassLogger();
            logger.Error(new Exception("excepcion generica"), "error generico");
        }

    

        public void AgregarLinea(StringBuilder pContenido)
        {
            
        }
    }
}
