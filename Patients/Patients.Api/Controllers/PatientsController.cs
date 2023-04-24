using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Patients.Api.Auth;
using Patients.Api.Mappers;
using Patients.Application.Models;
using Patients.Application.Repository;
using Patients.Application.Services;
using Patients.Contracts.Requests;
using Patients.Contracts.Responses;

namespace Patients.Api.Controllers;

[ApiController]
public class PatientsController : ControllerBase
{
	private readonly IPatientRepository _patientRepository;
    private readonly IPatientService _patientService;
	public PatientsController(IPatientRepository patientRepository, IPatientService patientService)
	{
		_patientRepository= patientRepository;
        _patientService= patientService;
	}

    [Authorize(AuthConstants.HealthcareProviderPolicyName)]
    [HttpPost(ApiEndpoints.Patients.Create)]
	public async Task<IActionResult> Create([FromBody] CreatePatientRequest request, CancellationToken token)
	{
		Patient patient = request.ToPatient();

        await _patientService.CreateAsync(patient, token);

        return CreatedAtAction(nameof(Get), new { idOrHealthNumber = patient.Id }, patient);
    }

    [Authorize(AuthConstants.HealthcareProviderPolicyName)]
    [HttpGet(ApiEndpoints.Patients.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrHealthNumber, CancellationToken token)
    {
        Patient? patient = Guid.TryParse(idOrHealthNumber, out var id) ?
            await _patientService.GetByIdAsync(id, token) :
            await _patientService.GetByHealthNumberAsync(idOrHealthNumber, token);

        if (patient is null)
        {
            return NotFound();
        }

        PatientResponse response = patient.ToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.HealthcareProviderPolicyName)]
    [HttpGet(ApiEndpoints.Patients.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        IEnumerable<Patient> patients = await _patientService.GetAllAsync(token);

        PatientsResponse response = patients.ToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.HealthcareProviderPolicyName)]
    [HttpPut(ApiEndpoints.Patients.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePatientRequest request, CancellationToken token)
    {
        Patient patient = request.ToPatient(id);
        Patient? updatedPatient = await _patientService.UpdateAsync(patient, token);
        if (updatedPatient is null)
        {
            return NotFound();
        }

        PatientResponse response = patient.ToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Patients.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        bool deleted = await _patientService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}
