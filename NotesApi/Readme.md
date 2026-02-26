# Tecnologías usadas

El proyecto está hecho con:
.NET 8
ASP.NET Core
Entity Framework Core
SQLite
JWT (Json Web Token)
Swagger


# ¿Dónde se guardan los datos?

Los datos se guardan en un archivo llamado:

notes.db


Es una base de datos SQLite.

Dentro de ese archivo hay dos tablas:



## Tabla Users (Usuarios)

Guarda la información de los usuarios.

Tiene:

Id → identificador único
Email → correo del usuario
Password → contraseña
Role → rol (User o Admin)



## Tabla Notes (Notas)

Guarda las notas creadas por los usuarios.

Tiene:

Id → identificador único
Title → título de la nota
Content → contenido
OwnerUserId → id del usuario dueño
CreatedAt → fecha de creación



# Partes del programa

El programa está dividido en secciones.



## Controllers (Controladores)

Son las “puertas” por donde entran las peticiones.

Hay dos:

### AuthController

Se encarga de:

Registrar usuarios
Iniciar sesión

### NotesController

Se encarga de:

Crear notas
Ver notas
Eliminar notas

Este controlador está protegido.
Solo funciona si el usuario envía su token.



## Services (Servicios)

Contienen la lógica del programa.

Ejemplo:

Validar si la contraseña es correcta
Crear el token JWT



## Repositories (Repositorios)

Son los que hablan directamente con la base de datos.

Ejemplo:

Guardar usuario
Buscar usuario
Guardar nota
Borrar nota



## AppDbContext en Data

Es la conexión entre el programa y la base de datos.

Aquí se define que existen:

Users
Notes



# Endpoints



## Registrar usuario

Ruta:


POST /api/auth/register


Se envía:

json
{
  "email": "usuario@mail.com",
  "password": "123456",
  "role": "User"
}


El sistema guarda el usuario en la base de datos.



## Login

Ruta:


POST /api/auth/login


Se envía:

json
{
  "email": "usuario@mail.com",
  "password": "Hola123456"
}


Si los datos son correctos, el servidor devuelve:

json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}


Ese token es obligatorio para usar las notas.



## Ver notas

Ruta:


GET /api/notes


Requiere token.

Devuelve solo las notas del usuario que inició sesión.



## Crear nota

Ruta:


POST /api/notes


Requiere token.

Se envía:

json
{
  "title": "Mi nota",
  "content": "Contenido"
}


Se guarda en la base de datos con el usuario como dueño.



## Eliminar nota

Ruta:


DELETE /api/notes/{id}


Reglas:

Un usuario normal solo puede borrar sus propias notas.
Un Admin puede borrar cualquier nota.


# Seguridad

El sistema usa JWT.