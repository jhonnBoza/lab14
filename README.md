# Laboratorio 14 — API en capas con JWT (Gimnasio Aura)

Desarrollo de Aplicaciones Empresariales Avanzadas. Semana 14.
Arévalo Sermeño, Edwin William.

Sigue el gimnasio de las semanas anteriores. La solución quedó en cuatro proyectos: Entidades, Datos, Negocio y Api. Esta semana se agregó la tabla Usuario y el token JWT.

## Qué hace cada rol

- **Recepcionista**: ve planes y miembros, crea y edita planes, inscribe gente y la puede dar de baja.
- **Entrenador**: consulta planes y miembros. No cambia precios ni inscribe. Si alguien se registra por la API, el servidor le deja este rol. El cliente no manda el rol.

## Cómo correrlo

La clave del token y la cadena de conexión no van en el repositorio. Hay que copiar el ejemplo:

```powershell
copy Gimnasio.Api\appsettings.Development.example.json Gimnasio.Api\appsettings.Development.json
```

En ese archivo pon una `Jwt:Key` de al menos 32 caracteres y la cadena de LocalDB, por ejemplo:

`Server=(localdb)\MSSQLLocalDB;Database=GimnasioDb;Trusted_Connection=True;TrustServerCertificate=True`

Luego:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -f 65001 -i sql/001_gimnasio.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -f 65001 -i sql/002_usuarios.sql
dotnet run --project Gimnasio.Api --launch-profile http
```

Swagger: http://localhost:5202/swagger

En Authorize se pega solo el token (Swagger agrega Bearer).

## Usuarios de prueba

| Correo | Clave | Rol |
| --- | --- | --- |
| recepcion@gimnasioaura.com | Recepcion.2026 | Recepcionista |
| entrena@gimnasioaura.com | Entrena.2026 | Entrenador |

Las claves en la base están con BCrypt. El script `sql/002_usuarios.sql` trae los hash.
