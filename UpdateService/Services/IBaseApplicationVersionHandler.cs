namespace UpdateService.Services
{
    public interface IBaseApplicationVersionHandler
    {
        Task<string?> GetCurrentVersion(string applicationName);

        Task<Stream?> DownloadVersion(string applicationName, string version);

        Task<IReadOnlyCollection<string>> GetApplicationNames();
    }

}
