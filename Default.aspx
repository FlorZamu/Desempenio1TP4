<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TP4Compras._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>TP4 - Registro de compras</title>
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
            <h1>TP4 - Registro de compras</h1>
            <div class="caja">
                <h2>Menú</h2>
                <ul class="lista-menu">
                    <li><a href="Proveedores.aspx">ABM de Proveedores</a></li>
                    <li><a href="Monedas.aspx">ABM de Monedas</a></li>
                    <li><a href="Compras.aspx">ABM de Compras</a></li>
                    <li><a href="ComprasEnPesos.aspx">Compras expresadas en pesos, filtradas por fecha</a></li>
                    <li><a href="VerLog.aspx">Log de operaciones ABM (archivo de texto)</a></li>
                </ul>
            </div>
        </div>
    </form>
</body>
</html>
