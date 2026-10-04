using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP4Compras
{
    public partial class ComprasEnPesos : System.Web.UI.Page
    {
        private const string ConsultaEnPesos =
            "SELECT c.fecha, p.razonSocial, c.puntoVenta, c.numeroFactura, m.detalle AS moneda, m.cotizacion, "
            + "ISNULL(c.montoGravado, 0) + ISNULL(c.iva, 0) AS totalOriginal, "
            + "CAST(ISNULL(c.montoGravado, 0) * m.cotizacion AS DECIMAL(18,2)) AS gravadoPesos, "
            + "CAST(ISNULL(c.iva, 0) * m.cotizacion AS DECIMAL(18,2)) AS ivaPesos, "
            + "CAST((ISNULL(c.montoGravado, 0) + ISNULL(c.iva, 0)) * m.cotizacion AS DECIMAL(18,2)) AS totalPesos "
            + "FROM Compras c "
            + "INNER JOIN Proveedores p ON p.id = c.idProveedor "
            + "INNER JOIN Moneda m ON m.id = c.idMoneda "
            + "WHERE c.fecha >= @desde AND c.fecha <= @hasta "
            + "ORDER BY c.fecha, c.id";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime hoy = DateTime.Today;
                txtDesde.Text = Datos.FechaATexto(new DateTime(hoy.Year, hoy.Month, 1));
                txtHasta.Text = Datos.FechaATexto(hoy);
                Filtrar();
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            Filtrar();
        }

        private void MostrarError(string texto)
        {
            lblMensaje.Text = texto;
            lblMensaje.CssClass = "error";
        }

        private void Filtrar()
        {
            DateTime desde;
            DateTime hasta;
            string errores = "";

            lblMensaje.Text = "";
            lblTotal.Text = "";

            if (!DateTime.TryParse(txtDesde.Text, out desde))
            {
                errores += "La fecha desde no es válida.<br/>";
            }
            if (!DateTime.TryParse(txtHasta.Text, out hasta))
            {
                errores += "La fecha hasta no es válida.<br/>";
            }
            if (errores == "" && desde > hasta)
            {
                errores += "La fecha desde no puede ser posterior a la fecha hasta.<br/>";
            }
            if (errores != "")
            {
                MostrarError(errores);
                gvCompras.DataSource = null;
                gvCompras.DataBind();
                return;
            }

            try
            {
                DataTable tabla = Datos.Consultar(ConsultaEnPesos,
                    Datos.Parametro("@desde", desde),
                    Datos.Parametro("@hasta", hasta));

                gvCompras.DataSource = tabla;
                gvCompras.DataBind();
                decimal total = 0;
                foreach (DataRow fila in tabla.Rows)
                {
                    total += Convert.ToDecimal(fila["totalPesos"]);
                }
                lblTotal.Text = "Compras encontradas: " + tabla.Rows.Count
                    + " - Total en pesos: $ " + total.ToString("N2");
            }
            catch (Exception ex)
            {
                MostrarError(Datos.MensajeError(ex));
            }
        }
    }
}
