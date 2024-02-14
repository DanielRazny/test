using System.Collections.Concurrent;

namespace UpdateService.Services.Cache
{
    public class ApplicationVersionCache : IApplicationVersionCache
    {
        private ConcurrentDictionary<string, string> _applicationVersion;

        public ApplicationVersionCache()
        {
            _applicationVersion = new ConcurrentDictionary<string, string>();
        }

        public Task<string?> GetCurrentVersion(string applicationName)
            => Task.FromResult(_applicationVersion.GetValueOrDefault(applicationName));

        public string SetApplicationVersion(string applicationName, string version)
            => _applicationVersion[applicationName] = version;

        // ToDo application
        // public IReadOnlyCollection<string> GetAllApplications()
        // public RemoveApplication();
    }
}
