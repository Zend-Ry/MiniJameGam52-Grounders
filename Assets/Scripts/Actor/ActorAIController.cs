using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum SearchState
{
    Idle,
    Blind,
    Noise
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
    [SerializeField] Vector2 blindSearchMinMax = new Vector2(1f, 3f); // Min and max radius for blind search
    float chanceToEndBlindSearch = 0.1f; // Chance to end blind search each frame, can be adjusted for more or less randomness in search duration

    // Idle variables
    float idleMaxDuration = 2.4f;
    private float currentIdleTimeout = 0f;
    float idleTimer = 0f;

    // Noise variables
    [SerializeField] float noiseChance = 0.0005f;
    [SerializeField] NoiseMaker noiseMaker;

    void Start()
    {
        _actors = new List<Transform>();
        if (!noiseMaker)
            noiseMaker = GetComponent<NoiseMaker>();
        SwitchStates(SearchState.Blind);
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
                    _searchState = SearchState.Idle;
                    currentIdleTimeout = Random.Range(0f, idleMaxDuration); // Randomize idle duration after each blind search
                break;
            case SearchState.Noise:
                break;
        }
        
        if (_searchState == SearchState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleMaxDuration || idleTimer > currentIdleTimeout)
            {
                targetMoveLocation = GetRandomPointInBlindRadius();
                SwitchStates(SearchState.Blind);
                idleTimer = 0f;
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
        }
    }
    void MoveTowardTarget()
    {
        Vector3 direction = (targetMoveLocation - (Vector2)transform.position).normalized;
        moveDir = direction;
        transform.position += (direction * (speed * Time.deltaTime));
        float random = Random.Range(0.0f, 1.0f);
        if (random <= noiseChance) 
        {
            if (noiseMaker)
                noiseMaker.CreateNoise();
        }
    }

    
    // Blind Search state methods
    void EndBlindSearch()
    {
        Debug.Log("Blind search ended, switching to path search");
        //_searchState = SearchState.Path;
    }
    Vector3 GetRandomPointInBlindRadius()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(blindSearchMinMax.x, blindSearchMinMax.y);
        return (Vector3)(randomDirection * randomDistance) + transform.position;
    }
    bool IsPointReached(Vector3 point)
    {
        return (transform.position - point).magnitude <= 0.1f;
    }
    
    // Noise Search state methods
    void GetNoiseWithinRadius()
    {
        
    }

    // Debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)moveDir);
    }
}