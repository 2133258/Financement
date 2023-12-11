using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

public class AutoUpdateService
{
    private readonly string _githubApiReleaseUrl = "https://api.github.com/repos/{username}/{repo}/releases/latest";

    public async Task<ReleaseInfo> CheckForUpdatesAsync()
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "request"); // GitHub API requires a User-Agent header
            var response = await client.GetStringAsync(_githubApiReleaseUrl);
            var releaseInfo = JsonConvert.DeserializeObject<ReleaseInfo>(response);

            if (IsNewerVersionAvailable(releaseInfo.TagName))
            {
                return releaseInfo;
            }

            return null;
        }
    }

    private bool IsNewerVersionAvailable(string latestVersion)
    {
        var currentVersion = GetCurrentVersion();
        return Version.Parse(latestVersion) > Version.Parse(currentVersion);
    }

    private string GetCurrentVersion()
    {
        // Assuming the version is stored in Assembly Information
        return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }


    public async void DownloadAndUpdate(ReleaseInfo releaseInfo)
    {
        var client = new WebClient();

        // Assuming the installer URL is in the releaseInfo object
        string downloadUrl = releaseInfo.InstallerUrl;
        string localPath = Path.Combine(Path.GetTempPath(), "update_installer.exe");

        try
        {
            await client.DownloadFileTaskAsync(new Uri(downloadUrl), localPath);

            // Execute the installer
            Process.Start(localPath);

            // Optionally, close the current application
            Application.Current.Shutdown();
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log the error or notify the user)
        }
    }

}

public class ReleaseInfo
{
    [JsonProperty("tag_name")]
    public string TagName { get; set; }

    [JsonProperty("assets")]
    public List<GitHubAsset> Assets { get; set; }

    // Assuming the installer is the first asset. Adjust as needed.
    public string InstallerUrl => Assets.FirstOrDefault()?.BrowserDownloadUrl;
}

public class GitHubAsset
{
    [JsonProperty("browser_download_url")]
    public string BrowserDownloadUrl { get; set; }

    // Add more properties as needed
}

