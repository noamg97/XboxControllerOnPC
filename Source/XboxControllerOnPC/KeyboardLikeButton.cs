using Microsoft.Xna.Framework.Input;
using System;
using Timer = System.Diagnostics.Stopwatch;
using keyboard_event = System.Windows.Forms.SendKeys;

namespace XboxControllerOnPC
{
    class KeyboardLikeButton : IXboxButton
    {
        Buttons button;
        string keyboardButton;
        bool wasUp = false;
        bool useTimer; Timer timer; int timerInterval;
        static bool shift = false, control = false, alt = false;

        /// <summary>
        /// Ctor for an object that manages an xbox button that acts like a keyboard button
        /// </summary>
        /// <param name="button">The desired Xbox button</param>
        /// <param name="keyboardButton">The desired keyboard key the button should replace</param>
        /// <param name="useTimer">
        /// Whether or not the button's action should occur only once (button down/button up), or every once in a while while the button is pressed.
        /// Use for buttons such as the Arrow keys.
        /// </param>
        /// <param name="timerInterval">If [useTimer] was set to true, [timerInterval] sets the time between each action while the button is pressed</param>
        public KeyboardLikeButton(Buttons button, string keyboardButton, bool useTimer = false, int timerInterval = 100)
        {
            this.button = button;
            this.keyboardButton = keyboardButton;
            this.useTimer = useTimer;
            if (useTimer)
            {
                timer = new Timer();
                timer.Start();
                this.timerInterval = timerInterval;
            }
        }

        /// <summary>
        /// Updates the button and executes it's wanted action if necessary
        /// </summary>
        /// <param name="xboxState">The current state of the Xbox Controller</param>
        /// <param name="mouseState">The current state of the mouse</param>
        public void Update(GamePadState xboxState, MouseState mouseState)
        {
            //Update the special buttons
            if (ApplicationData.control.HasValue && button == ApplicationData.control.Value) { control = xboxState.IsButtonDown(button); return; }
            if (ApplicationData.shift.HasValue && button == ApplicationData.shift.Value) { shift = xboxState.IsButtonDown(button); return; }
            if (ApplicationData.alt.HasValue && button == ApplicationData.alt.Value) { alt = xboxState.IsButtonDown(button); return; }




            if (xboxState.IsButtonDown(button))
            {
                if (wasUp || (useTimer && timer.ElapsedMilliseconds > timerInterval))
                {
                    string addons = (control ? KeyboardButton.Control : "") + (shift ? KeyboardButton.Shift : "") + (alt ? KeyboardButton.Alt : "");

                    keyboard_event.SendWait(addons + keyboardButton);

                    wasUp = false;
                    if (useTimer) timer.Restart();
                }
            }
            else
                wasUp = true;
        }
    }
}


public static class KeyboardButton
{
    public const string
        Left = "{LEFT}", Right = "{RIGHT}", Up = "{UP}", Down = "{DOWN}",
        Control = "^", Shift = "+", Alt = "%",
        Tab = "{TAB}",
        Backspace = "{BS}",
        Enter = "{ENTER}",
        Home = "{HOME}", End = "{END}",
        F4 = "{F4}";
}
