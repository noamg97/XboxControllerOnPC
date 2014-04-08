using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using System.IO;
using MsgBox = System.Windows.Forms.MessageBox;

namespace XboxControllerOnPC
{
    class MainClass
    {
        static GamePadState xboxState;
        static MouseState mouseState;
        static KeyboardState keyboardState;
        static Stopwatch stopWatch = new Stopwatch();
        static bool askForAutoStart = true;



        public static void Main()
        {
            LoadData();

            if (askForAutoStart && !File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\XboxControllerOnPC.lnk"))
            {
                System.Windows.Forms.DialogResult res = MsgBox.Show("Would you like this program to automatically open on StartUp ?", "", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (res == System.Windows.Forms.DialogResult.Yes)
                    CreateShortcutAtStartUpFolder();
                else
                {
                    System.Windows.Forms.DialogResult res2 = MsgBox.Show("Stop Asking ?", "", System.Windows.Forms.MessageBoxButtons.YesNo);
                    if (res2 == System.Windows.Forms.DialogResult.Yes)
                        ChangeAskForAutoStart(false);
                }
            }

            while (true)
            {
                InitializeNewLoop();

                if (xboxState.IsConnected)
                {
                    ThumbSticksProcessor.Process(xboxState, mouseState);
                    ButtonsProcessor.Process(xboxState, mouseState);
                }
                else
                    Thread.Sleep(2000);


                Thread.Sleep(Math.Max(0, (1000 / 60) - (int)stopWatch.ElapsedMilliseconds));
            }
        }

        static void InitializeNewLoop()
        {
            stopWatch.Restart();

            xboxState = GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            ThumbSticksProcessor.ResetToMouse(mouseState);
        }



        /// <summary>
        /// An ugly piece of code that loads the data file and parses it
        /// </summary>
        static void LoadData()
        {
            try
            {
                //Load the text from the file and seperate it to lines
                string[] lines = GetData();


                //For each line that start and ends with a '~~', we check what segment comes after it, and parse it accordingly
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("~~") && lines[i].EndsWith("~~"))
                    {
                        #region Ask for auto start
                        if (lines[i].Contains("Ask For Auto Start"))
                        {
                            askForAutoStart = lines[++i].ToLower().Contains("true");
                        }
                        #endregion

                        #region Thumbsticks Sensitivity
                        if (lines[i].Contains("Thumbsticks Sensitivity"))
                        {
                            for (int j = 0; j < 2 && i + 1 < lines.Length && !lines[i + 1].Contains("~~"); j++)
                            {
                                i++;
                                lines[i] = lines[i].Replace(" ", "");
                                string value = lines[i].Substring(lines[i].IndexOf("=") + 1);

                                if (lines[i].Contains("RightThumbstickSensitivity")) ApplicationData.RightThumbstickSensitivity = Convert.ToInt32(value);
                                else if (lines[i].Contains("LeftThumbstickSensitivity")) ApplicationData.LeftThumbstickSensitivity = Convert.ToInt32(value);
                            }
                        }
                        #endregion

                        #region Special Buttons
                        else if (lines[i].Contains("Special Buttons"))
                        {
                            for (int j = 0; j < 4 && i + 1 < lines.Length && !lines[i + 1].Contains("~~"); j++)
                            {
                                i++;
                                lines[i] = lines[i].Replace(" ", "");
                                string value = lines[i].Substring(lines[i].IndexOf("=") + 1);

                                Buttons? btn = ButtonFromString(value);
                                if (btn.HasValue)
                                {
                                    if (lines[i].Contains("Exit"))
                                    {
                                        ApplicationData.exitProcess = btn;
                                    }
                                    else if (lines[i].Contains("Shift"))
                                    {
                                        ApplicationData.shift = btn;
                                        ApplicationData.xboxButtons.Add(new KeyboardLikeButton(ApplicationData.shift.Value, KeyboardButton.Shift));
                                    }
                                    else if (lines[i].Contains("Control"))
                                    {
                                        ApplicationData.control = btn;
                                        ApplicationData.xboxButtons.Add(new KeyboardLikeButton(ApplicationData.control.Value, KeyboardButton.Control));
                                    }
                                    else if (lines[i].Contains("Alt"))
                                    {
                                        ApplicationData.alt = btn;
                                        ApplicationData.xboxButtons.Add(new KeyboardLikeButton(ApplicationData.alt.Value, KeyboardButton.Alt));
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Mouse-Like Buttons
                        else if (lines[i].Contains("Mouse-Like Buttons"))
                        {
                            for (int j = 0; j < 3 && i + 1 < lines.Length && !lines[i + 1].Contains("~~"); j++)
                            {
                                i++;
                                lines[i] = lines[i].Replace(" ", "");
                                string value = lines[i].Substring(lines[i].LastIndexOf(".") + 1);

                                Buttons? btn = ButtonFromString(value);

                                if (btn.HasValue)
                                {
                                    if (lines[i].Contains("LeftMouse")) ApplicationData.xboxButtons.Add(new MouseLikeButton(btn.Value, MouseLikeButton.MouseButton.left_mouse));
                                    else if (lines[i].Contains("MiddleMouse")) ApplicationData.xboxButtons.Add(new MouseLikeButton(btn.Value, MouseLikeButton.MouseButton.middle_mouse));
                                    else if (lines[i].Contains("RightMouse")) ApplicationData.xboxButtons.Add(new MouseLikeButton(btn.Value, MouseLikeButton.MouseButton.right_mouse));
                                }
                            }
                        }
                        #endregion

                        #region Keyboard-Like Buttons
                        else if (lines[i].Contains("Keyboard-Like Buttons"))
                        {
                            while (i + 1 < lines.Length && !lines[i + 1].Contains("~~"))
                            {
                                i++;
                                lines[i] = lines[i].Replace(" ", "");
                                string value = lines[i].Substring(lines[i].LastIndexOf(".") + 1);

                                Buttons? btn;
                                if (value.Contains("|"))
                                {
                                    string[] parts = value.Split('|');
                                    btn = ButtonFromString(parts[0]);
                                    if (btn.HasValue)
                                    {
                                        if (parts.Length == 3)
                                            ApplicationData.xboxButtons.Add(new KeyboardLikeButton(btn.Value, lines[i].Remove(lines[i].IndexOf('=')), parts[1].ToLower() == "true", Convert.ToInt32(parts[2])));
                                        else
                                            ApplicationData.xboxButtons.Add(new KeyboardLikeButton(btn.Value, lines[i].Remove(lines[i].IndexOf('=')), parts[1].ToLower() == "true"));
                                    }
                                }
                                else
                                {
                                    btn = ButtonFromString(value);

                                    if (btn.HasValue)
                                        ApplicationData.xboxButtons.Add(new KeyboardLikeButton(btn.Value, lines[i].Remove(lines[i].IndexOf('='))));
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception e)
            {
                MsgBox.Show("Error while parsing the data file", "", System.Windows.Forms.MessageBoxButtons.OK);
                Environment.Exit(0);
            }
        }

        static Buttons? ButtonFromString(string str)
        {
            switch (str.Substring(str.IndexOf('.') + 1))
            {
                case "A": return Buttons.A;

                case "B": return Buttons.B;

                case "Back": return Buttons.Back;

                case "BigButton": return Buttons.BigButton;

                case "DPadDown": return Buttons.DPadDown;

                case "DPadLeft": return Buttons.DPadLeft;

                case "DPadRight": return Buttons.DPadRight;

                case "DPadUp": return Buttons.DPadUp;

                case "LeftShoulder": return Buttons.LeftShoulder;

                case "LeftStick": return Buttons.LeftStick;

                case "LeftThumbstickDown": return Buttons.LeftThumbstickDown;

                case "LeftThumbstickLeft": return Buttons.LeftThumbstickLeft;

                case "LeftThumbstickRight": return Buttons.LeftThumbstickRight;

                case "LeftThumbstickUp": return Buttons.LeftThumbstickUp;

                case "LeftTrigger": return Buttons.LeftTrigger;

                case "RightShoulder": return Buttons.RightShoulder;

                case "RightStick": return Buttons.RightStick;

                case "RightThumbstickDown": return Buttons.RightThumbstickDown;

                case "RightThumbstickLeft": return Buttons.RightThumbstickLeft;

                case "RightThumbstickRight": return Buttons.RightThumbstickRight;

                case "RightThumbstickUp": return Buttons.RightThumbstickUp;

                case "RightTrigger": return Buttons.RightTrigger;

                case "Start": return Buttons.Start;

                case "X": return Buttons.X;

                case "Y": return Buttons.Y;

                case "None":
                default: return null;

            }
        }

        static string[] GetData()
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\ApplicationData.txt"; 

            try
            {
                return File.ReadAllText(path).Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception)
            {
                MsgBox.Show("Error while trying to load ApplicationData.txt\n\n" + path, "", System.Windows.Forms.MessageBoxButtons.OK);
                Environment.Exit(0);
                throw; //So the compiler will be happy
            }
        }

        static void ChangeAskForAutoStart(bool value)
        {
            string[] lines = GetData();

            for (int i = 0; i < lines.Length; i++)
                if (lines[i].StartsWith("~~") && lines[i].EndsWith("~~"))
                    if (lines[i].Contains("Ask For Auto Start"))
                    {
                        lines[i + 1] = "false";
                        break;
                    }

            File.WriteAllLines("ApplicationData.txt", lines);
        }



        static void CreateShortcutAtStartUpFolder()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\XboxControllerOnPC.lnk";

            IWshRuntimeLibrary.IWshShortcut shortcut = (new IWshRuntimeLibrary.WshShell()).CreateShortcut(path) as IWshRuntimeLibrary.IWshShortcut;
            shortcut.TargetPath = System.Windows.Forms.Application.StartupPath + "\\XboxControllerOnPC.exe";
            shortcut.Save();
        }
    }




    #region Interfaces
    public interface IXboxButton
    {
        void Update(GamePadState xboxState, MouseState mouseState);
    }
    #endregion
}
