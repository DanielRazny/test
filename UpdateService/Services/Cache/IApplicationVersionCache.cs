namespace UpdateService.Services.Cache
{
    // I would not split that into read and write
    // too much interfaces could create messy code
    public interface IApplicationVersionCache
    {
        Task<string?> GetCurrentVersion(string applicationName);

        string SetApplicationVersion(string applicationName, string version);
    }
}