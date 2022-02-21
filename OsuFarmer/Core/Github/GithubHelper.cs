using OsuFarmer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Core.Github
{
    public static class GithubHelper
    {
        private const string API_REPO_URL = "https://api.github.com/repos/EngineerMark/OsuFarmer/";

        public static async Task<Release?> GetLatestRelease()
        {
            string url = API_REPO_URL + "releases/latest";
            Release? release = await ApiHelper.GetDataDeserialized<Release>(url);
            return release;
        }
    }
}
