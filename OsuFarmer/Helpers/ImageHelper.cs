using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Avalonia;
using Avalonia.Platform;

namespace OsuFarmer.Helpers
{
    //For clarity, do not include one of the bitmap types' namespaces
    public static class ImageHelper
    {
        public static Avalonia.Media.Imaging.Bitmap? FromSysBitmap(System.Drawing.Bitmap? bmp)
        {
            if(bmp == null)
                return null;

            Avalonia.Media.Imaging.Bitmap? newBmp = null;
            using (MemoryStream memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                //AvIrBitmap is our new Avalonia compatible image. You can pass this to your view
                newBmp = new Avalonia.Media.Imaging.Bitmap(memory);
            }
            return newBmp;
        }

        public static System.Drawing.Bitmap? GetBitmapFromWeb(string? url)
        {
            if(url == null)
                return default;

            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(responseStream);
            return bitmap;
        }

        public static Avalonia.Media.Imaging.Bitmap? GetAvaloniaBitmapFromWeb(string? url)
        {
            return FromSysBitmap(GetBitmapFromWeb(url));
        }

        public static Avalonia.Media.Imaging.Bitmap? GetAvaloniaBitmapFromAssets(string? url)
        {
            if (url == null)
                return null;

            IAssetLoader? assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            Avalonia.Media.Imaging.Bitmap? bitmap = new Avalonia.Media.Imaging.Bitmap(assets.Open(new Uri(url)));
            return bitmap;
        }
    }
}
