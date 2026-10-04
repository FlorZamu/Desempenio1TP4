<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compras.aspx.cs" Inherits="TP4Compras.Compras" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Compras - TP4 Registro de compras</title>
    <link href="estilos.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="menu">
            <a href="Default.aspx">Inicio</a>
            <a href="Proveedores.aspx">Proveedores</a>
            <a href="Monedas.aspx">Monedas</a>
            <a href="Compras.aspx">Compras</a>
            <a href="ComprasEnPesos.aspx">Compras en pesos</a>
            <a href="VerLog.aspx">Log</a>
        </div>
        <div class="contenido">
            <h1>ABM de Compras</h1>

            <div class="caja">
                <h2><asp:Label ID="lblTitulo" runat="server" Text="Nueva compra"></asp:Label></h2>
                <asp:HiddenField ID="hfId" runat="server" />
                <div class="campo">
                    <label>Fecha</label>
                    <asp:TextBox ID="txtFecha" runat="server" TextMode="Date"></asp:TextBox>
                </div>
                <div class="campo">
                    <label>Proveedor</label>
                    <asp:DropDownList ID="ddlProveedor" runat="server"></asp:DropDownList>
                </div>
                <div class="campo">
                    <label>Punto de venta</label>
                    <asp:TextBox ID="txtPuntoVenta" runat="server" TextMode="Number" min="1"></asp:TextBox>
                </div>
                <div class="campo">
                    <label>Número de factura</label>
                    <asp:TextBox ID="txtNumeroFactura" runat="server" TextMode="Number" min="1"></asp:TextBox>
                </div>
                <div class="campo">
                    <label>Moneda</label>
                    <asp:DropDownList ID="ddlMoneda" runat="server"></asp:DropDownList>
                </div>
                <div class="campo">
                    <label>Monto gravado</label>
                    <asp:TextBox ID="txtMontoGravado" runat="server" TextMode="Number" step="0.01" min="0"></asp:TextBox>
                </div>
                <div class="campo">
                    <label>IVA</label>
                    <asp:TextBox ID="txtIva" runat="server" TextMode="Number" step="0.01" min="0"></asp:TextBox>
                </div>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton boton-gris" OnClick="btnCancelar_Click" />
                <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
            </div>

            <asp:GridView ID="gvCompras" runat="server" AutoGenerateColumns="False" DataKeyNames="id" CssClass="grilla"
                EmptyDataText="No hay compras cargadas."
                OnSelectedIndexChanged="gvCompras_SelectedIndexChanged" OnRowDeleting="gvCompras_RowDeleting">
                <SelectedRowStyle CssClass="seleccionada" />
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="Id" />
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="razonSocial" HeaderText="Proveedor" />
                    <asp:TemplateField HeaderText="Factura">
                        <ItemTemplate>
                            <%# string.Format("{0:0000}-{1:00000000}", Eval("puntoVenta"), Eval("numeroFactura")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="montoGravado" HeaderText="Monto gravado" DataFormatString="{0:N2}" ItemStyle-CssClass="numero" />
                    <asp:BoundField DataField="iva" HeaderText="IVA" DataFormatString="{0:N2}" ItemStyle-CssClass="numero" />
                    <asp:BoundField DataField="total" HeaderText="Total" DataFormatString="{0:N2}" ItemStyle-CssClass="numero" />
                    <asp:CommandField ShowSelectButton="True" SelectText="Modificar" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Delete" Text="Eliminar"
                                OnClientClick="return confirm('¿Seguro que querés eliminar esta compra?');"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
