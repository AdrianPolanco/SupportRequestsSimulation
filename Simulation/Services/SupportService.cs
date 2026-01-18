using Microsoft.EntityFrameworkCore;
using PlainOutcomes.Lib;
using Simulation.Dtos.Create;
using Simulation.Entities;
using Simulation.Interfaces;
using Simulation.Persistence.Context;

namespace Simulation.Services;

public class SupportService(SupportDbContext dbContext, ICustomerService customerService)
{
    public async Task<Outcome<CreateSupportDto>> CreateAsync(CreateSupportRequest request, 
        CancellationToken cancellationToken)
    {
        var customerResult = await customerService.GetByIdAsync(request.CustomerId, cancellationToken);

        if (!customerResult.IsSuccess)
        {
            return Outcome<CreateSupportDto>.Failure(customerResult.Error);
        }
        
        var supportRequest = new SupportRequest
        {
            CustomerId = request.CustomerId,
            Subject = request.Subject,
            Description = request.Description,
            Status = Status.Open,
            Priority = request.Priority
        };

        await dbContext.SupportRequests.AddAsync(supportRequest, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = new CreateSupportDto(supportRequest.Id, 
            supportRequest.CreatedAt);

        return Outcome<CreateSupportDto>.Success(response);
    }

    public async Task<Outcome<List<SupportRequest>>> GetAsync(CancellationToken cancellationToken)
    {
        var requests = await dbContext.SupportRequests
            .AsQueryable()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return Outcome<List<SupportRequest>>.Success(requests);
    }
}