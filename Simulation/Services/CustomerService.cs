using Microsoft.EntityFrameworkCore;
using PlainOutcomes;
using PlainOutcomes.Lib;
using Simulation.Dtos.Create;
using Simulation.Entities;
using Simulation.Interfaces;
using Simulation.Persistence.Context;

namespace Simulation.Services;

public class CustomerService: ICustomerService
{
    private readonly SupportDbContext _dbContext;

    public CustomerService(SupportDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Outcome<Customer>> CreateAsync(CreateCustomerRequest request, 
            CancellationToken cancellationToken)
        {
            var customer = new Customer { Name = request.Name };
    
            await _dbContext.Customers.AddAsync(customer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
    
            return Outcome<Customer>.Success(customer);
        }
    
        public async Task<Outcome<Customer>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (customer is not null) return Outcome<Customer>.Success(customer);
            
            var error = new OutcomeError("Customer.NotExisting", 
                $"The customer with id {id} does not exist.");
                
            return Outcome<Customer>.Failure(error);

        }
}