using SFML.Graphics;
using SFML.Window;
using System;

namespace FlappyBirdClone
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(1024, 768), "SFML graphics with OpenGL", Styles.Default);
            var cornFlowerBlue = new Color(154, 206, 235);

            window.SetVerticalSyncEnabled(true);

            window.Closed += (_, __) => window.Close();
            window.KeyReleased += Window_KeyReleased;

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(cornFlowerBlue);
                window.Display();
            }
        }

        private static void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Space)
            {

            }
        }
    }
}
