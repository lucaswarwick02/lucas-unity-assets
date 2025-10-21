## IO

> Click [here](../../../README.md#features) to go back.

## `AbstractSaveData<T>`

A generic abstract base class that provides a JSON-based save system for persistent game data in Unity. It handles file reading, writing, and serialisation through `Newtonsoft.Json`, while maintaining a static `Current` instance for active save data. Derived classes define default values and filenames, making it easy to implement consistent and type-safe save systems across multiple data types.

Example Usage:
```c#
using Arcadian.System;
using UnityEngine;

public class PlayerSaveData : AbstractSaveData<PlayerSaveData>
{
    public int Level;
    public int Experience;
    public Vector3 Position;

    protected override PlayerSaveData DefaultSaveData()
    {
        return new PlayerSaveData
        {
            Level = 1,
            Experience = 0,
            Position = Vector3.zero
        };
    }

    protected override string FileName()
    {
        return "player_save.json";
    }
}

public class Example : MonoBehaviour
{
    void Start()
    {
        // Create a new save if one doesn’t exist
        if (!PlayerSaveData.Exists())
            PlayerSaveData.NewSave();

        // Load existing save
        PlayerSaveData.Load();
        Debug.Log($"Loaded level: {PlayerSaveData.Current.Level}");

        // Modify and save
        PlayerSaveData.Current.Level++;
        PlayerSaveData.Save();
    }
}
```