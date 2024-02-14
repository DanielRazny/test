
namespace UpdateService.Services.Database
{
    public class DatabaseHandler : IDatabaseHandler
    {
        public Task<Stream?> DownloadVersion(string applicationName, string version)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<string>> GetApplicationNames()
        {
            throw new NotImplementedException();
        }

        public string? GetCurrentVersion(string applicationName)
        {
            throw new NotImplementedException();
        }

        Task<string?> IBaseApplicationVersionHandler.GetCurrentVersion(string applicationName)
        {
            throw new NotImplementedException();
        }
    }
}
