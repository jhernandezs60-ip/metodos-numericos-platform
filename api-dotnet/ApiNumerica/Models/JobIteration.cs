using System.Text.Json.Serialization;

namespace ApiNumerica.Models;

public class JobIteration
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public int IterationNumber { get; set; }

    public string? Xi { get; set; }

    public double? Error { get; set; }

    public string? DataJson { get; set; }

    [JsonIgnore]
    public Job? Job { get; set; }
}