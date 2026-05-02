using System.Text.Json;

namespace ApiNumerica.DTOs;

public class CreateJobRequest
{
    public string Method { get; set; } = string.Empty;

    public JsonElement Parameters { get; set; }
}