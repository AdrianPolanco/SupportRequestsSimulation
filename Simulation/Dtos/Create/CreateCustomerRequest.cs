using System.ComponentModel.DataAnnotations;

namespace Simulation.Dtos.Create;

public record CreateCustomerRequest(
    [Required(ErrorMessage = "Name is required!")] string Name);