using System.Text.Json.Serialization;

namespace Prueba_Ziur.Models;

public sealed class MovimientoInventario
{
    [JsonPropertyName("Codigo")]
    public int Codigo { get; set; }

    [JsonPropertyName("Descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    [JsonPropertyName("VActiva")]
    public bool VActiva { get; set; }
}
