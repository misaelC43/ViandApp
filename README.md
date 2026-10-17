# ViandApp

Aplicación de escritorio (Avalonia UI) para gestionar un emprendimiento de viandas, con una Web API (ASP.NET Core) y base de datos SQLite.

## Estructura

- `AvaloniaApplication4/`: cliente de escritorio (Avalonia, MVVM, CommunityToolkit.Mvvm).
- `ViandApp.Api/`: servidor ASP.NET Core Web API con Controllers, Entity Framework Core y SQLite.

## Requisitos

- .NET SDK 10
- Visual Studio 2026 (o la CLI de `dotnet`)
- Herramienta de EF Core: `dotnet tool install --global dotnet-ef`

## Cómo ejecutarlo

1. Clonar el repositorio.
2. Crear la base de datos (desde la carpeta `ViandApp.Api`):
```
   dotnet ef database update
```
   Esto genera `viandapp.db` con las tablas y datos iniciales (4 categorías y 4 viandas).
3. Iniciar los dos proyectos a la vez:
   - En Visual Studio: clic derecho en la solución > Configurar proyectos de inicio > Varios proyectos de inicio, y poner `ViandApp.Api` y `AvaloniaApplication4` en **Inicio**.
   - O por terminal, en dos ventanas: `dotnet run` dentro de `ViandApp.Api` y `dotnet run` dentro de `AvaloniaApplication4`.
4. Iniciar sesión en la app con usuario `admin` y contraseña `1234`.

La API escucha en `https://localhost:7248`. Esa dirección está en `AvaloniaApplication4/Services/ApiService.cs`.

## Probar la API

Con la API corriendo, abrir `https://localhost:7248/scalar/v1`.

## Pantallas

1. **Login**: acceso con usuario fijo.
2. **Inicio**: pedidos pendientes y próximo turno, calculados desde la API.
3. **Menú**: listado de viandas, alta y baja lógica (activa/inactiva).
4. **Pedidos**: viandas activas y botón para pedir; la API calcula la hora de entrega (+45 min).
5. **Turnos**: listado de pedidos con cliente, horarios, estado y total.

## Modelo de datos

`Categoria` 1 - N `Vianda` N - N `Pedido` (mediante `DetallePedido`) N - 1 `Cliente`.

## API

| Controller | Endpoints |
|---|---|
| Viandas | GET, GET por id, POST, PUT, DELETE |
| Pedidos | GET (filtro por estado), GET por id, POST, PATCH estado |
| Clientes | GET, GET por id, POST |
| Categorias | GET |