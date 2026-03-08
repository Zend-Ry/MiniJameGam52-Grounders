using System.Collections.Generic;
using UnityEngine;

enum SearchState
{
    Idle,
    Blind,
    Noise,
    Path
}

[RequireComponent(typeof(ActorAnimationController))]
public class ActorController : MonoBehaviour
{
    private List<Transform> _actors;
    SearchState _searchState = SearchState.Idle;

    Vector2 moveDir = Vector2.zero;
    public Vector2 GetMoveDir() => moveDir;
    public Vector2 targetMoveLocation = Vector2.zero;
    [SerializeField] float speed = 5.0f;

    // Blind variables
    [Range(0.1f, 3f)] float blindSearchRadius = 2f;

    float chanceToEndBlindSearch = 0.1f; // Chance to end blind search each frame, can be adjusted for more or less randomness in search duration

    // Path variables
    public PatrolPath PatrolPath { get; set; }
    public float pathReachingRadius = 0.5f;
    private int _pathDestinationNodeIndex;


    void Start()
    {
        _actors = new List<Transform>();
        SwitchStates(SearchState.Path);
    }
    void FixedUpdate()
    {
        switch (_searchState)
        {
            case SearchState.Blind:
                // Get a point within a radius and move towards it until reached then repeat
                if (!IsPointReached(targetMoveLocation))
                    MoveTowardTarget();
                else
                    targetMoveLocation = GetRandomPointInBlindRadius();
                break;
            case SearchState.Noise:
                break;
            case SearchState.Path:
                UpdatePathDestination();
                MoveTowardTarget();
                break;
        }

        // Attempt to end if in blind search
        if (_searchState == SearchState.Blind)
        {
            float rand = Random.value;
            Debug.Log("rand: " + rand + " | Chance:  " + chanceToEndBlindSearch * (Time.deltaTime / 100f));
            if (rand <= chanceToEndBlindSearch * Time.deltaTime)
            {
                EndBlindSearch();
            }
        }
    }
    void SwitchStates(SearchState newState)
    {
        _searchState = newState;
        switch (_searchState)
        {
            case SearchState.Blind:
                break;
            case SearchState.Noise:
                break;
            case SearchState.Path:
                OnPathStateAssigned();
                break;
        }
    }
    void MoveTowardTarget()
    {
        Vector3 direction = (targetMoveLocation - (Vector2)transform.position).normalized;
        moveDir = direction;
        transform.position += (direction * (speed * Time.deltaTime));
    }

    
    // Blind Search state methods
    void EndBlindSearch()
    {
        Debug.Log("Blind search ended, switching to path search");
        _searchState = SearchState.Path;
         SetPathDestinationToClosest();
    }
    Vector3 GetRandomPointInBlindRadius()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0f, blindSearchRadius);
        return (Vector3)(randomDirection * randomDistance) + transform.position;
    }
    bool IsPointReached(Vector3 point)
    {
        return (transform.position - point).magnitude <= 0.1f;
    }

    // Path Search state methods
    // TODO: FIX METHODS THEY ARE CONFUSING
    void OnPathStateAssigned()
    {
        if (IsPathValid())
        {
            SetPathDestinationToClosest();
            targetMoveLocation = GetDestinationOnPath() - transform.position;
        }
    }

    bool IsPathValid()
    {
        return PatrolPath != null && PatrolPath.PathNodes.Count > 0;
    }

    public void SetPathDestinationToClosest()
    {
        if (IsPathValid())
        {
            int closestPathNodeIndex = 0;
            for (int i = 0; i < PatrolPath.PathNodes.Count; i++)
            {
                float distanceToPathNode = PatrolPath.GetDistanceToNode(transform.position, i);
                if (distanceToPathNode < PatrolPath.GetDistanceToNode(transform.position, closestPathNodeIndex))
                {
                    closestPathNodeIndex = i;
                }
            }

            _pathDestinationNodeIndex = closestPathNodeIndex;
        }
        else
            _pathDestinationNodeIndex = 0;
    }

    public Vector3 GetDestinationOnPath()
    {
        if (IsPathValid())
        {
            return PatrolPath.GetPositionOfPathNode(_pathDestinationNodeIndex);
        }

        return transform.position;
    }

    public void UpdatePathDestination(bool inverseOrder = false)
    {
        if (IsPathValid())
        {
            // Check if reached the path destination
            if ((transform.position - GetDestinationOnPath()).magnitude <= pathReachingRadius)
            {
                // Increment or decrement path destination index
                _pathDestinationNodeIndex =
                    inverseOrder ? (_pathDestinationNodeIndex - 1) : (_pathDestinationNodeIndex + 1);

                if (_pathDestinationNodeIndex < 0)
                {
                    _pathDestinationNodeIndex += PatrolPath.PathNodes.Count;
                }

                if (_pathDestinationNodeIndex >= PatrolPath.PathNodes.Count)
                {
                    _pathDestinationNodeIndex -= PatrolPath.PathNodes.Count;
                }

                // Update the target move location
                targetMoveLocation = GetDestinationOnPath();
            }
        }
    }

    // Debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)moveDir);
    }
}