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

## `DraggableUI<T>`

A generic Unity component for making UI elements draggable, detecting valid drop targets of type `T`, and optionally snapping back to their original position. It handles drag events, proximity-based target detection, and provides hooks for entering, exiting, and dropping onto targets.

Example Usage:
```c#
using Arcadian.UI;
using UnityEngine;

public class DraggableItem : DraggableUI<DropZone>
{
    protected override bool IsValidTarget(DropZone target) => true;

    protected override void OnTargetEnter(DropZone target)
    {
        Debug.Log($"Entered drop zone: {target.name}");
    }

    protected override void OnTargetExit(DropZone target)
    {
        Debug.Log($"Exited drop zone: {target.name}");
    }

    protected override void OnTargetDrop(DropZone target)
    {
        Debug.Log($"Dropped on: {target.name}");
    }
}

public class DropZone : MonoBehaviour { }
```

## `FloatingText`

A lightweight utility for displaying temporary animated text in world space, ideal for effects like damage numbers or notifications. It spawns a <c>TextMeshPro</c> object, animates it upward with fade-in/out transitions, then destroys it after completion.

Example Usage:
```c#
using UnityEngine;
using Arcadian.UI;

public class DamagePopup : MonoBehaviour
{
    public void ShowDamage(float amount, Vector3 position)
    {
        FloatingText.Instantiate(
            text: $"-{amount:F0}",
            position: position,
            fontSize: 6f,
            duration: 0.75f,
            animationDuration: 0.25f,
            offset: 1f,
            rotation: 5f
        );
    }
}
```

## `InteractiveButton`

Unity component to allow for single functions to be defined for both gamepad and mouse/keyboard interactions for UI elements such as buttons.

Example Usage:
```c#
public class StartGameButton : InteractiveButton
{
    public override void OnEnter()
    {
        // Highlight or animate the button further if desired
        Debug.Log("Start Game button focused");
    }

    public override void OnExit()
    {
        // Revert highlight or animation
        Debug.Log("Start Game button unfocused");
    }

    public override void OnSubmit(PointerEventData eventData = null)
    {
        // Trigger game start logic
        Debug.Log("Start Game button clicked!");
        SceneTransition.LoadScene("GameScene");
    }
}
```

## `SceneTransition`

A utility for smooth scene loading with fade an optional prefab-based animations. It overlays a full-screen canvas, fades to black, loads the new scene, optionally plays a custom transition prefab implementing `ISceneTransitionAnimation`, then fades back out. The static `LoadScene` methods support both scene names and built indices for flexible use.

Example Usage:
```c#
public class ExampleButton : MonoBehaviour
{
    [SerializeField] private string sceneName; 

    public void OnClick()
    {
        // Change scene by fading to back
        SceneTransition.LoadScene(sceneName);
    }
}
```

## `ISceneTransitionAnimation`

An interface for defining custom scene transition animations used by <c>SceneTransition</c>. Implementations should perform visual effects (like text pop-ups) in a coroutine, returning control once complete.

Example Usage:
```c#
public class FadeLogoTransition : MonoBehaviour, ISceneTransitionAnimation
{
    [SerializeField] private CanvasGroup logoGroup;

    public IEnumerator Play()
    {
        // Fade in logo
        yield return this.Tween(0.5f, t => logoGroup.alpha = Mathf.Lerp(0, 1, t));
        yield return new WaitForSeconds(0.5f);

        // Fade out logo
        yield return this.Tween(0.5f, t => logoGroup.alpha = Mathf.Lerp(1, 0, t));
    }
}
```