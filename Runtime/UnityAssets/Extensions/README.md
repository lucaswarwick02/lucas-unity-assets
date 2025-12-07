# Extensions

> Click [here](../../../README.md#features) to go back.

## `Color`

A set of extension methods for `Color` to easily adjust brightness, darkness, alpha, saturation, and value. Useful for dynamically modifying colors in UI or visual effects without manually converting between color spaces.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using UnityEngine;

public class Example : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        Color original = spriteRenderer.color;

        // Brighten the color by multiplying value
        spriteRenderer.color = original.MultiplyValue(1.5f);

        // Reduce saturation
        spriteRenderer.color = spriteRenderer.color.MultiplySaturation(0.5f);

        // Adjust hue
        spriteRenderer.color = spriteRenderer.color.MultiplyHue(1.2f);

        // Set alpha to semi-transparent
        spriteRenderer.color = spriteRenderer.color.SetAlpha(0.5f);
    }
}
```

## `Enumerable`

A collection of extension methods for `IEnumerable` and `IReadOnlyList` to simplify iteration and weighted random selection. Useful for clener looping logic and probabilistic element picking without writing boilerplate code.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
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

## `MonoBehaviour`

A set of `MonoBehaviour` extension methods for timing and scheduling actions. Useful for invoking callbacks at the end of a frame, or running basic tweens.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
using System.Collections;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        // Invoke an action at the end of the current frame
        this.InvokeEndOnFrame(() => Debug.Log("End of frame reached"));
    }
}
```

## `Numbers`

An extension method for converting integers to Roman numerals. Useful for displaying numbers in a classical or stylized format (e.g., UI labels, levels, ranks) without manual mapping logic.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
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

## `Object`

A set of extension methods for `GameObject` and `Object`, providing easy access to child components and deep cloning. Useful for quickly finding nested components or duplicating objects without manually copying fields. 

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
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

## `Sprite`

A set of extension methods for `Sprite` to manipulate pivots, locate pixels by color, covert pixel positions to UV offsets, an remove specific pixel colors. Useful for dynamically adjusting sprite origins, detecting color positions, or creating modified textures at runtime.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
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

## `String`

A set of extension methods for `string` to handle pluralization, formatting, casing, rich text styling, and emoji insertion. Useful for dynamically generating readable, stylized, and interactive text in UI without manual string manipulation. 

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
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

## `Transform`

A `Transform` extension method to find the closest `MonoBehaviour` from a list. Useful for targetting the nearest object, enemy, or interactive element efficiently without repeatedly calculating distances manually. 

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
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

## `TypeReference`

A simple extension for `TypeReference` to create an instance of the referenced type and cast it to a specified generic type. Useful for dynamically instantiating types without manually calling `Activator.CreateInstance`.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
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

## `Vector`

A set of extension methods for `Vector2` and `Vector3` to calculate angles, convert between 2D and 3D, and flip or negate axis. Useful for simplifying common vector math operations in gameplay and UI logic. 

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
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