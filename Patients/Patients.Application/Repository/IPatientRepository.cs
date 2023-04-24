using Patients.Application.Models;

namespace Patients.Application.Repository;

public interface IPatientRepository
{
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    Task<bool> CreateAsync(Patient patient, CancellationToken token = default);
    Task<Patient?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<Patient?> GetByHealthNumberAsync(string HealthNumber, CancellationToken token = default);
    Task<IEnumerable<Patient>> GetAllAsync(CancellationToken token = default);
    Task<bool> UpdateAsync(Patient patient, CancellationToken token = default);
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

}
