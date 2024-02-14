using Bootloader.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using UpdateService.Services.VersionChecker;

namespace Bootloader
{
    internal class BootLoader
    {
        private readonly IOptions<ApplicationOptions> _options;
        private readonly HttpClient _httpClient;
        private readonly IVersionCompareHandler _versionCompareHandler;

        public BootLoader(IOptions<ApplicationOptions> options, HttpClient httpClient, IVersionCompareHandler versionCompareHandler)
        {
            _options = options;
            _httpClient = httpClient;
            _versionCompareHandler = versionCompareHandler;
        }

        public async Task RunAsync()
        {
            var currentVersion = GetVersionNumber(_options.Value.ApplicationPath!);
            var applicationName = GetApplicationName(_options.Value.ApplicationPath!);
            var serverVersion = await GetServerApplicationVersion(applicationName!);

            if (_versionCompareHandler.CompareVersions(serverVersion.Version, currentVersion) == 1)
            {

            }

        }

        public string? GetVersionNumber(string path)
        {
            var info = FileVersionInfo.GetVersionInfo(path);
            return info.ProductVersion;
        }

        public string? GetApplicationName(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public async Task<VersionModel?> GetServerApplicationVersion(string applicationName)
        {
            _httpClient.BaseAddress = new Uri(_options.Value.UpdateServerUrl);
            using var result = await _httpClient
                .GetAsync($"Version?applicationName={applicationName}") // ToDo escape
                .ConfigureAwait(false);

            var content = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<VersionModel>(content);
        }
    }

    public class VersionModel
    {        
        public required string ApplicationName { get; set; }

        public required string Version { get; set; }
    }
}
