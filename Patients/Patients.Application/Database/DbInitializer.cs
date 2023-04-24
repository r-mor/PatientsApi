using Dapper;

namespace Patients.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
	public DbInitializer(IDbConnectionFactory dbConnectionFactory)
	{
		_dbConnectionFactory= dbConnectionFactory;
	}

	public async Task InitializeDbAsync()
	{
		using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patients]') AND type in (N'U'))
        BEGIN
            CREATE TABLE Patients (
                Id UNIQUEIDENTIFIER NOT NULL,
                FirstName NVARCHAR(200) NOT NULL,
                LastName NVARCHAR(200) NOT NULL,
                HealthNumber NVARCHAR(200) NOT NULL,
                Birthdate NVARCHAR(200) NOT NULL,
                Address NVARCHAR(200) NOT NULL,
                Phone NVARCHAR(200) NOT NULL,
                Email NVARCHAR(200) NOT NULL,
                CONSTRAINT PK_Patients PRIMARY KEY (Id)
            );
        END

        """);
    }
}
