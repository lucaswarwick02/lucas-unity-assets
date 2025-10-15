# Sound

> Click [here](../../../README.md#features) to go back.

## `SoundEffect`

A ScriptableObject wrapper for easily playing sound effects via Addressables, supporting custom pitch, clip length, and mixer routing. Useful for managing reusable SFX assets that can be triggered anywhere without needing scene references or persistent AudioSources.

Example Usage:
```c#
using Arcadian.Sound;
using UnityEngine;

public class Example : MonoBehaviour
{
    public SoundEffect buttonClick;

    void OnMouseDown()
    {
        // Play sound at default settings
        buttonClick.Play();

        // Play with altered length and randomised pitch
        buttonClick.Play(clipLength: 0.5f, offsetPitch: true);
    }
}
```

## `SoundEffectInstance`

A lightweight, self-contained audio component for playing temporary sound effects with optional pitch variance and automatic clean up. Useful for one-shot SFX like explosions, button clicks, or footsteps without manually managing object lifetimes.

Example Usage:
```c#
using Arcadian.Sound;
using UnityEngine;

public class Example : MonoBehaviour
{
    public AudioClip explosionClip;
    public AudioMixerGroup sfxMixer;
    public SoundEffectInstance sfxPrefab;

    void TriggerExplosion()
    {
        // Create a temporary sound effect instance
        var instance = Instantiate(sfxPrefab, transform.position, Quaternion.identity);
        
        instance.SetClip(explosionClip);
        instance.SetMixerGroup(sfxMixer);
        instance.OffsetPitch();      // Add slight variation
        instance.Play();             // Automatically destroys itself after playback
    }
}
```