# StateManagement

> Click [here](../../../README.md#features) to go back.

## `StateMachine`

A generic state machine for managing `State` objects. Automatically calls `EndState` on the previous state and `StartState` on the new state. Useful for AI, gameplay logic, or UI state management.

Example Usage:
```c#
using Arcadian.StateManagement;
using UnityEngine;

// Example concrete states
public class IdleState : State
{
    public override void StartState() => Debug.Log("Entered Idle State");
    public override void EndState() => Debug.Log("Exited Idle State");
}

public class RunningState : State
{
    public override void StartState() => Debug.Log("Entered Running State");
    public override void EndState() => Debug.Log("Exited Running State");
}

public class Example : MonoBehaviour
{
    private StateMachine<State> stateMachine;

    void Start()
    {
        var idle = new IdleState();
        var running = new RunningState();
        stateMachine = new StateMachine<State>(new State[] { idle, running });

        // Start in idle state
        stateMachine.SetState(typeof(IdleState));
    }

    void Update()
    {
        // Switch states based on input
        if (Input.GetKey(KeyCode.W))
            stateMachine.SetState(typeof(RunningState));
        else
            stateMachine.SetState(typeof(IdleState));
    }
}

```