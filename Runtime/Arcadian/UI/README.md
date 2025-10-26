# UI

> Click [here](../../../README.md#features) to go back.

## `AbstractDeveloperConsole`

A base class for creating an in-game developer console in Unity, allowing commands to be typed, executed, and displayed through a minimal overlay UI. It provides built-in commands like `echo`, clear`, and `scene`, along with support for adding custom commands, maintaining command history, and persisting across scene loads. The class and activation key must be set in the package settings in order to use your console.

Example Usage:
```c#
using Arcadian.UI;
using System.Collections.Generic;

public class MyConsole : AbstractDeveloperConsole
{
    protected override Dictionary<string, Func<string[], string>> GetCommands()
    {
        return new Dictionary<string, Func<string[], string>>
        {
            { "sayhello", _ => "Hello, Developer!" },
            { "add", args =>
                {
                    if (args.Length < 2) return "Usage: add <a> <b>";
                    return (int.Parse(args[0]) + int.Parse(args[1])).ToString();
                }
            }
        };
    }
}

```