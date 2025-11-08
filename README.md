# Arcadian Assets

A collection of shared Unity tools and utilities for internal projects.


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
            "org.nuget",
            "com.dbrizov"
        ]
    }
],
```
3. Open **Window -> Package Manager**
4. Add https://github.com/lucaswarwick02/arcadian-assets.git as a Git URL


## Features

Below is a list of the different submodules:

- [Animation](Runtime/Arcadian/Animation/README.md) (`AnimationLoop`, `AnimationGroup`)
- [Effects](Runtime/Arcadian/Effects/README.md) (`Pulse`)
- [Enums](Runtime/Arcadian/Enums/README.md) (`Direction2D`)
- [Extensions](Runtime/Arcadian/Extensions/README.md)  (`Color`, `Enumerable`, `MonoBehaviour`, `Number`, `Object`, `Sprite`, `String`, `Transform`, `TypeReference`, `Vector`)
- [GameObjects](Runtime/Arcadian/GameObjects/README.md) (`SmoothCameraFollow`, `UnityEventHook`)
- [Generic](Runtime/Arcadian/Generic/README.md) (`Applicator<T>`, `Ref<T>`)
- [IO](Runtime/Arcadian/IO/README.md) (`AbstractSaveData<T>`)
- [Maths](Runtime/Arcadian/Maths/README.md) (`Curves`)
- [Pathfinding](Runtime/Arcadian/Pathfinding/README.md) (`GridMovement`, `GridPathfinder`, `Node`)
- Shaders
- [Sound](Runtime/Arcadian/Sound/README.md) (`SoundEffect`, `SoundEffectInstance`)
- [StateManagement](Runtime/Arcadian/StateManagement/README.md) (`StateMachine`)
- [UI](Runtime/Arcadian/UI/README.md) (`AbstractDeveloperConsole`, `DraggableUI`, `FloatingText`, `InteractiveButton`, `SceneTransition`, `WorldSpaceUI`)
  - [UI.Scrolling](Runtime/Arcadian/UI/Scrolling/README.md) (`AutoScrollContent`, `HorizontalAutoScroll`, `VerticalAutoScroll`)