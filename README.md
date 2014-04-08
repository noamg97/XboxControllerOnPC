Use An Xbox Controller On Your PC
-----------------------------------------------
<br/><br/>
XboxControllerOnPC is a small open source C# program that allows you to easily use your Xbox controller as a mouse and partially as a keyboard, while connected to a windows PC.
The Xbox controller comes with a driver nicely fitted for video games, however, it doesn't support a use of the controller instead of a keyboard&mouse, something that can be very comfortable sitting on a sofa or away from a table or just for while taking a break from a video game you play with the controller.
XboxControllerOnPC was created after failing to find a program on the internet for this purpose, and since it seemed like quite a trivial program - that offers a very convenient solution.
<br/><br/>

<h3>Using XboxControllerOnPC</h>
-----------------------------------------------
To run the program, simply click the .exe file at the "\Release" directory, and connect an xbox 360 (xbox 1 untested) controller to your PC. The program will run at the background and you will not notice it's presence except from when using your xbox controller.
Before starting a video game, you should exit the program (default Exit key is the xbox's "Back" button), since the OS will probably think it's a mouse&keyboard, and the xbox buttons might not be interpreted correctly. 


When running the program, a pop-up message will show asking you whether you want the application to run at Start-Up.
Pressing "Yes" will create a shortcut to the program on "shell:startup". Pressing "No" will result in another pop-up message, asking whether or not it should keep asking on later runs.
You can always change your decision by changing the "~~ Ask For Auto Start ~~" key on the application's data file:

All the settings and default buttons can be found at "\Release\ApplicationData.txt" and can be changed to your liking, using the following format:

<code>
~~ [KEY] ~~<br/>
[Variable \ PC Action] = [Value \ Xbox Controller Button]<br/>
[Variable \ PC Action] = [Value \ Xbox Controller Button]<br/>
[Variable \ PC Action] = [Value \ Xbox Controller Button]<br/>
</code>

<h5>Supported [KEY]s:</h5><br/>
	<b>-"Ask For Auto Start"</b>: specifies whether or not the program should ask to start at Start-Up.<br/>
		[Value must be true\false]<br/>
	<b>-"Thumb-sticks Sensitivity"</b>: specifies the ratio between the mouse speed and the controller's thumb-stick.<br/>
		[Two variables must be declared and assigned: "RightThumbstickSensitivity" and "LeftThumbstickSensitivity"]<br/>
		[Value must be a valid number]<br/>
	<b>-"Special Buttons"</b>: assigns value to non-regular buttons (exits button & buttons that need to be added to other button presses, such as Ctrl, Shift, and Alt)<br/>
		[Optional variables are "Exit", "Shift", "Control", "Alt"]<br/>
		[Value must be a valid Xbox button, as will be specified later]<br/>
	<b>-"Mouse-Like Buttons"</b>: buttons on the xbox controller that act like a mouse button.<br/>
		[Optional variables are "LeftMouse", "RightMouse", "MiddleMouse"]<br/>
		[Value must be a valid Xbox button, as will be specified later]<br/>
	<b>-"Keyboard-Like Buttons"</b>: buttons on the xbox controller that act like a keyboard button or a combination of such.<br/>
		[The variable specified must be in Windows' format, extensively explained at: http://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.send(v=vs.110).aspx]<br/>
		[Value must be a valid Xbox button, as will be specified later, optionally followed by two other variables, also specified later in this README]<br/>

		
		
The valid Xbox Buttons are the same as declared at Microsoft.XNA.Framework.Input.Buttons, described here: http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.buttons.aspx

Under the key "Keyboard-Like Buttons", more values can be assigned except from the xbox button.
The two other values are optional, and should be separated by a separator ("|").
The first value that could be added, tells the program whether or not the button's action should occur only once (button down/button up), or every once in a while while the button is pressed. "true" or "false".
The second optional value, telling the program how many milliseconds should it wait between each call while the button is pressed. In order to use this value, you have to set the first one to "true".


<h3>Additional Notes</h>
==============
XboxControllerOnPC partially implements the System.Windows.Forms & Microsoft's XNA libraries for easier use (code-wise) of the xbox controller and the windows API.
The source code isn't too big nor is it hard to understand, feel free to improve it & add features to your liking.

<br/>
<br/>

enjoy !~
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>

-- Originally developed by Noam Gal --
