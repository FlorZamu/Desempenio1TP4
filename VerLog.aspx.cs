using System;
using System.IO;
using System.Web.UI;

namespace TP4Compras
{
    public partial class VerLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ruta = Server.MapPath(Datos.ArchivoLog);
            lblRuta.Text = Server.HtmlEncode(ruta);

            try
            {
                if (File.Exists(ruta))
                {
                    litLog.Text = File.ReadAllText(ruta);
                }
                else
                {
                    litLog.Text = "Todavía no se registró ninguna operación.";
                }
            }
            catch (Exception ex)
            {
                litLog.Text = "No se pudo leer el archivo de log: " + ex.Message;
            }
        }
    }
}
