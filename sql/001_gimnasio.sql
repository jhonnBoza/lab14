-- Gimnasio Aura
-- Base de la semana 12/13. Si la base ya existe, no borra nada.
-- sqlcmd -S "(localdb)\MSSQLLocalDB" -E -f 65001 -i sql/001_gimnasio.sql

IF DB_ID(N'GimnasioDb') IS NULL
BEGIN
    CREATE DATABASE GimnasioDb;
END
GO

USE GimnasioDb;
GO

IF OBJECT_ID(N'dbo.Plan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[Plan] (
        PlanId          INT             IDENTITY(1,1) NOT NULL,
        Nombre          NVARCHAR(80)    NOT NULL,
        PrecioMensual   DECIMAL(10,2)   NOT NULL,
        DuracionMeses   INT             NOT NULL,
        Descripcion     NVARCHAR(300)   NULL,
        CONSTRAINT PK_Plan PRIMARY KEY (PlanId),
        CONSTRAINT UQ_Plan_Nombre UNIQUE (Nombre),
        CONSTRAINT CK_Plan_Precio CHECK (PrecioMensual >= 0 AND PrecioMensual <= 5000),
        CONSTRAINT CK_Plan_Duracion CHECK (DuracionMeses BETWEEN 1 AND 36)
    );
END
GO

IF OBJECT_ID(N'dbo.Miembro', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Miembro (
        MiembroId           INT             IDENTITY(1,1) NOT NULL,
        Nombres             NVARCHAR(80)    NOT NULL,
        Apellidos           NVARCHAR(80)    NOT NULL,
        Email               NVARCHAR(120)   NOT NULL,
        Telefono            NVARCHAR(20)    NULL,
        FechaInscripcion    DATE            NOT NULL,
        PlanId              INT             NOT NULL,
        CONSTRAINT PK_Miembro PRIMARY KEY (MiembroId),
        CONSTRAINT UQ_Miembro_Email UNIQUE (Email),
        CONSTRAINT FK_Miembro_Plan FOREIGN KEY (PlanId) REFERENCES dbo.[Plan] (PlanId)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.[Plan])
BEGIN
    INSERT INTO dbo.[Plan] (Nombre, PrecioMensual, DuracionMeses, Descripcion) VALUES
        (N'Básico', 49.90, 1, N'Acceso a sala de máquinas en horario regular.'),
        (N'Full', 89.90, 1, N'Sala, clases grupales y vestidores.'),
        (N'Anual', 799.00, 12, N'Plan full con compromiso de doce meses.');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Miembro)
BEGIN
    INSERT INTO dbo.Miembro (Nombres, Apellidos, Email, Telefono, FechaInscripcion, PlanId) VALUES
        (N'Lucía', N'Quispe', N'lucia.quispe@correo.com', N'987654321', '2026-01-15', 1),
        (N'Mateo', N'Rojas', N'mateo.rojas@correo.com', N'912345678', '2026-02-03', 2),
        (N'Sofía', N'Castro', N'sofia.castro@correo.com', N'998877665', '2026-03-20', 2),
        (N'Andrés', N'Paredes', N'andres.paredes@correo.com', NULL, '2026-04-11', 3),
        (N'Valeria', N'Huamán', N'valeria.huaman@correo.com', N'955443322', '2026-05-02', 1);
END
GO
