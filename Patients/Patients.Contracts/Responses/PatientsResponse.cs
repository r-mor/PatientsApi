namespace Patients.Contracts.Responses;

public class PatientsResponse
{
    public IEnumerable<PatientResponse> Items { get; init; } = Enumerable.Empty<PatientResponse>();
}

