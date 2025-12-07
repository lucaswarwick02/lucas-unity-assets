# Animation

> Click [here](../../../README.md#features) to go back.

## `AnimationLoop`

A reusable Unity component for quickly animating a sequence of sprites on a `SpriteRenderer` or `UI.Image`. Useful for creating looping or one-off animations (like effect, UI icons, or simple character animations) without needing an Animator Controller, giving lightweight, scriptable control over timing, looping, and destruction on completion.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using UnityEngine;

public class Example : MonoBehaviour
{
    public AnimationLoop animationLoop;

    void Start()
    {
        // Play the animation
        animationLoop.Play();

        // Subscribe to finish event
        animationLoop.onAnimationFinished += () => Debug.Log("Animation finished!");
    }

    void Update()
    {
        // Stop animation on space press
        if (Input.GetKeyDown(KeyCode.Space))
            animationLoop.Stop();
    }
}

```

## `AnimationSet`

A reusable Unity component for managing multiple animations on a `SpriteRenderer` or `UI.Image`. Useful for switching between named animation states (like character animations, UI transitions, or effects) with lightweight, scriptable control over frame timing, looping, and automatic frame updates.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public AnimationSet animationSet;

    void Start()
    {
        // Set default animation
        animationSet.SetAnimation("Idle");

        // Start animation playback
        animationSet.StartAnimation();

        // Subscribe to frame changes
        animationSet.onFrameChange += () => Debug.Log($"Frame changed: {animationSet.CurrentFrame}");
    }

    void Update()
    {
        // Switch animation on key press
        if (Input.GetKeyDown(KeyCode.Space))
            animationSet.SetAnimation("Run");
    }
}

```