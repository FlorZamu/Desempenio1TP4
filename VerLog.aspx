<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerLog.aspx.cs" Inherits="TP4Compras.VerLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Log - TP4 Registro de compras</title>
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
            <h1>Log de operaciones ABM</h1>
            <p>Archivo: <asp:Label ID="lblRuta" runat="server" Text=""></asp:Label></p>
            <pre class="log"><asp:Literal ID="litLog" runat="server" Mode="Encode"></asp:Literal></pre>
        </div>
    </form>
</body>
</html>
