namespace Simulation.Entities;

public class SupportRequest
{
    public Guid  Id { get; set; }
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; }
    public Status Status { get; set; }
    public DateTimeOffset CreatedAt = DateTimeOffset.UtcNow;
}