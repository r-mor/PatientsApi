using Dapper;
using Patients.Application.Database;
using Patients.Application.DataProtector;
using Patients.Application.Models;
using System.Data.Common;

namespace Patients.Application.Repository;

public class PatientRepositorySqlServerExpress : IPatientRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IPatientDataProtector _patientDataProtector;

    public PatientRepositorySqlServerExpress(IDbConnectionFactory dbConnectionFactory, IPatientDataProtector patientDataProtector)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _patientDataProtector = patientDataProtector;
    }

    public async Task<bool> CreateAsync(Patient patient, CancellationToken token = default)
    {
        var patientEncrypted = _patientDataProtector.PatientEncrypt(patient);
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            INSERT INTO Patients (Id, FirstName, LastName, HealthNumber, Birthdate, Address, Phone, Email)
            VALUES (@Id, @FirstName, @LastName, @HealthNumber, @Birthdate, @Address, @Phone, @Email);
            """, patientEncrypted, transaction, cancellationToken: token));

        transaction.Commit();

        return (result > 0);
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            DELETE FROM Patients WHERE Id = @id
            """, new { id }, transaction, cancellationToken: token));

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
            SELECT COUNT(1) FROM Patients WHERE Id = @id
            """, new { id }, cancellationToken: token));
    }

    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await connection.QueryAsync(new CommandDefinition("""
            SELECT * FROM Patients
            """, cancellationToken: token));

        return ToPatients(result);
    }

    private IEnumerable<Patient> ToPatients(IEnumerable<dynamic> result)
    {
        List<Patient> patients = new();
        foreach (dynamic item in result)
        {
            patients.Add(
                _patientDataProtector.PatientDecrypt(
                new()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    HealthNumber = item.HealthNumber,
                    Birthdate = item.Birthdate,
                    Address = item.Address,
                    Phone = item.Phone,
                    Email = item.Email
                }));
        }
        return patients;
    }

    public async Task<Patient?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        var patientEncrypted = await connection.QuerySingleOrDefaultAsync<PatientEncrypted>(
            new CommandDefinition("""
                SELECT TOP 1 *
                FROM Patients
                WHERE Id = @id
                """, new { id }, cancellationToken: token));

        if (patientEncrypted is null)
        {
            return null;
        }

        return _patientDataProtector.PatientDecrypt(patientEncrypted);
    }

    public async Task<bool> UpdateAsync(Patient patient, CancellationToken token = default)
    {
        var patientEncrypted = _patientDataProtector.PatientEncrypt(patient);
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
            UPDATE Patients
            SET FirstName = @FirstName,
                LastName = @LastName,
                HealthNumber = @HealthNumber, 
                Birthdate = @Birthdate, 
                Address = @Address, 
                Phone = @Phone, 
                Email = @Email

            WHERE id= @Id
            """, patientEncrypted, transaction, cancellationToken: token));
        transaction.Commit();

        return result > 0;
    }

    public async Task<Patient?> GetByHealthNumberAsync(string HealthNumber, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        string HealtNumberEncrypted = _patientDataProtector.DataEncrypt(HealthNumber);

        var patientEncrypted = await connection.QuerySingleOrDefaultAsync<PatientEncrypted>(
            new CommandDefinition("""
                SELECT TOP 1 *
                FROM Patients
                WHERE HealthNumber = @HealthNumber
                """, new { HealthNumber = HealtNumberEncrypted }, cancellationToken: token));

        if (patientEncrypted is null)
        {
            return null;
        }

        return _patientDataProtector.PatientDecrypt(patientEncrypted);
    }
}
