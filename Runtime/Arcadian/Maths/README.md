# Maths

> Click [here](../../../README.md#features) to go back.

## `Curves`

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