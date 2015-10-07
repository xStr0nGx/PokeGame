using System;
using System.Drawing;
using System.Runtime.InteropServices;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace PokeGame
{
    internal class Application
    {
        private const int MonitorWidth = 1920;
        private const int MonitorHeight = 1080;

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr after, int x, int y, int width, int height, int flags);

        public static Surface WindowSurface { get; private set; }

        private static Surface Rectangle { get; set; }
        private static Point RectangleLocation { get; set; }

        public static void Main()
        {
            WindowSurface = Video.SetVideoMode(1200, 700, false, false, false);
            Rectangle = new Surface(200, 100);
            RectangleLocation = new Point(500, 270);

            var newX = MonitorWidth / 2 - Video.Screen.Width / 2;
            var newY = MonitorHeight / 2 - Video.Screen.Height / 2;
            SetWindowPos(Video.WindowHandle, IntPtr.Zero, newX, newY, Video.Screen.Width, Video.Screen.Height, 0x004 | 0x0010);

            Video.WindowCaption = "Pokemon";

            Events.Quit += (sender, args) => Environment.Exit(0);
            Events.TargetFps = 150;
            Events.Tick += OnTick;
            Events.KeyboardDown += OnKeyDown;
            Events.Run();
        }

        private static void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                case Key.UpArrow:
                    RectangleLocation = new Point(RectangleLocation.X, RectangleLocation.Y - 25);
                    break;

                case Key.S:
                case Key.DownArrow:
                    RectangleLocation = new Point(RectangleLocation.X, RectangleLocation.Y + 25);
                    break;
                case Key.A:
                case Key.LeftArrow:
                    RectangleLocation = new Point(RectangleLocation.X - 25, RectangleLocation.Y);
                    break;

                case Key.D:
                case Key.RightArrow:
                    RectangleLocation = new Point(RectangleLocation.X + 25, RectangleLocation.Y);
                    break;
            }
        }

        private static void OnTick(object sender, TickEventArgs e)
        {
            WindowSurface.Fill(Color.CornflowerBlue);
            Rectangle.Fill(Color.Red);

            WindowSurface.Lock();
            WindowSurface.Blit(Rectangle, RectangleLocation);
            WindowSurface.Unlock();

            WindowSurface.Update();
        }
    }
}