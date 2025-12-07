# Enums

> Click [here](../../../README.md#features) to go back.

## `Direction2D`

A simple enum representing 2D directions (`Up`, `Down`, `Left`, `Right`) with an extension to convert a `Vector2D` into the closest `Direction2D`. Useful for interpreting movement or input vectors in a grid or direction based system.

Example Usage:
```c#
using LucasWarwick02.UnityAssets;
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