# Arcadian Assets

A colelction of shared Unity tools and utilities for internal projects.

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

### Table of Contents

Below is a list of the different submodules:
- [Animation](#animation)

### Animation

`AnimationLoop`

A reusable Unity component for quickly animating a sequence of sprites on a `SpriteRenderer` or `UI.Image`. Useful for creating looping or one-off animations (like effect, UI icons, or simple character aniamtions) without needing an Animator Controller, giving lightweight, scriptable control over timing, looping, and destruction on completion.

Example Usage:
```c#
using Arcadian.Animation;
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