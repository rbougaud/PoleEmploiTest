namespace Domain.Entities;

public class JobOffer
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Company { get; set; } = default!;
    public string ContractType { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Url { get; set; } = default!;
    public DateTime DatePosted { get; set; }
    public DateTime LastUpdated { get; set; }
}

