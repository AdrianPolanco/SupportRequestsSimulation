using PlainOutcomes.Lib;
using Simulation.Dtos.Create;
using Simulation.Entities;

namespace Simulation.Interfaces;

public interface ICustomerService
{
    Task<Outcome<Customer>> CreateAsync(CreateCustomerRequest request,
        CancellationToken cancellationToken);

    Task<Outcome<Customer>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}