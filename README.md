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

- [Animation](Runtime/Arcadian/Animation/README.md) (`AnimationLoop`, `AnimationGroup`)
- [Effects](Runtime/Arcadian/Effects/README.md) (`Pulse`)
- [Enums](Runtime/Arcadian/Enums/README.md) (`Direction2D`)
- [Extensions](Runtime/Arcadian/Extensions/README.md)  (`Color`, `Enumerable`, `MonoBehaviour`, `Number`, `Object`, `Sprite`, `String`, `Transform`, `TypeReference`, `Vector`)
- [GameObjects](Runtime/Arcadian/GameObjects/README.md) (`SmoothCameraFollow`, `UnityEventHook`)
- [Generic](Runtime/Arcadian/Generic/README.md) (`Applicator`, `Ref`)
- [Maths](#maths) (`Curves`)
- Pathfinding
- Shaders
- Sound
- StateManagement
- System
- UI

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