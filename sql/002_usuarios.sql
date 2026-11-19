-- Usuarios del gimnasio. Correo único, clave en hash BCrypt y rol.
-- sqlcmd -S "(localdb)\MSSQLLocalDB" -E -f 65001 -i sql/002_usuarios.sql
--
-- Claves de prueba (no están guardadas en texto):
--   recepcion@gimnasioaura.com  /  Recepcion.2026  /  Recepcionista
--   entrena@gimnasioaura.com    /  Entrena.2026    /  Entrenador
--
-- El POST /api/auth/registro no recibe rol: la API deja Entrenador.
-- Para armar la recepcionista como pide la guía también se puede registrar
-- y después hacer:
--   UPDATE dbo.Usuario SET Rol = N'Recepcionista' WHERE Correo = N'recepcion@gimnasioaura.com';

USE GimnasioDb;
GO

IF OBJECT_ID(N'dbo.Usuario', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuario (
        UsuarioId   INT             IDENTITY(1,1) NOT NULL,
        Correo      NVARCHAR(120)   NOT NULL,
        ClaveHash   NVARCHAR(200)   NOT NULL,
        Rol         NVARCHAR(40)    NOT NULL,
        CONSTRAINT PK_Usuario PRIMARY KEY (UsuarioId),
        CONSTRAINT UQ_Usuario_Correo UNIQUE (Correo)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE Correo = N'recepcion@gimnasioaura.com')
BEGIN
    INSERT INTO dbo.Usuario (Correo, ClaveHash, Rol) VALUES (
        N'recepcion@gimnasioaura.com',
        N'$2a$11$3Tc7K5csTKRGHsaGS8oHYO.EOJiYXaUfWwvdktO4xpGWFr13B76uS',
        N'Recepcionista'
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE Correo = N'entrena@gimnasioaura.com')
BEGIN
    INSERT INTO dbo.Usuario (Correo, ClaveHash, Rol) VALUES (
        N'entrena@gimnasioaura.com',
        N'$2a$11$G0O1hLrDfGpbf0wGXpSjruYZboVaPs8yxrNNN.udmxsv2q7TDjUem',
        N'Entrenador'
    );
END
GO
