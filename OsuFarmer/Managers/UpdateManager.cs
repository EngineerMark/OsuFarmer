using OsuFarmer.Core.Github;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Managers
{
    public class UpdateManager : Manager<UpdateManager>
    {
        Release? LatestRelease { get; set; }
        Version? CurrentVersion { get; set; }

        public UpdateManager()
        {
            Register(this);
        }

        public async Task Start()
        {
            CurrentVersion = GetAppVersion();
            await CheckForUpdates();
        }

        private async Task CheckForUpdates()
        {
            Release? latest_release = await GithubHelper.GetLatestRelease();
            LatestRelease = latest_release;

            if (LatestRelease != null && LatestRelease.Version > CurrentVersion)
            {
                UIManager.Instance.ApplyUpdate(LatestRelease);
                await UIManager.Instance.DisplayAlertAsync("Update available ("+CurrentVersion.ToString()+" > "+LatestRelease.Version.ToString()+")", "A new version is available on GitHub. Go to the Info tab to check it out", new string[] { "Ok" });
            }
        }

        public Version GetAppVersion()
        {
            string version = GetType().Assembly.GetName().Version.ToString();
            Version v = new Version(version);
            //v = new Version("0.9.0");
            return v;
        }
    }
}
