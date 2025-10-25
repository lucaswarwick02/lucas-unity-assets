# Sound

> Click [here](../../../README.md#features) to go back.

## `SFX`

A ScriptableObject wrapper for easily playing sound effects via Addressables, supporting custom pitch, clip length, and mixer routing. Useful for managing reusable SFX assets that can be triggered anywhere without needing scene references or persistent AudioSources.

Example Usage:
```c#
using Arcadian.Sound;
using UnityEngine;

public class Example : MonoBehaviour
{
    public SFX buttonSFX;

    void OnMouseDown()
    {
        // Play sound at default settings
        buttonSFX.Play();

        // Play with altered length and randomised pitch
        buttonSFX.Play(clipLength: 0.5f, offsetPitch: true);
    }
}
```