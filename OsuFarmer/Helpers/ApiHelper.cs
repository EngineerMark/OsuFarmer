using Avalonia.Media.Imaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Helpers
{
    public static class ApiHelper
    {
        public static async Task<Bitmap> BitmapFromUrl(string url)
        {
            HttpClient client = new HttpClient();
            System.IO.Stream responseStream = await client.GetStreamAsync(url);
            return new Bitmap(url);
        }

        public static async Task<T?> GetDataDeserialized<T>(string? url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string? data = await GetData(url);
                if (!string.IsNullOrEmpty(data))
                {
                    T? output = default;
                    try
                    {
                        output = JsonConvert.DeserializeObject<T>(data);
                    }
                    catch (Exception) { }
                    return output;
                }
                return default;
            }
            return default;
        }

        public static async Task<string?> GetData(string url, int timeout = 20000)
        {
            using HttpClient? client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeout);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
            string s = "";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                s = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                s = string.Empty;
            }
            return s;
        }
    }
}
