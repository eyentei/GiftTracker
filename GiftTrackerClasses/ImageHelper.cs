using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GiftTrackerClasses
{
    public static class ImageHelper
    {
        public static byte[] BitmapSourceToByteArray(string image)
        {
            BitmapSource bSource = new BitmapImage(new Uri(image, UriKind.Relative));
            using (var stream = new MemoryStream())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bSource));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
