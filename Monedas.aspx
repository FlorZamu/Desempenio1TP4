<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Monedas.aspx.cs" Inherits="TP4Compras.Monedas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Monedas - TP4 Registro de compras</title>
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
            <h1>ABM de Monedas</h1>

            <div class="caja">
                <h2><asp:Label ID="lblTitulo" runat="server" Text="Nueva moneda"></asp:Label></h2>
                <asp:HiddenField ID="hfId" runat="server" />
                <div class="campo">
                    <label>Detalle</label>
                    <asp:TextBox ID="txtDetalle" runat="server" MaxLength="50"></asp:TextBox>
                </div>
                <div class="campo">
                    <label>Fecha de cotización</label>
                    <asp:TextBox ID="txtFechaCotizacion" runat="server" TextMode="Date"></asp:TextBox>
                </div>
                <div class="campo">
                    <label>Cotización en pesos</label>
                    <asp:TextBox ID="txtCotizacion" runat="server" TextMode="Number" step="0.01" min="0"></asp:TextBox>
                </div>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton boton-gris" OnClick="btnCancelar_Click" />
                <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
            </div>

            <asp:GridView ID="gvMonedas" runat="server" AutoGenerateColumns="False" DataKeyNames="id" CssClass="grilla"
                EmptyDataText="No hay monedas cargadas."
                OnSelectedIndexChanged="gvMonedas_SelectedIndexChanged" OnRowDeleting="gvMonedas_RowDeleting">
                <SelectedRowStyle CssClass="seleccionada" />
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="Id" />
                    <asp:BoundField DataField="detalle" HeaderText="Detalle" />
                    <asp:BoundField DataField="fechaCotizacion" HeaderText="Fecha de cotización" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="cotizacion" HeaderText="Cotización en pesos" DataFormatString="{0:N2}" ItemStyle-CssClass="numero" />
                    <asp:CommandField ShowSelectButton="True" SelectText="Modificar" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Delete" Text="Eliminar"
                                OnClientClick="return confirm('¿Seguro que querés eliminar esta moneda?');"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
