namespace UpdateService.Services.VersionChecker
{
    public interface IVersionCompareHandler
    {
        int CompareVersions(string? version1, string? version2);
    }
}
