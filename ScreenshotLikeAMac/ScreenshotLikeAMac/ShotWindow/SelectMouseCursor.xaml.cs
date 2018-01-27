using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

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

            public static implicit operator System.Drawing.Point(POINT point)
            {
                return new System.Drawing.Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static System.Drawing.Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        static void draw(System.Drawing.Rectangle rect, System.Drawing.Brush color, IntPtr hwnd)
        {
            using (Graphics g = Graphics.FromHdc(hwnd))
            {
                g.FillRectangle(color, rect);
            }
        }

        Matrix m;
        double dx;
        double dy;

        POINT STARTPOINT;
        POINT ENDPOINT;

        bool capturing = false;

        Capture cap = new Capture();

        TranslateTransform CursorTranslate = new TranslateTransform();

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

            CursorTranslate.X = GetCursorPosition().X / dx - 13;
            CursorTranslate.Y = GetCursorPosition().Y / dy - 13;

            Cursor.RenderTransform = CursorTranslate;

            XTextblock.Text = (GetCursorPosition().X / dx).ToString();
            YTextblock.Text = (GetCursorPosition().Y / dy).ToString();
        }

        private void MouseHook_MouseAction(object sender, EventArgs e)
        {

            CursorTranslate.X = GetCursorPosition().X / dx - 13;
            CursorTranslate.Y = GetCursorPosition().Y / dy - 13;

            Cursor.RenderTransform = CursorTranslate;

            XTextblock.Text = (capturing == false) ? (GetCursorPosition().X / dx).ToString() : (GetCursorPosition().X / dx).ToString() + " (" + Math.Abs(STARTPOINT.X - (GetCursorPosition().X / dx)) + ")" ;
            YTextblock.Text = (capturing == false) ? (GetCursorPosition().Y / dy).ToString() : (GetCursorPosition().Y / dy).ToString() + " (" + Math.Abs(STARTPOINT.Y - (GetCursorPosition().Y / dy)) + ")";

            if(capturing == true)
            {
                CursorArea.RenderTransform = new TranslateTransform((STARTPOINT.X > (GetCursorPosition().X / dx)) ? (GetCursorPosition().X / dx) : STARTPOINT.X, (STARTPOINT.Y > (GetCursorPosition().Y / dy)) ? (GetCursorPosition().Y / dy) : STARTPOINT.Y);
                CursorArea.Width = Math.Abs(STARTPOINT.X - (GetCursorPosition().X / dx));
                CursorArea.Height = Math.Abs(STARTPOINT.Y - (GetCursorPosition().Y / dy));
            }
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
