# Arcadian Assets

A collection of shared Unity tools and utilities for internal projects.

---

## Setup Guide

1. Open **Edit -> Project Settings -> Project Manager**
2. Under **Scoped Registries**, add the following:
```json
"scopedRegistries": [
    {
        "name": "package.openupm.com",
        "url": "https://package.openupm.com",
        "scopes": [
            "com.solidalloy",
            "com.openupm",
            "org.nuget"
        ]
    }
],
```
3. Open **Window -> Package Manager**
4. Add https://github.com/lucaswarwick02/arcadian-assets.git as a Git URL

---

## Features

Below is a list of the different submodules:

> [WIP] Items in this list have been added, but not fully documented yet.

- [x] [Animation](Runtime/Arcadian/Animation/README.md) (`AnimationLoop`, `AnimationGroup`)
- [x] [Effects](Runtime/Arcadian/Effects/README.md) (`Pulse`)
- [x] [Enums](Runtime/Arcadian/Enums/README.md) (`Direction2D`)
- [x] [Extensions](Runtime/Arcadian/Extensions/README.md)  (`Color`, `Enumerable`, `MonoBehaviour`, `Number`, `Object`, `Sprite`, `String`, `Transform`, `TypeReference`, `Vector`)
- [ ] [GameObjects](#gameobjects) (`SmoothCameraFollow`, `UnityEventHook`)
- [ ] [Generic](#generic) (`Applicator`, `Ref`)
- [ ] [Maths](#maths) (`Curves`)
- [ ] [Pathfinding]
- [ ] [Shaders]
- [ ] [Sound]
- [ ] [StateManagement]
- [ ] [System]
- [ ] [UI]

### GameObjects

`SmoothCameraFollow`


A reusable Unity component for smoothly following a target (e.g., player) with damping and optional screen shake. Useful for creating dynamic, responsive camera motion that feels natural while maintaining focus on the target.

Example Usage:
```c#
using Arcadian.GameObjects;
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

`UnityEventHooks`

A reusable Unity component that exposes UnityEvents for common MonoBehaviour lifecycle hooks (`Awake`, `OnEnable`, `OnDisable`). Useful for triggering actions directly in the inspector without writing boilerplate code.

Example Usage:
```c#
using Arcadian.GameObjects;
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

### Generic

`Applicator`

A generic utility class that stores a list of functions or actions and invokes them in a sequence. Useful for building modular, chainable operations or event-like systems where you want to process a value through multiple transformations, or none at all. Good example would be equipment in an RPG that provide bonuses when equipped. 

Example Usage:
```c#
using Arcadian.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        // Applicator for transforming an int
        var intApplicator = new Applicator<int>();
        intApplicator.Add(x => x + 5);
        intApplicator.Add(x => x * 2);

        int result = intApplicator.Invoke(3); // (3 + 5) * 2 = 16
        Debug.Log($"Result: {result}");

        // Applicator for simple actions
        var actionApplicator = new Applicator();
        actionApplicator.Add(() => Debug.Log("First action"));
        actionApplicator.Add(() => Debug.Log("Second action"));
        actionApplicator.Invoke();
    }
```

`Ref`

A simple generic struct that wraps a value in a reference-like container. Useful for passing values by reference, or storing mutable data in contexts where normal value types would be copied. Useful for scenarios where `ref` cannot be used (bit pedantic, but does have it's use cases).

Example Usage:
```c#
using Arcadian.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        var health = new Ref<int>(100);

        // Modify value through the reference
        health.Value -= 25;
        Debug.Log($"Health: {health.Value}"); // Outputs 75

        // Pass the reference to another method
        IncreaseHealth(health);
        Debug.Log($"Health after increase: {health.Value}"); // Outputs 125
    }

    void IncreaseHealth(Ref<int> hpRef)
    {
        hpRef.Value += 50;
    }
}
```

### Maths

`Curves`

A lightweight static helper providing reusable Unity `AnimationCurve` presets for common "ease in" and "ease out" transitions. Useful for animations, UI effects, or smooth value interpolation without manually defining curves each time.

```c#
using Arcadian.Maths;
using UnityEngine;

public class Example : MonoBehaviour
{
    public Transform target;
    public float duration = 1f;

    void Start()
    {
        StartCoroutine(MoveWithCurve());
    }

    IEnumerator MoveWithCurve()
    {
        Vector3 start = transform.position;
        Vector3 end = target.position;

        float time = 0f;
        while (time < duration)
        {
            // Smoothly interpolate position using the 'In' curve
            float t = Curves.In.Evaluate(time / duration);
            transform.position = Vector3.Lerp(start, end, t);

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }
}
```