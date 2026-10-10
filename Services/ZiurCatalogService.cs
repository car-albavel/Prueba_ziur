using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Prueba_Ziur.Models;

namespace Prueba_Ziur.Services;

public sealed class ZiurApiException(string message, Exception? inner = null) : Exception(message, inner);

public interface IZiurCatalogService
{
    Task<IReadOnlyList<MovimientoInventario>> GetMovimientosAsync(CancellationToken ct = default);
}

public sealed class ZiurCatalogService(
    HttpClient http,
    IOptions<ZiurApiOptions> options,
    ILogger<ZiurCatalogService> logger) : IZiurCatalogService
{
    public async Task<IReadOnlyList<MovimientoInventario>> GetMovimientosAsync(CancellationToken ct = default)
    {
        var opts = options.Value;
        using var request = new HttpRequestMessage(HttpMethod.Get, opts.Endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", opts.Token);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            using var response = await http.SendAsync(request, ct);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Ziur API respondió {Status}", (int)response.StatusCode);
                throw new ZiurApiException(response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden =>
                        "No autorizado: el token de acceso es inválido o ha expirado.",
                    HttpStatusCode.NotFound => "No se encontró el servicio solicitado.",
                    >= HttpStatusCode.InternalServerError => "El servidor de Ziur no está disponible. Intente más tarde.",
                    _ => $"La API devolvió un error ({(int)response.StatusCode})."
                });
            }

            var data = await response.Content.ReadFromJsonAsync<List<MovimientoInventario>>(ct);
            return data ?? [];
        }
        catch (ZiurApiException)
        {
            throw;
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            throw new ZiurApiException("La solicitud excedió el tiempo de espera.");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Error de red al consultar Ziur API");
            throw new ZiurApiException("No se pudo conectar con la API de Ziur. Verifique su conexión.", ex);
        }
        catch (System.Text.Json.JsonException ex)
        {
            logger.LogError(ex, "Respuesta de Ziur API con formato inesperado");
            throw new ZiurApiException("La respuesta de la API tiene un formato inesperado.", ex);
        }
    }
}
