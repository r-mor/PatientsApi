using FluentValidation;
using Patients.Application.Models;
using Patients.Application.Repository;

namespace Patients.Application.Validators;

public class PatientValidator : AbstractValidator<Patient>
{
    private readonly IPatientRepository _patientRepository;
	public PatientValidator(IPatientRepository patientRepository)
	{
        _patientRepository = patientRepository;

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.HealthNumber)
            .NotEmpty();

        RuleFor(x => x.HealthNumber)
            .MustAsync(ValidateHealthNumber)
            .WithMessage("A patient with the same health number already exists in the system");

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.Birthdate)
            .LessThanOrEqualTo(DateTime.UtcNow);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\+?[6-9]\d{9}$")
            .WithMessage("Phone number is not in a valid format");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email address is not in a valid format");
            

    }

    private async Task<bool> ValidateHealthNumber(Patient patient, string healthNumber, CancellationToken arg2)
    {
        var existingPatient = await _patientRepository.GetByHealthNumberAsync(healthNumber);

        if (existingPatient is not null)
        {
            return existingPatient.Id == patient.Id;
        }

        return existingPatient is null;
    }
}
