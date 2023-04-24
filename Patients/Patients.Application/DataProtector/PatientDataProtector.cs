using Microsoft.AspNetCore.DataProtection;
using Patients.Application.Models;

namespace Patients.Application.DataProtector;

public class PatientDataProtector : IPatientDataProtector
{
    private readonly string _encryptionKey;
    private readonly IDataProtector _dataProtector;

    public PatientDataProtector(DataEncryption encryption, IDataProtectionProvider protectionProvider)
    {
        _encryptionKey = encryption.key;
        _dataProtector= protectionProvider.CreateProtector(_encryptionKey);
    }

    public Patient PatientDecrypt(PatientEncrypted patientEncrypted)
    {
        return new Patient
        {
            Id = patientEncrypted.Id,
            FirstName = _dataProtector.Unprotect(patientEncrypted.FirstName),
            LastName = _dataProtector.Unprotect(patientEncrypted.LastName),
            HealthNumber = _dataProtector.Unprotect(patientEncrypted.HealthNumber),
            Birthdate = DateTime.Parse(_dataProtector.Unprotect(patientEncrypted.Birthdate)),
            Address = _dataProtector.Unprotect(patientEncrypted.Address),
            Email = _dataProtector.Unprotect(patientEncrypted.Email),
            Phone = _dataProtector.Unprotect(patientEncrypted.Phone),
        };
    }

    public PatientEncrypted PatientEncrypt(Patient patient)
    {
        return new PatientEncrypted
        {
            Id = patient.Id,
            FirstName = _dataProtector.Protect(patient.FirstName),
            LastName = _dataProtector.Protect(patient.LastName),
            HealthNumber = _dataProtector.Protect(patient.HealthNumber),
            Birthdate = _dataProtector.Protect(patient.Birthdate.ToLongDateString()),
            Address = _dataProtector.Protect(patient.Address),
            Email = _dataProtector.Protect(patient.Email),
            Phone = _dataProtector.Protect(patient.Phone),
        };
    }

    public string DataDecrypt(string data)
    {
        return _dataProtector.Unprotect(data);
    }

    public string DataEncrypt(string data)
    {
        return _dataProtector.Protect(data);
    }
}
