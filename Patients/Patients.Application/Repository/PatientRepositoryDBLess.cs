using Patients.Application.Models;

namespace Patients.Application.Repository;

public class PatientRepositoryDBLess : IPatientRepository
{
    private readonly List<Patient> _patients = new();
    public Task<bool> CreateAsync(Patient patient, CancellationToken token = default)
    {
        _patients.Add(patient);
        return Task.FromResult(true);
    }
    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        int removedCount = _patients.RemoveAll(x => x.Id == id);
        bool patientRemoved = removedCount > 0;
        return Task.FromResult(patientRemoved);
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Patient>> GetAllAsync(CancellationToken token = default)
    {
        return Task.FromResult(_patients.AsEnumerable());
    }

    public Task<Patient?> GetByHealthNumberAsync(string HealthNumber, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<Patient?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        Patient? patient = _patients.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(patient);
    }

    public Task<bool> UpdateAsync(Patient patient, CancellationToken token = default)
    {
        int patientIndex = _patients.FindIndex(x => x.Id == patient.Id);
        if (patientIndex == -1)
        {
            return Task.FromResult(false);
        }

        _patients[patientIndex] = patient;
        return Task.FromResult(true);
    }
}
