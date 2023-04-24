namespace Patients.Contracts.Responses;

public class PatientResponse
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string HealthNumber { get; init; }
    public required DateTime Birthdate { get; init; }
    public required string Address { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }

}
