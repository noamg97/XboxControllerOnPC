using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace XboxControllerOnPC
{
    class ButtonsProcessor
    {
        /// <summary>
        /// Static function that manages and updates everything that envolves the controller's buttons
        /// </summary>
        /// <param name="xboxState">The current state of the Xbox controller</param>
        /// <param name="mouseState">The current state of the mouse</param>
        public static void Process(GamePadState xboxState, MouseState mouseState)
        {
            //Check for exit
            if (xboxState.IsButtonDown(ApplicationData.exitProcess.Value))
                Environment.Exit(0);

            //Update each button on the buttons list
            foreach (IXboxButton btn in ApplicationData.xboxButtons)
                btn.Update(xboxState, mouseState);
        }
    }



    class ThumbSticksProcessor
    {
        static Vector2 thumbSticks = Vector2.Zero;


        /// <summary>
        /// Static function that manages and updates mouse movement, making it correspond with the controller's thumbsticks
        /// </summary>
        /// <param name="xboxState">The current state of the Xbox controller</param>
        /// <param name="mouseState">The current state of the mouse</param>
        public static void Process(GamePadState xboxState, MouseState mouseState)
        {
            thumbSticks.X += ApplicationData.RightThumbstickSensitivity * xboxState.ThumbSticks.Right.X;
            thumbSticks.Y -= ApplicationData.RightThumbstickSensitivity * xboxState.ThumbSticks.Right.Y;
            thumbSticks.X += ApplicationData.LeftThumbstickSensitivity * xboxState.ThumbSticks.Left.X;
            thumbSticks.Y -= ApplicationData.LeftThumbstickSensitivity * xboxState.ThumbSticks.Left.Y;



            Mouse.SetPosition((int)thumbSticks.X, (int)thumbSticks.Y);
        }
        /// <summary>
        /// Resets the [Vector2 thumbSticks] to the current position of the mouse at the beginning of every loop, allowing the controller to not override the actual mouse
        /// </summary>
        /// <param name="mouseState">The current state of the mouse</param>
        public static void ResetToMouse(MouseState mouseState)
        {
            thumbSticks = new Vector2(mouseState.X, mouseState.Y);
        }
    }

}
