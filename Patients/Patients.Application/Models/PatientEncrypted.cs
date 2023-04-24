
namespace Patients.Application.Models;

public class PatientEncrypted
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string HealthNumber { get; init; }
    public required string Birthdate { get; init; }
    public required string Address { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
}
