# ISSD-TP4-FlorPerez

TP4 - Caso **Registro de compras**. Aplicación Web ASP.NET (Web Forms, .NET Framework 4.7.2) con SQL Server.

## Cómo ejecutarlo

1. **Crear la base.** Abrir `BaseDeDatos.sql` en SQL Server Management Studio (o en Visual Studio: *Ver > Explorador de objetos de SQL Server*) y ejecutarlo completo. Crea la base `ISSD-TP4-202601`, las tres tablas y algunos datos de prueba.
2. **Revisar la conexión.** En `Web.config`, en `Data Source`, tiene que estar el mismo servidor donde se ejecutó el script. Viene con `(localdb)\MSSQLLocalDB`; si se usa SQL Server Express hay que poner `.\SQLEXPRESS`.
3. **Abrir y ejecutar.** Abrir `TP4Compras.slnx` con Visual Studio y apretar F5. Se abre `Default.aspx` con el menú.

## Qué pide la consigna y dónde está

| Consigna | Dónde |
|---|---|
| Base de datos `ISSD-TP4-202601` con las tablas Compras, Proveedores y Moneda | `BaseDeDatos.sql` |
| Formulario web para cada ABM de tabla | `Proveedores.aspx`, `Monedas.aspx`, `Compras.aspx` |
| Log en archivo de texto con las operaciones ABM realizadas y la fecha | `Datos.RegistrarLog` escribe en `App_Data/log_abm.txt`. Se puede ver desde `VerLog.aspx` |
| Compras expresadas en pesos, en grilla, filtradas por fecha desde / hasta | `ComprasEnPesos.aspx` |

`Datos.cs` tiene el código que comparten todas las páginas: la conexión a la base, la ejecución de consultas con parámetros y la escritura del log.

Cada línea del log tiene este formato:

```
04/10/2026 15:30:12 | ALTA | Proveedores | id=4, razonSocial=Papelera Norte S.A., cuit=30444444442
```

Las compras se pasan a pesos multiplicando el importe por la cotización de su moneda: `(montoGravado + iva) * cotizacion`. La moneda "Pesos" tiene cotización 1.

## Ajustes al SQL de la consigna

El SQL de la consigna, tal como está escrito, no funciona en SQL Server. Se cambiaron tres cosas:

1. **`REFERENCES Monedas(id)` pasó a `REFERENCES Moneda(id)`.** La tabla se llama `Moneda`; con `Monedas` la tabla Compras no se puede crear.
2. **`detalle VARCHAR` pasó a `detalle VARCHAR(50)`.** En SQL Server un `VARCHAR` sin longitud guarda un solo caracter, así que no entra ni la palabra "Pesos".
3. **`cotizacion DECIMAL(4,2)` pasó a `DECIMAL(10,2)`.** `DECIMAL(4,2)` admite como máximo 99,99 y no entra una cotización de dólar o euro expresada en pesos.

Además, `Moneda` se crea antes que `Compras`, porque Compras tiene una clave foránea hacia ella.
