<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComprasEnPesos.aspx.cs" Inherits="TP4Compras.ComprasEnPesos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Compras en pesos - TP4 Registro de compras</title>
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
            <h1>Compras expresadas en pesos</h1>

            <div class="caja">
                <h2>Filtro por fecha</h2>
                <div class="campo">
                    <label>Fecha desde</label>
                    <asp:TextBox ID="txtDesde" runat="server" TextMode="Date"></asp:TextBox>
                </div>
                <div class="campo">
                    <label>Fecha hasta</label>
                    <asp:TextBox ID="txtHasta" runat="server" TextMode="Date"></asp:TextBox>
                </div>
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="boton" OnClick="btnFiltrar_Click" />
                <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
            </div>

            <asp:GridView ID="gvCompras" runat="server" AutoGenerateColumns="False" CssClass="grilla"
                EmptyDataText="No hay compras entre esas fechas.">
                <Columns>
                    <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="razonSocial" HeaderText="Proveedor" />
                    <asp:TemplateField HeaderText="Factura">
                        <ItemTemplate>
                            <%# string.Format("{0:0000}-{1:00000000}", Eval("puntoVenta"), Eval("numeroFactura")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="totalOriginal" HeaderText="Total en moneda original" DataFormatString="{0:N2}" ItemStyle-CssClass="numero" />
                    <asp:BoundField DataField="cotizacion" HeaderText="Cotización" DataFormatString="{0:N2}" ItemStyle-CssClass="numero" />
                    <asp:BoundField DataField="gravadoPesos" HeaderText="Gravado en $" DataFormatString="{0:N2}" ItemStyle-CssClass="numero" />
                    <asp:BoundField DataField="ivaPesos" HeaderText="IVA en $" DataFormatString="{0:N2}" ItemStyle-CssClass="numero" />
                    <asp:BoundField DataField="totalPesos" HeaderText="Total en $" DataFormatString="{0:N2}" ItemStyle-CssClass="numero" />
                </Columns>
            </asp:GridView>

            <div class="total">
                <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
