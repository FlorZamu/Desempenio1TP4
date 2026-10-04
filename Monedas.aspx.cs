using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP4Compras
{
    public partial class Monedas : System.Web.UI.Page
    {
        private const decimal CotizacionMaxima = 99999999.99m;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }
        private void CargarGrilla()
        {
            try
            {
                gvMonedas.DataSource = Datos.Consultar("SELECT id, detalle, fechaCotizacion, cotizacion FROM Moneda ORDER BY id");
                gvMonedas.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje(Datos.MensajeError(ex), false);
            }
        }

        private void MostrarMensaje(string texto, bool correcto)
        {
            lblMensaje.Text = texto;
            lblMensaje.CssClass = correcto ? "ok" : "error";
        }
        private void LimpiarFormulario()
        {
            hfId.Value = "";
            txtDetalle.Text = "";
            txtFechaCotizacion.Text = "";
            txtCotizacion.Text = "";
            lblTitulo.Text = "Nueva moneda";
            gvMonedas.SelectedIndex = -1;
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string detalleMoneda = txtDetalle.Text.Trim();
            DateTime fecha;
            decimal cotizacion;
            string errores = "";

            if (detalleMoneda == string.Empty)
            {
                errores += "El detalle no puede estar vacío.<br/>";
            }
            if (!DateTime.TryParse(txtFechaCotizacion.Text, out fecha))
            {
                errores += "La fecha de cotización no es válida.<br/>";
            }
            if (!Datos.LeerDecimal(txtCotizacion.Text, out cotizacion) || cotizacion <= 0 || cotizacion > CotizacionMaxima)
            {
                errores += "La cotización debe ser un número mayor que cero.<br/>";
            }
            if (errores != "")
            {
                MostrarMensaje(errores, false);
                return;
            }

            try
            {
                string detalle = "detalle=" + detalleMoneda
                    + ", fechaCotizacion=" + fecha.ToString("dd/MM/yyyy")
                    + ", cotizacion=" + Datos.DecimalATexto(cotizacion);

                if (hfId.Value == "")
                {
                    int idNuevo = Datos.Insertar(
                        "INSERT INTO Moneda (detalle, fechaCotizacion, cotizacion) VALUES (@detalle, @fechaCotizacion, @cotizacion)",
                        Datos.Parametro("@detalle", detalleMoneda),
                        Datos.Parametro("@fechaCotizacion", fecha),
                        Datos.Parametro("@cotizacion", cotizacion));

                    Datos.RegistrarLog("ALTA", "Moneda", "id=" + idNuevo + ", " + detalle);
                    MostrarMensaje("Moneda agregada.", true);
                }
                else
                {
                    int id = Convert.ToInt32(hfId.Value);
                    int filas = Datos.Ejecutar(
                        "UPDATE Moneda SET detalle = @detalle, fechaCotizacion = @fechaCotizacion, cotizacion = @cotizacion WHERE id = @id",
                        Datos.Parametro("@detalle", detalleMoneda),
                        Datos.Parametro("@fechaCotizacion", fecha),
                        Datos.Parametro("@cotizacion", cotizacion),
                        Datos.Parametro("@id", id));

                    if (filas == 0)
                    {
                        MostrarMensaje("La moneda ya no existe.", false);
                    }
                    else
                    {
                        Datos.RegistrarLog("MODIFICACION", "Moneda", "id=" + id + ", " + detalle);
                        MostrarMensaje("Moneda modificada.", true);
                    }
                }

                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MostrarMensaje(Datos.MensajeError(ex), false);
            }

            CargarGrilla();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            lblMensaje.Text = "";
        }
        protected void gvMonedas_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvMonedas.SelectedDataKey.Value);
                DataTable tabla = Datos.Consultar(
                    "SELECT id, detalle, fechaCotizacion, cotizacion FROM Moneda WHERE id = @id",
                    Datos.Parametro("@id", id));

                if (tabla.Rows.Count == 0)
                {
                    MostrarMensaje("La moneda ya no existe.", false);
                    LimpiarFormulario();
                    CargarGrilla();
                    return;
                }

                DataRow fila = tabla.Rows[0];
                hfId.Value = id.ToString();
                txtDetalle.Text = fila["detalle"].ToString();
                txtFechaCotizacion.Text = Datos.FechaATexto(fila["fechaCotizacion"]);
                txtCotizacion.Text = Datos.DecimalATexto(fila["cotizacion"]);
                lblTitulo.Text = "Modificar moneda Nº " + id;
                lblMensaje.Text = "";
            }
            catch (Exception ex)
            {
                MostrarMensaje(Datos.MensajeError(ex), false);
            }
        }
        protected void gvMonedas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvMonedas.DataKeys[e.RowIndex].Value);

                DataTable tabla = Datos.Consultar(
                    "SELECT detalle, cotizacion FROM Moneda WHERE id = @id",
                    Datos.Parametro("@id", id));
                string detalle = "id=" + id;
                if (tabla.Rows.Count > 0)
                {
                    detalle += ", detalle=" + tabla.Rows[0]["detalle"]
                        + ", cotizacion=" + Datos.DecimalATexto(tabla.Rows[0]["cotizacion"]);
                }

                int filas = Datos.Ejecutar("DELETE FROM Moneda WHERE id = @id", Datos.Parametro("@id", id));
                if (filas > 0)
                {
                    Datos.RegistrarLog("BAJA", "Moneda", detalle);
                    MostrarMensaje("Moneda eliminada.", true);
                }
                else
                {
                    MostrarMensaje("La moneda ya no existe.", false);
                }

                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MostrarMensaje(Datos.MensajeError(ex), false);
            }

            CargarGrilla();
        }
    }
}
