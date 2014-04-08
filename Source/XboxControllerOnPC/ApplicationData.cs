using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxControllerOnPC
{
    static class ApplicationData
    {
        // --------- Exit Button --------- //
        public static Buttons? exitProcess;


        // --------- Special Buttons --------- // Special buttons are keyboard buttons that should be added to a different button when it's being pressed. [Control/Shift/Alt]
        public static Buttons? shift;
        public static Buttons? control;
        public static Buttons? alt;

        // --------- Other Buttons --------- //
        public static List<IXboxButton> xboxButtons = new List<IXboxButton>();


        // --------- Thumbsticks Sensitivities --------- //
        public static int RightThumbstickSensitivity;
        public static int LeftThumbstickSensitivity;
    }
}
