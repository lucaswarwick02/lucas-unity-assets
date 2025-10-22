## Generic

> Click [here](../../../README.md#features) to go back.

## `Applicator<T>`

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

## `Ref<T>`

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