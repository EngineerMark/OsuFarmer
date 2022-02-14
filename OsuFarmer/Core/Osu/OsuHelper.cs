using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuFarmer.Managers;
using OsuFarmer.Helpers;

namespace OsuFarmer.Core.Osu
{
    public class OsuHelper
    {
        public static async Task<User?> GetUser(string? id, int mode, string id_type = "string")
        {
            if (id == null)
                return null;

            List<User?>? users = await ApiHelper.GetDataDeserialized<List<User?>?>("https://osu.ppy.sh/api/get_user?k=" + SettingsManager.Instance.Settings.ApiKey + "&u=" + id + "&m=" + mode + "&type=" + id_type);
            if (users == null || users.Count == 0)
            {
                return null;
            }
            User? user = users[0];
            return user;
        }

        public static async Task<bool> IsApiValid(){
            if (string.IsNullOrEmpty(SettingsManager.Instance.Settings.ApiKey))
                return false;

            return await IsUserValid("peppy");
        }

        public static async Task<bool> IsUserValid(string? name){
            return (await GetUser(name, 0)) != null;
        }
    }
}
