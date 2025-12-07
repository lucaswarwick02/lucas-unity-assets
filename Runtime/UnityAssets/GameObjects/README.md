# GameObjects

> Click [here](../../../README.md#features) to go back.

## `SmoothCameraFollow`


A reusable Unity component for smoothly following a target (e.g., player) with damping and optional screen shake. Useful for creating dynamic, responsive camera motion that feels natural while maintaining focus on the target.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using UnityEngine;

public class Example : MonoBehaviour
{
    public SmoothCameraFollow cameraFollow;
    public Transform player;

    void Start()
    {
        // Assign player as the camera target
        cameraFollow.target = player;

        // Offset camera slightly behind and above the player
        cameraFollow.offset = new Vector3(0, 2, -10);
    }

    void Update()
    {
        // Trigger a medium shake for 0.5 seconds when pressing space
        if (Input.GetKeyDown(KeyCode.Space))
            SmoothCameraFollow.Shake(ShakeStrength.Medium, 0.5f);
    }
}
```

## `UnityEventHooks`

A reusable Unity component that exposes UnityEvents for common MonoBehaviour lifecycle hooks (`Awake`, `OnEnable`, `OnDisable`). Useful for triggering actions directly in the inspector without writing boilerplate code.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using UnityEngine;

public class Example : MonoBehaviour
{
    public UnityEventHooks eventHooks;

    void Start()
    {
        // In the Inspector, link functions to lifecycle events:
        // - OnAwake → Initialise game data
        // - OnStart → Spawn player
        // - OnEnableEvent → Enable HUD
        // - OnDisableEvent → Save progress
    }
}
```