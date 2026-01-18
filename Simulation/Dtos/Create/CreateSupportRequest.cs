using System.ComponentModel.DataAnnotations;
using Simulation.Entities;

namespace Simulation.Dtos.Create;

public record CreateSupportRequest(
    [Required] Guid CustomerId,
    [Required]
    [MaxLength(100, ErrorMessage = "The subject must have a maximum of 100 characters.")] 
    string Subject,
    [Required]
    [MaxLength(250, ErrorMessage = "The description must have a maximum of 100 characters.")] 
    string Description,
    [Required]
    Priority Priority
    );