using System;
using UnityEngine;

public class ActorMovement : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Patrol,
        Chase
    }

    public AIState AiState { get; private set; }
    ActorController _actorController;

    private void Start()
    {
        _actorController = GetComponent<ActorController>();
        //DebugUtility.HandleErrorIfNullGetComponent<ActorController, ActorMovement>(_actorController, this, gameObject);

        AiState = AIState.Patrol;
    }

    private void Update()
    {
        UpdateAiStateTransitions();
        UpdateCurrentAiState();
    }

    void UpdateAiStateTransitions()
    {
        switch (AiState)
        {
            case AIState.Idle:
                break;
            case AIState.Patrol:
                break;
            case AIState.Chase:
                break;
        }
    }

    void UpdateCurrentAiState()
    {
            switch (AiState)
            {
                case AIState.Idle:
                    break;
                case AIState.Patrol:
                    _actorController.UpdatePathDestination();
                    _actorController.GetDestinationOnPath();
                    break;
                case AIState.Chase:
                    break;
            }
    }
}
