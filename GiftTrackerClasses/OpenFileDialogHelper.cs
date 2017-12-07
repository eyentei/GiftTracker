using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GiftTrackerClasses
{
    public static class OpenFileDialogHelper
    {
        public static OpenFileDialog OpenImageFileDialog()
        {
            // taken from https://stackoverflow.com/a/37492934

            OpenFileDialog dialog = new OpenFileDialog();

            var codecs = ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dialog.Filter = codecFilter;
            return dialog;
        }
    }
}
