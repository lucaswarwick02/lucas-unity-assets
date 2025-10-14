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
- [ ] [Enums](#enums) (`Direction2D`)
- [ ] [Extensions](#extensions)  (`Color`, `Enumerable`, `MonoBehaviour`, `Number`, `Object`, `Sprite`, `String`, `Transform`, `TypeReference`, `Vector`)
- [ ] [GameObjects](#gameobjects) (`SmoothCameraFollow`, `UnityEventHook`)
- [ ] [Generic](#generic) (`Applicator`, `Ref`)
- [ ] [Maths](#maths) (`Curves`)
- [ ] [Pathfinding]
- [ ] [Shaders]
- [ ] [Sound]
- [ ] [StateManagement]
- [ ] [System]
- [ ] [UI]

### Enums

`Direction2D`

A simple enum representing 2D directions (`Up`, `Down`, `Left`, `Right`) with an extension to convert a `Vector2D` into the closest `Direction2D`. Useful for interpreting movement or input vectors in a grid or direction based system.

Example Usage:
```c#
using Arcadian.Enums;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Direction2D direction = input.ToDirection2D();
        Debug.Log($"Current direction: {direction}");
    }
}
```

### Extensions

`Color`

A set of extension methods for `Color` to easily adjust brightness, darkness, alpha, saturation, and value. Useful for dynamically modifying colors in UI or visual effects without manually converting between color spaces.

Example Usage:
```c#
using Arcadian.Extensions;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        Color original = Color.red;

        // Increase overall brightness by 20%
        Color brighter = original.Brighten(1.2f);

        // Decrease brightness by 30%
        Color darker = original.Darken(0.3f);

        // Set alpha to 50% transparency
        Color semiTransparent = original.Alpha(0.5f);

        // Increase saturation by 50%
        Color moreSaturated = original.MultiplySaturation(1.5f);

        // Increase value/brightness component in HSV by 30%
        Color brighterValue = original.MultiplyValue(1.3f);

        Debug.Log($"Brighter: {brighter}, Darker: {darker}, Alpha: {semiTransparent}");
    }
}
```

`Enumerable`

A collection of extension methods for `IEnumerable` and `IReadOnlyList` to simplify iteration and weighted random selection. Useful for clener looping logic and probabilistic element picking without writing boilerplate code.

Example Usage:
```c#
using Arcadian.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };

        // Apply an action to each element
        numbers.ForEach(n => Debug.Log($"Number: {n}"));

        // Select a random element with equal weight
        int randomNumber = numbers.Random();
        Debug.Log($"Random number: {randomNumber}");

        // Select a random element with custom weights (higher = more likely)
        int weightedRandom = numbers.Random(n => n); // Higher numbers have higher chance
        Debug.Log($"Weighted random number: {weightedRandom}");
    }
}
```

`MonoBehaviour`

A set of `MonoBehaviour` extension methods for timing and scheduling actions. Useful for invoking callbacks at the end of a frame, or smoothly running logic over a given duration without writing custom coroutines.

Example Usage:
```c#
using Arcadian.Extensions;
using System.Collections;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        // Invoke an action at the end of the current frame
        this.InvokeEndOnFrame(() => Debug.Log("End of frame reached"));

        // Start a coroutine to gradually move an object over 2 seconds
        StartCoroutine(this.RunOverTime(2f, progress =>
        {
            // 'progress' goes from 0 to 1 over time
            transform.position = Vector3.Lerp(Vector3.zero, Vector3.up * 5, progress);
        }));
    }
}
```

`Numbers`

An extension method for converting integers to Roman numerals. Useful for displaying numbers in a classical or stylized format (e.g., UI labels, levels, ranks) without manual mapping logic.

Example Usage:
```c#
using Arcadian.Extensions;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        int level = 12;

        // Convert integer to Roman numeral representation
        string roman = level.ToRoman();

        // Output: "XII"
        Debug.Log($"Level {level} in Roman numerals: {roman}");
    }
}
```

`Object`

A set of extension methods for `GameObject` and `Object`, providing easy access to child components and deep cloning. Useful for quickly finding nested components or duplicating objects without manually copying fields. 

Example Usage:
```c#
using Arcadian.Extensions;
using UnityEngine;

public class Example : MonoBehaviour
{
    public GameObject parentObject;
    public SomeComponent originalComponent;

    void Start()
    {
        // Get a child component by name
        SomeComponent childComp = parentObject.GetChildComponent<SomeComponent>("ChildName");
        if (childComp != null)
            Debug.Log("Found child component!");

        // Deep clone an object
        SomeComponent clone = originalComponent.DeepClone();
        Debug.Log("Cloned object: " + clone);
    }
}
```

`Sprite`

A set of extension methods for `Sprite` to manipulate pivots, locate pixels by color, covert pixel positions to UV offsets, an remove specific pixel colors. Useful for dynamically adjusting sprite origins, detecting color positions, or creating modified textures at runtime.

Example Usage:
```c#
using Arcadian.Extensions;
using UnityEngine;

public class Example : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color32 targetColor;

    void Start()
    {
        Sprite sprite = spriteRenderer.sprite;

        // Center the sprite pivot
        spriteRenderer.sprite = sprite.CenterSprite();

        // Calculate the sprite's current pivot
        Vector2 pivot = SpriteExtensions.CalculatePivot(sprite);
        Debug.Log($"Pivot: {pivot}");

        // Find the pixel point of a specific color
        Vector2Int pixelPoint = sprite.GetPixelPoint(targetColor);
        Debug.Log($"Pixel point: {pixelPoint}");

        // Convert pixel point to offset (0-1)
        Vector2 offset = sprite.PixelPointToOffset(pixelPoint);
        Debug.Log($"Offset: {offset}");

        // Remove specific colors from the sprite
        spriteRenderer.sprite = sprite.RemovePixelPoints(targetColor);
    }
}
```

`String`

A set of extension methods for `string` to handle pluralization, formatting, casing, rich text styling, and emoji insertion. Useful for dynamically generating readable, stylized, and interactive text in UI without manual string manipulation. 

Example Usage:
```c#
using Arcadian.Extensions;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        string word = "city";

        // Convert to plural
        string plural = word.ToPlural(); // "cities"
        Debug.Log(plural);

        // Convert PascalCase to Title Case
        string title = "PascalCaseString".PascalCaseToTitleCase(); // "Pascal Case String"
        Debug.Log(title);

        // Apply rich text formatting
        string styled = "Hello".Bold().Color(Color.red).Size(24).Italic();
        Debug.Log(styled);

        // Insert emoji from sprite asset
        string emojiText = "Status: ".GetEmoji("EmojiAsset", "check");
        Debug.Log(emojiText);

        // Dual alignment example
        string aligned = StringExtensions.DualAlign("Left", "Right");
        Debug.Log(aligned);
    }
}
```

`Transform`

A `Transform` extension method to find the closest `MonoBehaviour` from a list. Useful for targetting the nearest object, enemy, or interactive element efficiently without repeatedly calculating distances manually. 

Example Usage:
```c#
using Arcadian.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    public List<Enemy> enemies;

    void Update()
    {
        // Find the closest enemy to this transform
        Enemy closestEnemy = transform.GetClosest(enemies);

        if (closestEnemy != null)
            Debug.Log($"Closest enemy: {closestEnemy.name}");
    }
}
```

`TypeReference`

A simple extension for `TypeReference` to create an instance of the referenced type and cast it to a specified generic type. Useful for dynamically instantiating types without manually calling `Activator.CreateInstance`.

Example Usage:
```c#
using Arcadian.Extensions;
using TypeReferences;
using UnityEngine;

public class Example : MonoBehaviour
{
    public TypeReference someType;

    void Start()
    {
        // Instantiate the type referenced by 'someType' and cast to the desired type
        var instance = someType.Cast<MonoBehaviour>();
        Debug.Log($"Created instance of type: {instance.GetType().Name}");
    }
}
```

`Vector`

A set of extension methods for `Vector2` and `Vector3` to calculate angles, convert between 2D and 3D, and flip or negate axis. Useful for simplifying common vector math operations in gameplay and UI logic. 

Example Usage:
```c#
using Arcadian.Extensions;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        Vector2 v2 = new Vector2(1, 2);
        Vector3 v3 = v2.ToVector3(); // Convert to Vector3 with z = 0

        // Calculate angles
        float angleDeg = v2.AngleDeg();
        Debug.Log($"Angle in degrees: {angleDeg}");

        // Flip and negate vectors
        Vector2 flippedY = v2.FlipY(); // Y axis flipped
        Vector2 negated = v2.Negate(); // Both axes negated

        Vector3 flippedX3D = v3.FlipX(); // X axis flipped in 3D
        Vector3 negated3D = v3.Negate(); // All axes negated

        Debug.Log($"Flipped Y: {flippedY}, Negated: {negated}");
    }
}
```

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