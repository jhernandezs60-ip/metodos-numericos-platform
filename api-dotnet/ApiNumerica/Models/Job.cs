namespace ApiNumerica.Models;

public class Job
{
    public int Id { get; set; }

    public string Method { get; set; } = string.Empty;

    public string ParametersJson { get; set; } = string.Empty;

    public string Status { get; set; } = "PENDING";

    public string? Result { get; set; }

    public bool? Converged { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public List<JobIteration> Iterations { get; set; } = new();
}