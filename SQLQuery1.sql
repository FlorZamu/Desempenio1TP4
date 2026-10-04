-- =============================================================
--  TP4 - Caso: Registro de compras
--  Base de datos: ISSD-TP4-202601
--  Tablas: Proveedores - Moneda - Compras
--
--  Ejecutar completo (F5) en SQL Server Management Studio o en
--  Visual Studio (Ver > Explorador de objetos de SQL Server).
--  Se puede ejecutar más de una vez: no duplica ni borra nada.
-- =============================================================

-- 1. Crear la base de datos (el nombre lleva corchetes porque tiene guiones)
IF DB_ID('ISSD-TP4-202601') IS NULL
    CREATE DATABASE [ISSD-TP4-202601];
GO

USE [ISSD-TP4-202601];
GO

-- 2. Tabla Proveedores (igual a la consigna)
IF OBJECT_ID('dbo.Proveedores') IS NULL
CREATE TABLE Proveedores (
    id INT PRIMARY KEY IDENTITY(1,1),
    razonSocial VARCHAR (150) NOT NULL,
    cuit VARCHAR (11)
);
GO

-- 3. Tabla Moneda
--    Se crea antes que Compras porque Compras tiene una clave foránea hacia ella.
--    Dos ajustes respecto del texto de la consigna (ver README):
--      * detalle VARCHAR(50): en SQL Server "VARCHAR" sin longitud guarda 1 solo caracter.
--      * cotizacion DECIMAL(10,2): con DECIMAL(4,2) el máximo es 99,99 y no entra
--        una cotización de dólar o euro en pesos.
IF OBJECT_ID('dbo.Moneda') IS NULL
CREATE TABLE Moneda (
    id INT PRIMARY KEY IDENTITY(1,1),
    detalle VARCHAR (50),
    fechaCotizacion DATE,
    cotizacion DECIMAL (10,2)
);
GO

-- 4. Tabla Compras
--    Ajuste respecto de la consigna: la clave foránea decía REFERENCES Monedas(id),
--    pero la tabla se llama Moneda.
IF OBJECT_ID('dbo.Compras') IS NULL
CREATE TABLE Compras (
    id INT PRIMARY KEY IDENTITY(1,1),
    fecha DATE,
    montoGravado DECIMAL (15,2),
    iva DECIMAL (14,2),
    numeroFactura INT,
    puntoVenta INT,
    idProveedor INT NOT NULL,
    idMoneda INT NOT NULL,
    FOREIGN KEY (idProveedor) REFERENCES Proveedores(id),
    FOREIGN KEY (idMoneda) REFERENCES Moneda(id)
);
GO

-- 5. Datos de prueba (solo si las tablas están vacías).
--    Las cotizaciones son valores de ejemplo: se cambian desde el formulario de Monedas.
IF NOT EXISTS (SELECT 1 FROM Moneda)
BEGIN
    INSERT INTO Moneda (detalle, fechaCotizacion, cotizacion) VALUES
        ('Pesos', '2026-10-01', 1.00),
        ('Dólar', '2026-10-01', 1400.00),
        ('Euro',  '2026-10-01', 1600.00);
END
GO

IF NOT EXISTS (SELECT 1 FROM Proveedores)
BEGIN
    INSERT INTO Proveedores (razonSocial, cuit) VALUES
        ('Distribuidora El Sol S.A.', '30111111118'),
        ('Librería Central S.R.L.',   '30222222226'),
        ('Tech Import S.A.',          '30333333334');
END
GO

IF NOT EXISTS (SELECT 1 FROM Compras)
   AND EXISTS (SELECT 1 FROM Proveedores WHERE cuit = '30111111118')
   AND EXISTS (SELECT 1 FROM Moneda WHERE detalle = 'Pesos')
BEGIN
    INSERT INTO Compras (fecha, montoGravado, iva, numeroFactura, puntoVenta, idProveedor, idMoneda)
    SELECT '2026-09-05', 100000.00, 21000.00, 1523, 1, p.id, m.id
    FROM Proveedores p, Moneda m WHERE p.cuit = '30111111118' AND m.detalle = 'Pesos';

    INSERT INTO Compras (fecha, montoGravado, iva, numeroFactura, puntoVenta, idProveedor, idMoneda)
    SELECT '2026-09-18', 45000.00, 9450.00, 880, 2, p.id, m.id
    FROM Proveedores p, Moneda m WHERE p.cuit = '30222222226' AND m.detalle = 'Pesos';

    INSERT INTO Compras (fecha, montoGravado, iva, numeroFactura, puntoVenta, idProveedor, idMoneda)
    SELECT '2026-09-25', 500.00, 105.00, 3071, 5, p.id, m.id
    FROM Proveedores p, Moneda m WHERE p.cuit = '30333333334' AND m.detalle = 'Dólar';

    INSERT INTO Compras (fecha, montoGravado, iva, numeroFactura, puntoVenta, idProveedor, idMoneda)
    SELECT '2026-10-02', 200.00, 42.00, 3102, 5, p.id, m.id
    FROM Proveedores p, Moneda m WHERE p.cuit = '30333333334' AND m.detalle = 'Euro';
END
GO

-- 6. Verificación rápida
SELECT * FROM Proveedores;
SELECT * FROM Moneda;
SELECT * FROM Compras;
