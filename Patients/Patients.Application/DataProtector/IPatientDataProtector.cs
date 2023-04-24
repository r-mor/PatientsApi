using Patients.Application.Models;

namespace Patients.Application.DataProtector;

public interface IPatientDataProtector
{
    Patient PatientDecrypt(PatientEncrypted patientEncrypted);
    PatientEncrypted PatientEncrypt(Patient patient);
    public string DataDecrypt(string data);
    public string DataEncrypt(string data);
}
