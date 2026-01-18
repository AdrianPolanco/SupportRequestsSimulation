using Microsoft.EntityFrameworkCore;
using Moq;
using PlainOutcomes;
using PlainOutcomes.Lib;
using Simulation.Dtos.Create;
using Simulation.Entities;
using Simulation.Interfaces;
using Simulation.Persistence.Context;
using Simulation.Services;

namespace Simulation.Tests;

public class SupportServiceTests
{
    private readonly SupportService _supportService;
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly SupportDbContext _dbContext;

    public SupportServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<SupportDbContext>()
            .UseInMemoryDatabase("TestingDatabase").Options;

        _dbContext = new SupportDbContext(dbContextOptions);
        
        _customerServiceMock = new Mock<ICustomerService>();

        _supportService = new SupportService(_dbContext, _customerServiceMock.Object);
    }
    
    [Fact]
    public async Task ShouldNotCreateSupportRequest()
    {
        Guid id = Guid.NewGuid();
        CancellationToken ct = new CancellationToken();
        var error = new OutcomeError("Customer.NotExisting", $"The customer with id {id} does not exist");
        _customerServiceMock.Setup(csm => csm.GetByIdAsync(id, ct)).ReturnsAsync(
            Outcome<Customer>.Failure(error));

        var createSupportRequest = new CreateSupportRequest(id, 
            "Testing", "Testing", Priority.High);

        var supportRequestOutcome = await _supportService.CreateAsync(createSupportRequest, ct);
        
        Assert.False(supportRequestOutcome.IsSuccess);
        Assert.Equal(supportRequestOutcome.Error, error);
        Assert.Equal(supportRequestOutcome.Error.Code, error.Code);
        Assert.Equal(supportRequestOutcome.Error.Message, error.Message);
        var isInDatabase = await _dbContext.SupportRequests.AnyAsync(
            sr => sr.Id.Equals(id), ct);
        Assert.False(isInDatabase);
    }

    [Fact]
    public async Task ShouldCreateSupportRequest()
    {
        var createSupportRequest = new CreateSupportRequest(Guid.NewGuid(), 
            "Testing...", "Testing...", Priority.High);
        var ct = new CancellationToken();
        _customerServiceMock.Setup(csm => csm.GetByIdAsync(createSupportRequest.CustomerId, ct))
            .ReturnsAsync(Outcome<Customer>.Success(new Customer {Id = createSupportRequest.CustomerId, Name = "Customer Test"}));
        var createSupportOutcome = await _supportService.CreateAsync(createSupportRequest, ct);
        
        Assert.True(createSupportOutcome.IsSuccess);
        Assert.NotNull(createSupportOutcome.Value);
        Assert.Equal( Status.Open, createSupportOutcome.Value.Status);
        var inDatabase = await _dbContext.SupportRequests
            .AnyAsync(sr => sr.Id.Equals(createSupportOutcome.Value.Id), ct);
        Assert.True(inDatabase);
    }
}