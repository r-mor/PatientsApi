namespace Patients.Api.Controllers;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Patients
    {
        private const string Base = $"{ApiBase}/patients";

        public const string Create = Base;
        public const string Get = $"{Base}/{{idOrHealthNumber}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
}
