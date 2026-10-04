using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TP4Compras
{
    public partial class Proveedores : System.Web.UI.Page
    {
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
                gvProveedores.DataSource = Datos.Consultar("SELECT id, razonSocial, cuit FROM Proveedores ORDER BY razonSocial");
                gvProveedores.DataBind();
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
            txtRazonSocial.Text = "";
            txtCuit.Text = "";
            lblTitulo.Text = "Nuevo proveedor";
            gvProveedores.SelectedIndex = -1;
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string razonSocial = txtRazonSocial.Text.Trim();
            string cuit = txtCuit.Text.Trim();
            string errores = "";

            if (razonSocial == string.Empty)
            {
                errores += "La razón social no puede estar vacía.<br/>";
            }
            if (cuit != string.Empty && (cuit.Length != 11 || !cuit.All(c => c >= '0' && c <= '9')))
            {
                errores += "El CUIT debe tener 11 números, sin guiones.<br/>";
            }
            if (errores != "")
            {
                MostrarMensaje(errores, false);
                return;
            }

            try
            {
                object valorCuit = cuit == string.Empty ? null : cuit;
                string detalle = "razonSocial=" + razonSocial + ", cuit=" + cuit;

                if (hfId.Value == "")
                {
                    int idNuevo = Datos.Insertar(
                        "INSERT INTO Proveedores (razonSocial, cuit) VALUES (@razonSocial, @cuit)",
                        Datos.Parametro("@razonSocial", razonSocial),
                        Datos.Parametro("@cuit", valorCuit));

                    Datos.RegistrarLog("ALTA", "Proveedores", "id=" + idNuevo + ", " + detalle);
                    MostrarMensaje("Proveedor agregado.", true);
                }
                else
                {
                    int id = Convert.ToInt32(hfId.Value);
                    int filas = Datos.Ejecutar(
                        "UPDATE Proveedores SET razonSocial = @razonSocial, cuit = @cuit WHERE id = @id",
                        Datos.Parametro("@razonSocial", razonSocial),
                        Datos.Parametro("@cuit", valorCuit),
                        Datos.Parametro("@id", id));

                    if (filas == 0)
                    {
                        MostrarMensaje("El proveedor ya no existe.", false);
                    }
                    else
                    {
                        Datos.RegistrarLog("MODIFICACION", "Proveedores", "id=" + id + ", " + detalle);
                        MostrarMensaje("Proveedor modificado.", true);
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
        protected void gvProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvProveedores.SelectedDataKey.Value);
                DataTable tabla = Datos.Consultar(
                    "SELECT id, razonSocial, cuit FROM Proveedores WHERE id = @id",
                    Datos.Parametro("@id", id));

                if (tabla.Rows.Count == 0)
                {
                    MostrarMensaje("El proveedor ya no existe.", false);
                    LimpiarFormulario();
                    CargarGrilla();
                    return;
                }

                DataRow fila = tabla.Rows[0];
                hfId.Value = id.ToString();
                txtRazonSocial.Text = fila["razonSocial"].ToString();
                txtCuit.Text = fila["cuit"].ToString();
                lblTitulo.Text = "Modificar proveedor Nº " + id;
                lblMensaje.Text = "";
            }
            catch (Exception ex)
            {
                MostrarMensaje(Datos.MensajeError(ex), false);
            }
        }
        protected void gvProveedores_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvProveedores.DataKeys[e.RowIndex].Value);

                DataTable tabla = Datos.Consultar(
                    "SELECT razonSocial, cuit FROM Proveedores WHERE id = @id",
                    Datos.Parametro("@id", id));
                string detalle = "id=" + id;
                if (tabla.Rows.Count > 0)
                {
                    detalle += ", razonSocial=" + tabla.Rows[0]["razonSocial"] + ", cuit=" + tabla.Rows[0]["cuit"];
                }

                int filas = Datos.Ejecutar("DELETE FROM Proveedores WHERE id = @id", Datos.Parametro("@id", id));
                if (filas > 0)
                {
                    Datos.RegistrarLog("BAJA", "Proveedores", detalle);
                    MostrarMensaje("Proveedor eliminado.", true);
                }
                else
                {
                    MostrarMensaje("El proveedor ya no existe.", false);
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
