namespace Domain.Entities;

public class Commune
{
    public string Code { get; set; } = default!;
    public string Libelle { get; set; } = default!;
    public string CodePostal { get; set; } = default!;
    public string CodeDepartement { get; set; } = default!;
}
