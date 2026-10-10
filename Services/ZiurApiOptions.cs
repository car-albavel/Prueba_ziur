using System.ComponentModel.DataAnnotations;

namespace Prueba_Ziur.Services;

public sealed class ZiurApiOptions
{
    public const string SectionName = "ZiurApi";

    [Required, Url]
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>Se obtiene de User Secrets o variables de entorno; nunca del repositorio.</summary>
    [Required]
    public string Token { get; set; } = string.Empty;

    [Range(1, 300)]
    public int TimeoutSeconds { get; set; } = 30;
}
