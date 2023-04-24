using Patients.Application.Models;
using Patients.Contracts.Requests;
using Patients.Contracts.Responses;

namespace Patients.Api.Mappers;

public static class PatientMapper
{
    public static Patient ToPatient(this CreatePatientRequest request)
    {
        return new Patient
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            HealthNumber = request.HealthNumber,
            Birthdate = request.Birthdate,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email
        };
    }

    public static Patient ToPatient(this UpdatePatientRequest request, Guid id)
    {
        return new Patient
        {
            Id = id,
            FirstName = request.FirstName,
            LastName = request.LastName,
            HealthNumber = request.HealthNumber,
            Birthdate = request.Birthdate,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email
        };
    }

    public static PatientResponse ToResponse(this Patient patient)
    {
        return new PatientResponse
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            HealthNumber = patient.HealthNumber,
            Birthdate = patient.Birthdate,
            Address = patient.Address,
            Phone = patient.Phone,
            Email = patient.Email
        };
    }

    public static PatientsResponse ToResponse(this IEnumerable<Patient> patients)
    {
        return new PatientsResponse
        {
            Items = patients.Select(ToResponse)
        };
    }
}
