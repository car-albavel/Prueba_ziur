# Prueba_ziur

Blazor Web App (.NET 10, Interactive Server) que consume la API de Ziur y muestra movimientos de inventario en una grilla (/movimientos) con busqueda, ordenamiento y estados de carga/error.

## Arquitectura
- Models/MovimientoInventario: DTO.
- Services/ZiurApiOptions: configuracion tipada validada al iniciar.
- Services/ZiurCatalogService: HttpClient tipado, Bearer token, timeout y manejo de errores.
- Components/Pages/Movimientos.razor: UI.

## Seguridad
El token se usa solo en el servidor y no esta en el repositorio. Configurar con:

    dotnet user-secrets set ZiurApi:Token <TOKEN> --project Prueba_Ziur.csproj

En produccion usar la variable de entorno ZiurApi__Token.

## Ejecucion
dotnet run y abrir /movimientos
