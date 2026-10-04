<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Proveedores.aspx.cs" Inherits="TP4Compras.Proveedores" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Proveedores - TP4 Registro de compras</title>
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
            <h1>ABM de Proveedores</h1>

            <div class="caja">
                <h2><asp:Label ID="lblTitulo" runat="server" Text="Nuevo proveedor"></asp:Label></h2>
                <asp:HiddenField ID="hfId" runat="server" />
                <div class="campo">
                    <label>Razón social</label>
                    <asp:TextBox ID="txtRazonSocial" runat="server" MaxLength="150"></asp:TextBox>
                </div>
                <div class="campo">
                    <label>CUIT (11 números)</label>
                    <asp:TextBox ID="txtCuit" runat="server" MaxLength="11"></asp:TextBox>
                </div>
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="boton" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton boton-gris" OnClick="btnCancelar_Click" />
                <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
            </div>

            <asp:GridView ID="gvProveedores" runat="server" AutoGenerateColumns="False" DataKeyNames="id" CssClass="grilla"
                EmptyDataText="No hay proveedores cargados."
                OnSelectedIndexChanged="gvProveedores_SelectedIndexChanged" OnRowDeleting="gvProveedores_RowDeleting">
                <SelectedRowStyle CssClass="seleccionada" />
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="Id" />
                    <asp:BoundField DataField="razonSocial" HeaderText="Razón social" />
                    <asp:BoundField DataField="cuit" HeaderText="CUIT" />
                    <asp:CommandField ShowSelectButton="True" SelectText="Modificar" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Delete" Text="Eliminar"
                                OnClientClick="return confirm('¿Seguro que querés eliminar este proveedor?');"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
