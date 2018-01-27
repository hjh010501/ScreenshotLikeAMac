using System;
using System.Collections.Generic;
using System.IO;
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

        public static SelectMouseCursor SelectMouseCursor;
        public static ScreenshotLikeAMac.UI.Setting Setting;

        Capture cap = new Capture();

        public HotkeyChecker()
        {
            InitializeComponent();
            GlobalKeyHook.Hook.KeyboardDown += Hook_KeyboardDown;
            if(Properties.Settings.Default.savepath == "")
            {
                string defaultpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\screenshot";
                DirectoryInfo screenshotfolder = new DirectoryInfo(defaultpath);
                if(screenshotfolder.Exists == false)
                {
                    screenshotfolder.Create();
                }
                Properties.Settings.Default.savepath = defaultpath;
                Properties.Settings.Default.Save();
            }
        }

        void Hook_KeyboardDown(object sender, GlobalKeyHookEventArgs e)
        {

            //System.Windows.MessageBox.Show(e.VKeyCode.ToString());

            if (e.VKeyCode == VKeyCode.Three)
            {
                if (GlobalKeyHook.IsKeyDown(VKeyCode.LeftAlt) && GlobalKeyHook.IsKeyDown(VKeyCode.LeftShift) && GlobalKeyHook.Hook.Pressing == false)
                {
                    cap.CaptureMyScreen();
                    GlobalKeyHook.Hook.Pressing = true;
                }
            }

            if (e.VKeyCode == VKeyCode.Four)
            {
                if (GlobalKeyHook.IsKeyDown(VKeyCode.LeftAlt) && GlobalKeyHook.IsKeyDown(VKeyCode.LeftShift) && GlobalKeyHook.Hook.Pressing == false)
                {
                    if(SelectMouseCursor == null)
                    {
                        SelectMouseCursor = new SelectMouseCursor();
                        SelectMouseCursor.Owner = App.Current.MainWindow;
                        SelectMouseCursor.Closed += delegate
                        {
                            SelectMouseCursor = null;
                        };
                        SelectMouseCursor.Show();

                    } else
                    {
                        SelectMouseCursor.Activate();
                    }
                    GlobalKeyHook.Hook.Pressing = true;

                }
            }

            if (e.VKeyCode == VKeyCode.Six)
            {
                if (GlobalKeyHook.IsKeyDown(VKeyCode.LeftAlt) && GlobalKeyHook.IsKeyDown(VKeyCode.LeftShift) && GlobalKeyHook.Hook.Pressing == false)
                {
                    if (Setting == null)
                    {
                        Setting = new ScreenshotLikeAMac.UI.Setting();
                        Setting.Owner = App.Current.MainWindow;
                        Setting.Closed += delegate
                        {
                            Setting = null;
                        };
                        Setting.Show();

                    }
                    else
                    {
                        Setting.Activate();
                    }
                    GlobalKeyHook.Hook.Pressing = true;
                }
            }

            if (e.VKeyCode == VKeyCode.ESC)
            {
                if(SelectMouseCursor != null)
                {
                    MouseHook.stop();
                    SelectMouseCursor.Close();
                    SelectMouseCursor = null;
                }
            }
            
        }
    }
}
