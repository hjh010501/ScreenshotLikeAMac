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

                Bitmap captureBitmap;
                Graphics memoryGraphics;

                foreach (Screen screen in Screen.AllScreens)
                {
                    captureBitmap = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb);
                    memoryGraphics = Graphics.FromImage(captureBitmap);
                    memoryGraphics.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    captureBitmap.Save("Screenshot " + DateTime.Now.ToString("yyyy-MM-dd tt H.mm.ss") + ((i==1) ? "" : " (" + i + ")") + ".png", ImageFormat.Png);
                    captureBitmap.Dispose();
                    memoryGraphics.Dispose();
                    i++;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void CaptureArea(double startx, double starty, double endx, double endy)
        {
            try
            {
                Bitmap captureBitmap = new Bitmap((int)Math.Abs(endx - startx), (int)Math.Abs(endy - starty), PixelFormat.Format32bppArgb);
                Graphics memoryGraphics = Graphics.FromImage(captureBitmap);
                memoryGraphics.CopyFromScreen((int)((startx > endx) ? endx : startx), (int)((starty > endy) ? endy : starty), 0, 0, new Size((int)Math.Abs(endx - startx), (int)Math.Abs(endy - starty)), CopyPixelOperation.SourceCopy);
                captureBitmap.Save("Screenshot " + DateTime.Now.ToString("yyyy-MM-dd tt H.mm.ss") + ".png", ImageFormat.Png);
                captureBitmap.Dispose();
                memoryGraphics.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
