
using FluentValidation;
using Patients.Application.Models;
using Patients.Application.Repository;

namespace Patients.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IValidator<Patient> _patientValidator;

    public PatientService(IPatientRepository patientRepository, IValidator<Patient> patientValidator)
    {
        _patientRepository = patientRepository;
        _patientValidator = patientValidator;
    }
    
    public async Task<bool> CreateAsync(Patient patient, CancellationToken token = default)
    {
        await _patientValidator.ValidateAndThrowAsync(patient, cancellationToken: token);
        return await _patientRepository.CreateAsync(patient, token);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _patientRepository.DeleteByIdAsync(id, token);
    }

    public Task<IEnumerable<Patient>> GetAllAsync(CancellationToken token = default)
    {
        return _patientRepository.GetAllAsync(token);
    }

    public Task<Patient?> GetByHealthNumberAsync(string healthNumber, CancellationToken token = default)
    {
        return _patientRepository.GetByHealthNumberAsync(healthNumber, token);
    }

    public Task<Patient?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _patientRepository.GetByIdAsync(id, token);
    }

    public async Task<Patient?> UpdateAsync(Patient patient, CancellationToken token = default)
    {
        await _patientValidator.ValidateAndThrowAsync(patient, cancellationToken: token);
        bool patientExists = await _patientRepository.ExistsByIdAsync(patient.Id, token);
        if(!patientExists)
        {
            return null;
        }
        await _patientRepository.UpdateAsync(patient, token);

        return patient;
    }
}
