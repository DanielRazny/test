
namespace UpdateService.Services
{
    public class HostedService: IHostedService
    {
        private readonly IApplicationVersionHandler _applicationVersionHandler;

        public HostedService(IApplicationVersionHandler applicationVersionHandler) {
            _applicationVersionHandler = applicationVersionHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // ToDo cancellationToken
            return _applicationVersionHandler.CheckApplicationVersions();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;         
        }
    }
}
