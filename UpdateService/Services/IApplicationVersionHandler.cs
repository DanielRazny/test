namespace UpdateService.Services
{
    public interface IApplicationVersionHandler : IBaseApplicationVersionHandler
    {
        Task CheckApplicationVersions();
    }

}
