using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP4Compras
{
    public partial class Compras : System.Web.UI.Page
    {
        // Consulta que usa la grilla: trae el nombre del proveedor y de la moneda con JOIN
        private const string ConsultaGrilla =
            "SELECT c.id, c.fecha, p.razonSocial, c.puntoVenta, c.numeroFactura, m.detalle AS moneda, "
            + "c.montoGravado, c.iva, ISNULL(c.montoGravado, 0) + ISNULL(c.iva, 0) AS total "
            + "FROM Compras c "
            + "INNER JOIN Proveedores p ON p.id = c.idProveedor "
            + "INNER JOIN Moneda m ON m.id = c.idMoneda "
            + "ORDER BY c.fecha DESC, c.id DESC";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCombos();
                CargarGrilla();
            }
        }

       
        private void CargarCombos()
        {
            try
            {
                ddlProveedor.DataSource = Datos.Consultar("SELECT id, razonSocial FROM Proveedores ORDER BY razonSocial");
                ddlProveedor.DataTextField = "razonSocial";
                ddlProveedor.DataValueField = "id";
                ddlProveedor.DataBind();
                ddlProveedor.Items.Insert(0, new ListItem("Elija un proveedor", "0"));

                ddlMoneda.DataSource = Datos.Consultar("SELECT id, detalle FROM Moneda ORDER BY id");
                ddlMoneda.DataTextField = "detalle";
                ddlMoneda.DataValueField = "id";
                ddlMoneda.DataBind();
                ddlMoneda.Items.Insert(0, new ListItem("Elija una moneda", "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje(Datos.MensajeError(ex), false);
            }
        }

      
        private void CargarGrilla()
        {
            try
            {
                gvCompras.DataSource = Datos.Consultar(ConsultaGrilla);
                gvCompras.DataBind();
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
            txtFecha.Text = "";
            txtPuntoVenta.Text = "";
            txtNumeroFactura.Text = "";
            txtMontoGravado.Text = "";
            txtIva.Text = "";
            ddlProveedor.ClearSelection();
            ddlMoneda.ClearSelection();
            lblTitulo.Text = "Nueva compra";
            gvCompras.SelectedIndex = -1;
        }

     
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DateTime fecha;
            int puntoVenta;
            int numeroFactura;
            decimal montoGravado;
            decimal iva;
            string errores = "";

            if (!DateTime.TryParse(txtFecha.Text, out fecha))
            {
                errores += "La fecha no es válida.<br/>";
            }
            if (ddlProveedor.SelectedValue == "0" || ddlProveedor.SelectedValue == "")
            {
                errores += "Debe elegir un proveedor.<br/>";
            }
            if (!int.TryParse(txtPuntoVenta.Text, out puntoVenta) || puntoVenta <= 0)
            {
                errores += "El punto de venta debe ser un número entero mayor que cero.<br/>";
            }
            if (!int.TryParse(txtNumeroFactura.Text, out numeroFactura) || numeroFactura <= 0)
            {
                errores += "El número de factura debe ser un número entero mayor que cero.<br/>";
            }
            if (ddlMoneda.SelectedValue == "0" || ddlMoneda.SelectedValue == "")
            {
                errores += "Debe elegir una moneda.<br/>";
            }
            if (!Datos.LeerDecimal(txtMontoGravado.Text, out montoGravado) || montoGravado < 0)
            {
                errores += "El monto gravado debe ser un número mayor o igual que cero.<br/>";
            }
            if (!Datos.LeerDecimal(txtIva.Text, out iva) || iva < 0)
            {
                errores += "El IVA debe ser un número mayor o igual que cero.<br/>";
            }
            if (errores != "")
            {
                MostrarMensaje(errores, false);
                return;
            }

            try
            {
                int idProveedor = Convert.ToInt32(ddlProveedor.SelectedValue);
                int idMoneda = Convert.ToInt32(ddlMoneda.SelectedValue);

                string detalle = "fecha=" + fecha.ToString("dd/MM/yyyy")
                    + ", proveedor=" + ddlProveedor.SelectedItem.Text
                    + ", puntoVenta=" + puntoVenta
                    + ", numeroFactura=" + numeroFactura
                    + ", moneda=" + ddlMoneda.SelectedItem.Text
                    + ", montoGravado=" + Datos.DecimalATexto(montoGravado)
                    + ", iva=" + Datos.DecimalATexto(iva);

                if (hfId.Value == "")
                {
                    int idNuevo = Datos.Insertar(
                        "INSERT INTO Compras (fecha, montoGravado, iva, numeroFactura, puntoVenta, idProveedor, idMoneda) "
                        + "VALUES (@fecha, @montoGravado, @iva, @numeroFactura, @puntoVenta, @idProveedor, @idMoneda)",
                        Datos.Parametro("@fecha", fecha),
                        Datos.Parametro("@montoGravado", montoGravado),
                        Datos.Parametro("@iva", iva),
                        Datos.Parametro("@numeroFactura", numeroFactura),
                        Datos.Parametro("@puntoVenta", puntoVenta),
                        Datos.Parametro("@idProveedor", idProveedor),
                        Datos.Parametro("@idMoneda", idMoneda));

                    Datos.RegistrarLog("ALTA", "Compras", "id=" + idNuevo + ", " + detalle);
                    MostrarMensaje("Compra agregada.", true);
                }
                else
                {
                    int id = Convert.ToInt32(hfId.Value);
                    int filas = Datos.Ejecutar(
                        "UPDATE Compras SET fecha = @fecha, montoGravado = @montoGravado, iva = @iva, "
                        + "numeroFactura = @numeroFactura, puntoVenta = @puntoVenta, "
                        + "idProveedor = @idProveedor, idMoneda = @idMoneda WHERE id = @id",
                        Datos.Parametro("@fecha", fecha),
                        Datos.Parametro("@montoGravado", montoGravado),
                        Datos.Parametro("@iva", iva),
                        Datos.Parametro("@numeroFactura", numeroFactura),
                        Datos.Parametro("@puntoVenta", puntoVenta),
                        Datos.Parametro("@idProveedor", idProveedor),
                        Datos.Parametro("@idMoneda", idMoneda),
                        Datos.Parametro("@id", id));

                    if (filas == 0)
                    {
                        MostrarMensaje("La compra ya no existe.", false);
                    }
                    else
                    {
                        Datos.RegistrarLog("MODIFICACION", "Compras", "id=" + id + ", " + detalle);
                        MostrarMensaje("Compra modificada.", true);
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

        
        protected void gvCompras_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvCompras.SelectedDataKey.Value);
                DataTable tabla = Datos.Consultar(
                    "SELECT id, fecha, montoGravado, iva, numeroFactura, puntoVenta, idProveedor, idMoneda FROM Compras WHERE id = @id",
                    Datos.Parametro("@id", id));

                if (tabla.Rows.Count == 0)
                {
                    MostrarMensaje("La compra ya no existe.", false);
                    LimpiarFormulario();
                    CargarGrilla();
                    return;
                }

                
                CargarCombos();

                DataRow fila = tabla.Rows[0];
                hfId.Value = id.ToString();
                txtFecha.Text = Datos.FechaATexto(fila["fecha"]);
                txtPuntoVenta.Text = fila["puntoVenta"].ToString();
                txtNumeroFactura.Text = fila["numeroFactura"].ToString();
                txtMontoGravado.Text = Datos.DecimalATexto(fila["montoGravado"]);
                txtIva.Text = Datos.DecimalATexto(fila["iva"]);
                ddlProveedor.SelectedValue = fila["idProveedor"].ToString();
                ddlMoneda.SelectedValue = fila["idMoneda"].ToString();
                lblTitulo.Text = "Modificar compra Nº " + id;
                lblMensaje.Text = "";
            }
            catch (Exception ex)
            {
                MostrarMensaje(Datos.MensajeError(ex), false);
            }
        }

      
        protected void gvCompras_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvCompras.DataKeys[e.RowIndex].Value);

              
                DataTable tabla = Datos.Consultar(
                    "SELECT c.fecha, p.razonSocial, c.puntoVenta, c.numeroFactura, c.montoGravado, c.iva "
                    + "FROM Compras c INNER JOIN Proveedores p ON p.id = c.idProveedor WHERE c.id = @id",
                    Datos.Parametro("@id", id));
                string detalle = "id=" + id;
                if (tabla.Rows.Count > 0)
                {
                    DataRow fila = tabla.Rows[0];
                    detalle += ", fecha=" + string.Format("{0:dd/MM/yyyy}", fila["fecha"])
                        + ", proveedor=" + fila["razonSocial"]
                        + ", puntoVenta=" + fila["puntoVenta"]
                        + ", numeroFactura=" + fila["numeroFactura"]
                        + ", montoGravado=" + Datos.DecimalATexto(fila["montoGravado"])
                        + ", iva=" + Datos.DecimalATexto(fila["iva"]);
                }

                int filas = Datos.Ejecutar("DELETE FROM Compras WHERE id = @id", Datos.Parametro("@id", id));
                if (filas > 0)
                {
                    Datos.RegistrarLog("BAJA", "Compras", detalle);
                    MostrarMensaje("Compra eliminada.", true);
                }
                else
                {
                    MostrarMensaje("La compra ya no existe.", false);
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
