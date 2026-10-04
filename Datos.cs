using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;

namespace TP4Compras
{
    public static class Datos
    {
        public const string ArchivoLog = "~/App_Data/log_abm.txt";

        private static readonly object bloqueoLog = new object();

        public static string CadenaConexion
        {
            get { return ConfigurationManager.ConnectionStrings["ConexionTP4"].ConnectionString; }
        }

        public static SqlParameter Parametro(string nombre, object valor)
        {
            if (valor == null)
            {
                valor = DBNull.Value;
            }
            if (valor is DateTime)
            {
                SqlParameter parametroFecha = new SqlParameter(nombre, SqlDbType.Date);
                parametroFecha.Value = ((DateTime)valor).Date;
                return parametroFecha;
            }
            return new SqlParameter(nombre, valor);
        }

        public static DataTable Consultar(string sql, params SqlParameter[] parametros)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddRange(parametros);
                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }
            return tabla;
        }

        public static int Ejecutar(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddRange(parametros);
                conexion.Open();
                return comando.ExecuteNonQuery();
            }
        }

        public static int Insertar(string sql, params SqlParameter[] parametros)
        {
            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(sql + "; SELECT CAST(SCOPE_IDENTITY() AS INT);", conexion))
            {
                comando.Parameters.AddRange(parametros);
                conexion.Open();
                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }
        public static void RegistrarLog(string operacion, string tabla, string detalle)
        {
            string ruta = HttpContext.Current.Server.MapPath(ArchivoLog);
            string linea = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                + " | " + operacion
                + " | " + tabla
                + " | " + detalle
                + Environment.NewLine;

            lock (bloqueoLog)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ruta));
                File.AppendAllText(ruta, linea, Encoding.UTF8);
            }
        }
        public static bool LeerDecimal(string texto, out decimal valor)
        {
            texto = (texto ?? "").Trim().Replace(',', '.');
            return decimal.TryParse(texto, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out valor);
        }
        public static string DecimalATexto(object valor)
        {
            if (valor == null || valor == DBNull.Value)
            {
                return "";
            }
            return Convert.ToDecimal(valor).ToString("0.00", CultureInfo.InvariantCulture);
        }
        public static string FechaATexto(object valor)
        {
            if (valor == null || valor == DBNull.Value)
            {
                return "";
            }
            return Convert.ToDateTime(valor).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        public static string MensajeError(Exception ex)
        {
            SqlException sqlEx = ex as SqlException;
            if (sqlEx != null)
            {
                if (sqlEx.Number == 547)
                {
                    return "No se puede realizar la operación porque el registro está relacionado con otros datos (por ejemplo, tiene compras cargadas).";
                }
                if (sqlEx.Number == 8115 || sqlEx.Number == 8152 || sqlEx.Number == 2628)
                {
                    return "Alguno de los valores ingresados es demasiado grande para la base de datos.";
                }
                if (sqlEx.Number == -1 || sqlEx.Number == 2 || sqlEx.Number == 53 || sqlEx.Number == 4060 || sqlEx.Number == 18456)
                {
                    return "No se pudo conectar a la base de datos. Revisá que hayas ejecutado BaseDeDatos.sql y que el servidor "
                        + "de la cadena de conexión del Web.config sea el correcto. Detalle: " + sqlEx.Message;
                }
            }
            return "Error: " + ex.Message;
        }
    }
}
