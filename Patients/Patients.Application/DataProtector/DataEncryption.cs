namespace Patients.Application.DataProtector;

public class DataEncryption
{
    public string key { get; init; }

	public DataEncryption(string encryptionKey)
	{
		key = encryptionKey;
    }
}
