using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenshotLikeAMac.ShotWindow
{
    class Capture
    {

        public void CaptureMyScreen()
        {
            try
            {

                int i = 1;

                foreach(Screen screen in Screen.AllScreens)
                {
                    Bitmap captureBitmap = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb);
                    Graphics memoryGraphics = Graphics.FromImage(captureBitmap);
                    memoryGraphics.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    captureBitmap.Save("Screenshot " + DateTime.Now.ToString("yyyy-MM-dd tt H.mm.ss") + ((i==1) ? "" : " (" + i + ")") + ".png", ImageFormat.Png);
                    i++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
