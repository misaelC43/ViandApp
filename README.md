# ViandApp

Aplicación de escritorio (Avalonia UI) para gestionar un emprendimiento de viandas, con una Web API (ASP.NET Core) y base de datos SQLite.

- **Materia:** Programación II – Orientada a Objetos
- **Integrante:** Misael (trabajo individual)
- **Temática:** gestión de menú, pedidos y turnos de entrega de un emprendimiento de viandas.

## Estructura

- `AvaloniaApplication4/`: cliente de escritorio (Avalonia, MVVM, CommunityToolkit.Mvvm).
- `ViandApp.Api/`: servidor ASP.NET Core Web API con Controllers, Entity Framework Core y SQLite.

## Requisitos

- .NET SDK 10
- Visual Studio 2026 (o la CLI de `dotnet`)
- Herramienta de EF Core: `dotnet tool install --global dotnet-ef`
- Certificado de desarrollo HTTPS de ASP.NET Core (una sola vez por PC): `dotnet dev-certs https --trust`

## Configuración de la conexión

- **Base de datos:** la cadena de conexión está en `ViandApp.Api/appsettings.json`, clave `ConnectionStrings:Default` (`Data Source=viandapp.db`). El archivo de base se crea junto al proyecto de la API.
- **Dirección de la API:** `https://localhost:7248` (definida en `ViandApp.Api/Properties/launchSettings.json`). El cliente la usa desde `AvaloniaApplication4/Services/ApiService.cs`. Si se cambia el puerto, hay que cambiarlo en los dos lugares.

## Cómo ejecutarlo

1. Clonar el repositorio.
2. Crear la base de datos (desde la carpeta `ViandApp.Api`):

```
    dotnet restore
    dotnet ef database update
```

    Restaura los paquetes y genera `viandapp.db` con las tablas y datos de prueba (4 categorías y 4 viandas).
3. **Iniciar primero la API:** en `ViandApp.Api`, ejecutar `dotnet run --launch-profile https` (importante: sin el perfil `https` la API no escucha en el puerto 7248 y la app no puede conectarse). También se puede iniciar desde Visual Studio con el perfil `https`.
4. **Después iniciar Avalonia:** en `AvaloniaApplication4`, ejecutar `dotnet run`.
    - En Visual Studio se pueden iniciar los dos a la vez: clic derecho en la solución > Configurar proyectos de inicio > Varios proyectos de inicio, con la API arriba de la lista.
5. Iniciar sesión con usuario `admin` y contraseña `1234`.

## Probar la API

Con la API corriendo, abrir `https://localhost:7248/scalar/v1`.

## Pantallas

1. **Login**: acceso con usuario fijo.
2. **Inicio**: pedidos pendientes y próximo turno, calculados desde la API.
3. **Menú**: listado de viandas, alta con formulario y baja lógica (activa/inactiva).
4. **Pedidos**: viandas activas y botón para pedir; la API calcula la hora de entrega (+45 min).
5. **Turnos**: listado de pedidos con cliente, horarios, estado y total.

## Entidades y relaciones

- `Categoria`: nombre.
- `Vianda`: nombre, descripción, precio, activa. Pertenece a una `Categoria`.
- `Cliente`: nombre, teléfono.
- `Pedido`: cliente, fecha, hora de entrega, estado.
- `DetallePedido`: vianda, cantidad y precio unitario de un `Pedido`.

`Categoria` 1 - N `Vianda` · `Cliente` 1 - N `Pedido` · `Pedido` 1 - N `DetallePedido` N - 1 `Vianda`.

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/viandas` | Lista viandas (`?soloActivas=true` filtra las disponibles) |
| GET | `/api/viandas/{id}` | Una vianda |
| POST | `/api/viandas` | Crea una vianda |
| PUT | `/api/viandas/{id}` | Modifica una vianda |
| DELETE | `/api/viandas/{id}` | Elimina una vianda (409 si ya fue pedida) |
| GET | `/api/pedidos` | Lista pedidos (`?estado=Pendiente` filtra) |
| GET | `/api/pedidos/{id}` | Un pedido |
| POST | `/api/pedidos` | Crea un pedido (la API calcula hora de entrega y precios) |
| PATCH | `/api/pedidos/{id}/estado` | Cambia el estado del pedido |
| GET | `/api/clientes` | Lista clientes |
| GET | `/api/clientes/{id}` | Un cliente |
| POST | `/api/clientes` | Crea un cliente |
| GET | `/api/categorias` | Lista categorías |