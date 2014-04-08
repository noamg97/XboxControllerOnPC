using Microsoft.Xna.Framework.Input;
using System;

namespace XboxControllerOnPC
{
    class MouseLikeButton : IXboxButton
    {
        Buttons button;
        MouseButton mouseButton;
        bool wasUp = false;


        /// <summary>
        /// Ctor for an object that manages an xbox button that acts like a mouse button
        /// </summary>
        /// <param name="button">The xbox button</param>
        /// <param name="mouseButton">The mouse button that is being copied</param>
        public MouseLikeButton(Buttons button, MouseButton mouseButton)
        {
            this.button = button;
            this.mouseButton = mouseButton;
            this.wasUp = mouseButton == MouseButton.right_mouse;
        }

        /// <summary>
        /// Executes the button's wanted action when it's being pressed
        /// </summary>
        /// <param name="xboxState">The current state of the Xbox Controller</param>
        /// <param name="mouseState">The current state of the mouse</param>
        public void Update(GamePadState xboxState, MouseState mouseState)
        {
            if (xboxState.IsButtonDown(button))
            {
                if (wasUp)
                {
                    mouse_event(Down, (uint)mouseState.X, (uint)mouseState.Y, 0, 0);
                    wasUp = false;
                }
            }
            else
            {
                if (!wasUp) mouse_event(Up, (uint)mouseState.X, (uint)mouseState.Y, 0, 0);
                wasUp = true;
            }
        }




        private uint Down
        {
            get
            {
                switch (mouseButton)
                {
                    case MouseButton.left_mouse: return MOUSEEVENTF_LEFTDOWN;
                    case MouseButton.middle_mouse: return MOUSEEVENTF_MIDDLEDOWN;
                    case MouseButton.right_mouse: return MOUSEEVENTF_RIGHTDOWN;
                }
                return MOUSEEVENTF_LEFTDOWN;
            }
        }
        private uint Up
        {
            get
            {
                switch (mouseButton)
                {
                    case MouseButton.left_mouse: return MOUSEEVENTF_LEFTUP;
                    case MouseButton.middle_mouse: return MOUSEEVENTF_MIDDLEUP;
                    case MouseButton.right_mouse: return MOUSEEVENTF_RIGHTUP;
                }
                return MOUSEEVENTF_LEFTUP;
            }
        }


        public enum MouseButton
        {
            left_mouse,
            middle_mouse,
            right_mouse
        }


        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
    }
}
