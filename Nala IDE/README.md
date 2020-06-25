# Nala IDE

The Nala IDE is a purpose-built code editor designed for use with the [Nala Programming Language](https://github.com/ntwiles/Nala).

![](https://github.com/ntwiles/Nala-IDE/blob/master/Resources/screenshot-2.png)

## Features

### Present
#### Open Files Tab View
Users can open multiple files and easily alternate between them in real-time.

#### Built-In Interpreter
Users can click the Run toolbar button and launch their Nala code in a separate console.

### In Progress
#### Directory Hierarchy Panel
A panel will soon be added on the left side which lists files in the current working directory.

#### Custom Code Editor WPF Control
Since the built-in WPF TextBox control doesn't allow for inline styling, and because the RichTexBox is the RichTextBox, I'm working on a custom control (CodeEditor.cs) to serve as the Nala IDE text editor. This has become the biggest bottleneck in the project by far, but progress is moving forward well enough. 

#### Syntax Highlighting
The project of the above-mentioned control was set upon in order to achieve syntax highlighting. I've added in basic highlighting for keywords and primitive types already, but any further syntax highlighting is on hold until the more basic features of the TextBox control can be recreated in my custom control.

### Future
#### In-Editor Error Reporting
Future versions will check Nala code for errors in the background and report them before execution.

## Usage
For the time being, that Nala IDE is hardcoded for use on my local system. The directory hierchy on the left is searching for a particular directory, and the "Run" button on the toolbar looks for nala.exe in a specific place. These values should be easy to find and modify if you look through the code, but I intend to create a "preferences" window very soon to resolve this issue.
