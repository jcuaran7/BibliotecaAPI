📚 Biblioteca API

API RESTful completa para la gestión de bibliotecas con sistema de préstamos, libros, autores, categorías y cálculo automático de multas por retraso.

🚀 Características

✅ CRUD completo de Autores, Categorías, Libros y Préstamos
✅ Validación de disponibilidad de libros
✅ Cálculo automático de multas por días de retraso
✅ Filtros avanzados por estado y usuario
✅ Sistema de estados (Activo, Devuelto, Vencido)
✅ DTOs para separación de capas
✅ Relaciones entre entidades (Libros → Autores → Categorías)

🛠️ Tecnologías

Framework: ASP.NET Core 8.0
ORM: Entity Framework Core
Base de datos: SQL Server
Arquitectura: API REST con DTOs
Documentación: Swagger / OpenAPI

📊 Endpoints
📖 Libros
Método	Endpoint	Descripción
GET	/api/Libros	Obtener todos los libros
GET	/api/Libros/{id}	Obtener un libro específico
POST	/api/Libros	Crear un nuevo libro
PUT	/api/Libros/{id}	Actualizar un libro
DELETE	/api/Libros/{id}	Eliminar un libro
Ejemplo POST
{
  "titulo": "Cien Años de Soledad",
  "isbn": "978-0307474728",
  "añoPublicacion": 1967,
  "autorId": 1,
  "categoriaId": 2
}

✍️ Autores
Método	Endpoint	Descripción
GET	/api/Autores	Obtener todos los autores
GET	/api/Autores/{id}	Obtener un autor específico
POST	/api/Autores	Crear un nuevo autor
PUT	/api/Autores/{id}	Actualizar un autor
DELETE	/api/Autores/{id}	Eliminar un autor
Ejemplo POST
{
  "nombre": "Gabriel García Márquez",
  "nacionalidad": "Colombiano",
  "fechaNacimiento": "1927-03-06"
}

🏷️ Categorías
Método	Endpoint	Descripción
GET	/api/Categorias	Obtener todas las categorías
GET	/api/Categorias/{id}	Obtener una categoría específica
POST	/api/Categorias	Crear una nueva categoría
PUT	/api/Categorias/{id}	Actualizar una categoría
DELETE	/api/Categorias/{id}	Eliminar una categoría
Ejemplo POST
{
  "nombre": "Ficción",
  "descripcion": "Novelas y cuentos de ficción literaria"
}

📋 Préstamos
Método	Endpoint	Descripción
GET	/api/Prestamos	Obtener todos los préstamos (con filtros opcionales)
GET	/api/Prestamos/{id}	Obtener un préstamo específico
POST	/api/Prestamos	Crear nuevo préstamo
PUT	/api/Prestamos/{id}	Devolver libro prestado
DELETE	/api/Prestamos/{id}	Eliminar préstamo
Ejemplos de filtros
GET /api/Prestamos?estado=Activo
GET /api/Prestamos?nombreUsuario=Juan
GET /api/Prestamos?estado=Vencido&nombreUsuario=Maria

Ejemplo POST
{
  "libroId": 1,
  "nombreUsuario": "Juan Pérez",
  "fechaPrestamo": "2026-02-11"
}

Ejemplo PUT (Devolver)
{
  "fechaDevolucion": "2026-02-20",
  "diasLimite": 7
}

📝 Modelos de Datos
📖 Libro
{
  "id": 1,
  "titulo": "Cien Años de Soledad",
  "isbn": "978-0307474728",
  "añoPublicacion": 1967,
  "autorId": 1,
  "categoriaId": 2
}

✍️ Autor
{
  "id": 1,
  "nombre": "Gabriel García Márquez",
  "nacionalidad": "Colombiano",
  "fechaNacimiento": "1927-03-06"
}

🏷️ Categoría
{
  "id": 1,
  "nombre": "Ficción",
  "descripcion": "Novelas y cuentos de ficción literaria"
}

📋 Préstamo (Response)
{
  "id": 1,
  "tituloLibro": "Cien Años de Soledad",
  "nombreUsuario": "Juan Pérez",
  "fechaPrestamo": "2026-02-09",
  "fechaDevolucion": "2026-02-20",
  "estado": "Vencido",
  "diasRetraso": 4,
  "multa": 4000
}

💰 Sistema de Multas

📅 Días permitidos: 7 días (configurable)

💵 Multa: $1000 por día de retraso

Cálculo automático
FechaLimite = FechaPrestamo + DiasLimite

Si FechaDevolucion > FechaLimite:
    Estado = "Vencido"
    DiasRetraso = (FechaDevolucion - FechaLimite).Days
    Multa = DiasRetraso × 1000

🔒 Validaciones
📋 Préstamos

✅ Verificación de existencia del libro
✅ Validación de disponibilidad
✅ Validación de fechas
✅ Prevención de devoluciones duplicadas
✅ Validación lógica (devolución > préstamo)
📖 Libros
✅ ISBN único
✅ Relación con autor y categoría existentes

🔗 Integridad Referencial
✅ No se pueden eliminar autores con libros asociados
✅ No se pueden eliminar categorías con libros asociados
✅ No se pueden eliminar libros con préstamos activos

📦 Instalación
1️⃣ Clonar repositorio
git clone https://github.com/tu-usuario/biblioteca-api.git
cd biblioteca-api

2️⃣ Restaurar paquetes
dotnet restore

3️⃣ Configurar cadena de conexión

En appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=BibliotecaDB;Trusted_Connection=true;TrustServerCertificate=true;"
}

4️⃣ Aplicar migraciones
dotnet ef database update

5️⃣ Ejecutar proyecto
dotnet run

6️⃣ Acceder a Swagger
https://localhost:7269/swagger

🏗️ Estructura del Proyecto
BibliotecaAPI/
├── Controllers/
│   ├── AutoresController.cs
│   ├── CategoriasController.cs
│   ├── LibrosController.cs
│   └── PrestamosController.cs
├── Models/
│   ├── Autor.cs
│   ├── Categoria.cs
│   ├── Libro.cs
│   └── Prestamo.cs
├── DTOs/
│   ├── AutorDTO.cs
│   ├── CategoriaDTO.cs
│   ├── LibroDTO.cs
│   ├── CreateLibroDTO.cs
│   ├── PrestamoDTO.cs
│   ├── CreatePrestamoDTO.cs
│   └── DevolverLibroDTO.cs
├── Data/
│   └── BibliotecaContext.cs
├── Migrations/
│   └── [archivos de migración]
└── Program.cs

🔗 Relaciones entre Entidades
Autor (1) ──────< Libro (N)
                    │
Categoría (1) ─────<┘
                    │
Libro (1) ──────< Préstamo (N)

🎯 Flujo Completo de Uso

1️⃣ Crear Autor
2️⃣ Crear Categoría
3️⃣ Crear Libro
4️⃣ Crear Préstamo
5️⃣ Devolver libro
6️⃣ Consultar filtros

🗃️ Base de Datos
Tablas principales

Autores
Categorias
Libros
Prestamos
Migraciones incluidas
Creación inicial de tablas
Actualización de campos
Snapshot del contexto

📚 Tecnologías y Patrones Implementados
✅ Repository Pattern con EF Core
✅ DTO Pattern
✅ Async/Await
✅ LINQ
✅ Include / ThenInclude
✅ Data Annotations
✅ Swagger

🧪 Pruebas
Swagger UI
Postman
Thunder Client

👨‍💻 Autor

Jeferson Cuaran Narvaez
GitHub: https://github.com/jcuaran7

LinkedIn: [https://linkedin.com/in/tu-perfil](https://www.linkedin.com/in/jeferson-cuaran-7ba629152/?locale=es)
