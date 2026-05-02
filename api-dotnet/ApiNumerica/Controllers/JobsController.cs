using ApiNumerica.Data;
using ApiNumerica.DTOs;
using ApiNumerica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ApiNumerica.Controllers;

[ApiController]
[Route("jobs")]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _context;

    public JobsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Method))
        {
            return BadRequest(new
            {
                message = "El método numérico es obligatorio."
            });
        }

        var job = new Job
        {
            Method = request.Method.Trim().ToLower(),
            ParametersJson = JsonSerializer.Serialize(request.Parameters),
            Status = "PENDING",
            CreatedAt = DateTime.UtcNow
        };

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJobById), new { id = job.Id }, job);
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs(
        [FromQuery] string? status,
        [FromQuery] string? method)
    {
        var query = _context.Jobs.AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(j => j.Status.ToLower() == status.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(method))
        {
            query = query.Where(j => j.Method.ToLower() == method.ToLower());
        }

        var jobs = await query
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();

        return Ok(jobs);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetJobById(int id)
    {
        var job = await _context.Jobs
            .Include(j => j.Iterations)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
        {
            return NotFound(new
            {
                message = "Job no encontrado."
            });
        }

        return Ok(job);
    }

    [HttpGet("{id:int}/iterations")]
    public async Task<IActionResult> GetJobIterations(int id)
    {
        var jobExists = await _context.Jobs.AnyAsync(j => j.Id == id);

        if (!jobExists)
        {
            return NotFound(new
            {
                message = "Job no encontrado."
            });
        }

        var iterations = await _context.JobIterations
            .Where(i => i.JobId == id)
            .OrderBy(i => i.IterationNumber)
            .ToListAsync();

        return Ok(iterations);
    }

    [HttpPost("{id:int}/simulate")]
    public async Task<IActionResult> SimulateJob(int id)
    {
        var job = await _context.Jobs
            .Include(j => j.Iterations)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
        {
            return NotFound(new
            {
                message = "Job no encontrado."
            });
        }

        if (job.Status == "DONE" || job.Status == "RUNNING")
        {
            return BadRequest(new
            {
                message = "Este job ya fue ejecutado o está en ejecución."
            });
        }

        job.Status = "RUNNING";
        job.StartedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var iterations = new List<JobIteration>
        {
            new JobIteration
            {
                JobId = job.Id,
                IterationNumber = 1,
                Xi = "1.500000",
                Error = 100,
                DataJson = "{\"fx\":\"-0.125000\"}"
            },
            new JobIteration
            {
                JobId = job.Id,
                IterationNumber = 2,
                Xi = "1.521739",
                Error = 1.428571,
                DataJson = "{\"fx\":\"0.002137\"}"
            },
            new JobIteration
            {
                JobId = job.Id,
                IterationNumber = 3,
                Xi = "1.521380",
                Error = 0.023596,
                DataJson = "{\"fx\":\"0.000001\"}"
            }
        };

        _context.JobIterations.AddRange(iterations);

        job.Status = "DONE";
        job.Result = "1.521380";
        job.Converged = true;
        job.FinishedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Job simulado correctamente.",
            job.Id,
            job.Method,
            job.Status,
            job.Result,
            job.Converged
        });
    }
}