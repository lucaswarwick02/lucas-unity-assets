# Effects

> Click [here](../../../README.md#features) to go back.

## `Pulse`

A simple Unity component that makes a GameObject smoothly pulse in size. Useful for drawing attention to UI elements or objects with a looping, easing-based scale effect.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using UnityEngine;

public class Example : MonoBehaviour
{
    public Pulse pulseEffect;

    void Start()
    {
        // Enable pulsing
        pulseEffect.enabled = true;
    }

    void Update()
    {
        // Disable pulsing on key press
        if (Input.GetKeyDown(KeyCode.Space))
            pulseEffect.enabled = false;
    }
}
```