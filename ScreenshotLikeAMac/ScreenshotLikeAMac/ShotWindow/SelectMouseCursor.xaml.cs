using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenshotLikeAMac.ShotWindow
{
    /// <summary>
    /// SelectMouseCursor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SelectMouseCursor : Window
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }

        Matrix m;
        double dx;
        double dy;

        POINT STARTPOINT;
        POINT ENDPOINT;

        bool capturing = false;

        Capture cap = new Capture();

        public SelectMouseCursor()
        {
            InitializeComponent();
            m = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;
            dx = m.M11;
            dy = m.M22;
            MouseHook.MouseMoveAction += MouseHook_MouseAction;
            MouseHook.MouseLeftDownAction += MouseHook_MouseLeftDownAction;
            MouseHook.MouseLeftUpAction += MouseHook_MouseLeftUpAction;
            MouseHook.Start();
            this.Left = GetCursorPosition().X / dx - 13;
            this.Top = GetCursorPosition().Y / dy - 13;
            XTextblock.Text = (GetCursorPosition().X / dx).ToString();
            YTextblock.Text = (GetCursorPosition().Y / dy).ToString();
        }

        private void MouseHook_MouseAction(object sender, EventArgs e)
        {

            this.Left = GetCursorPosition().X / dx - 13;
            this.Top = GetCursorPosition().Y / dy - 13;

            XTextblock.Text = (capturing == false) ? (GetCursorPosition().X / dx).ToString() : (GetCursorPosition().X / dx).ToString() + " (" + Math.Abs(STARTPOINT.X - (GetCursorPosition().X / dx)) + ")" ;
            YTextblock.Text = (capturing == false) ? (GetCursorPosition().Y / dy).ToString() : (GetCursorPosition().Y / dy).ToString() + " (" + Math.Abs(STARTPOINT.Y - (GetCursorPosition().Y / dx)) + ")";

        }

        private void MouseHook_MouseLeftDownAction(object sender, EventArgs e)
        {
            STARTPOINT.X = (int)(GetCursorPosition().X / dx);
            STARTPOINT.Y = (int)(GetCursorPosition().Y / dy);
            capturing = true;
        }

        private void MouseHook_MouseLeftUpAction(object sender, EventArgs e)
        {
            ENDPOINT.X = (int)(GetCursorPosition().X / dx);
            ENDPOINT.Y = (int)(GetCursorPosition().Y / dy);
            capturing = false;
            MouseHook.stop();
            this.Hide();

            cap.CaptureArea(STARTPOINT.X * dx, STARTPOINT.Y * dy, ENDPOINT.X * dx, ENDPOINT.Y * dy);
            this.Close();
        }
    
    }
}
