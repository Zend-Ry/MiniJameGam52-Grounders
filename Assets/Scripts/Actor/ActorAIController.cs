using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum SearchState
{
    Idle,
    Blind,
    Noise,
    NotIt
}

[RequireComponent(typeof(ActorAnimationController), typeof(AudioSource))]
public class ActorController : Controller
{
    private List<Transform> _actors;
    SearchState _searchState = SearchState.Idle;

    Vector2 moveDir = Vector2.zero;
    public Vector2 GetMoveDir() => moveDir;
    public Vector2 targetMoveLocation = Vector2.zero;
    [SerializeField] float speed = 5.0f;


    // Blind variables
    [SerializeField] Vector2 blindSearchMinMax = new Vector2(1f, 3f); // Min and max radius for blind search

    // Idle variables
    float idleMaxDuration = 2.4f;
    private float currentIdleTimeout = 0f;
    float idleTimer = 0f;

    // Noise variables
    [SerializeField] float noiseChance = 0.0005f;
    [SerializeField] NoiseMaker noiseMaker;
    
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] float chosenFootstepSfxFrequency = 0.3f; // This can be adjusted to change how often the footstep sound plays
    float m_FootstepDistanceCounter = 0f; // Counter to track distance for footstep sounds

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
        
        // Play footstep sound based on distance traveled
        if (m_FootstepDistanceCounter >= 1f * chosenFootstepSfxFrequency)
        {
            m_FootstepDistanceCounter = 0f;
            audioSource.PlayOneShot(footstepClip);
        }
        
        if (moveDir.magnitude > 0)
            m_FootstepDistanceCounter += moveDir.magnitude * Time.deltaTime;
        
        if (random <= noiseChance) 
        {
            if (noiseMaker)
                noiseMaker.CreateNoise();
        }
    }

    
    // Blind Search state methods
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

    // Debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)moveDir);
    }
}