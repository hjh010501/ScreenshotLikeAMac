using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace ScreenshotLikeAMac.ShotWindow
{
    /// <summary>
    /// HotkeyChecker.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HotkeyChecker : Window
    {

        Capture cap = new Capture();

        public HotkeyChecker()
        {
            InitializeComponent();
            GlobalKeyHook.Hook.KeyboardPressed += Hook_KeyboardPressed;
        }

        void Hook_KeyboardPressed(object sender, GlobalKeyHookEventArgs e)
        {

            //System.Windows.MessageBox.Show(e.VKeyCode.ToString());

            if (e.VKeyCode == VKeyCode.Three)
            {
                if (GlobalKeyHook.IsKeyPressed(VKeyCode.LeftAlt) && GlobalKeyHook.IsKeyPressed(VKeyCode.LeftShift))
                {
                    cap.CaptureMyScreen();
                    

                }
            }
            System.Threading.Thread.Sleep(100);
        }
    }
}
