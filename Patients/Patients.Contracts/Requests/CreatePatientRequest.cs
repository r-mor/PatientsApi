namespace Patients.Contracts.Requests;

public class CreatePatientRequest
{
    public required string FirstName {get; init;}
    public required string LastName { get; init; }
    public required string HealthNumber { get; init; }
    public required DateTime Birthdate { get; init; }
    public required string Address { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
}
