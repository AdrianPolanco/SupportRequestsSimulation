using Simulation.Entities;

namespace Simulation.Dtos.Create;

public record CreateSupportDto(Guid Id, DateTimeOffset CreatedAt, Status Status = Status.Open);