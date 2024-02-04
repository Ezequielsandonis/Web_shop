
Tienda eCommerce ASP.NET Core MVC
Esta aplicación es un ejemplo de una tienda eCommerce desarrollada con ASP.NET Core MVC que utiliza PayPal Sandbox para simular compras online y tiene conexión a una base de datos SQL.

Características Principales
ASP.NET Core MVC: Utilicé el framework ASP.NET Core MVC para desarrollar la lógica de la aplicación web.
PayPal Sandbox: Integración con PayPal Sandbox para procesar pagos y simular compras online de manera segura.
Base de Datos SQL: La aplicación utiliza una base de datos SQL para almacenar información sobre productos, clientes, pedidos, direcciones de clientes, etc.
Operaciones CRUD: cada controlador tiene la posibilidad de crear, editar, eliminar cualquier dato de su respectivo modelo.
Vistas: Existen varias vistas, que permite una navegación mas cómoda por la app.
Usuarios : La app permite la creación de 3 tipos de usuarios con roles diferentes (clientes, administradores y staff), cada uno con acceso a diferentes vistas de la app con diferentes funcionalidades.
Gestión de Pedidos: Los usuarios pueden realizar pedidos, ver el historial de pedidos y administrar su carrito de compras.


Instalación
Clona este repositorio en tu máquina local.
Abre el proyecto en Visual Studio o tu editor de código preferido.
Configura la cadena de conexión a tu base de datos SQL en appsettings.json.
Ejecuta el comando dotnet ef database update en la consola de NuGet Package Manager para aplicar las migraciones y crear la base de datos.
Ejecuta la aplicación y navega a través del navegador web  para acceder a la tienda eCommerce.


Uso
Explora la tienda eCommerce, agrega productos al carrito de compras y realiza compras utilizando PayPal Sandbox.
Puedes registrarte como usuario para guardar tus datos de envío y revisar tu historial de pedidos. Y puedes registrarte como administrador para acceder al dashboard y gestionar diferentes aspectos de la app
Para simular compras debes contar con tu cuenta de paypal y usuario sanbdbox
