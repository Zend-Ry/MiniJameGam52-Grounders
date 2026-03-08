using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ActorBOIDController : Controller
{
    [SerializeField] float fleeRadius = 3f;
    [SerializeField] Vector2 safeAreaCenter = Vector2.zero;
    [SerializeField] float safeAreaRadius = 10f;
    [SerializeField] float speed = 2.8f;
    [SerializeField] GameObject itActor;

    public bool isActive = true;

    // Noise variables
    [SerializeField] float noiseChance = 0.0005f;
    [SerializeField] NoiseMaker noiseMaker;

    List<GameObject> safeAreas = new List<GameObject>();
    
    public Vector2 moveDir = Vector2.zero;
    
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] float chosenFootstepSfxFrequency = 0.3f; // This can be adjusted to change how often the footstep sound plays
    float m_FootstepDistanceCounter = 0f; // Counter to track distance for footstep sounds
    
    float timeFleeing = 0f;

    private void Start()
    {
        // Find all safe areas in the scene (tagged as "SafeArea")
        GameObject[] safeAreaObjects = GameObject.FindGameObjectsWithTag("SafeArea");
        foreach (var safeArea in safeAreaObjects)
        {
            safeAreas.Add(safeArea);
        }
    }

    void Update()
    {
        if (!isActive)
            return;
        
        if (itActor != null)
        {
            Vector2 directionToItActor = (Vector2)(transform.position - itActor.transform.position);
            if (directionToItActor.magnitude <= fleeRadius)
            {
                // Flee from IT Actor
                moveDir = directionToItActor.normalized;
                noiseChance = 0.0005f + (timeFleeing * 0.01f); // Increase noise chance when fleeing over time
                timeFleeing += Time.deltaTime;
            }
            else
            {
                //FindClosestSafeArea();
                
                // Move towards the safe area if outside
                Vector2 directionToSafeArea = FindClosestSafeArea() - (Vector2)transform.position;
                if (directionToSafeArea.magnitude > safeAreaRadius)
                {
                    moveDir = directionToSafeArea.normalized;
                }
                else
                {
                    moveDir = Vector2.zero; // Stay idle in the safe area
                }
                
                noiseChance = 0.0005f;
            }
        }
    
        // Apply movement
        transform.position += (Vector3)(moveDir * (speed * Time.deltaTime));
        
        // Play footstep sound based on distance traveled
        if (m_FootstepDistanceCounter >= 1f * chosenFootstepSfxFrequency)
        {
            m_FootstepDistanceCounter = 0f;
            audioSource.PlayOneShot(footstepClip);
        }
        
        if (moveDir.magnitude > 0)
            m_FootstepDistanceCounter += moveDir.magnitude * Time.deltaTime;

        
        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        if (random <= noiseChance)
        {
            if (noiseMaker)
                noiseMaker.CreateNoise();
        }
    }

    Vector2 FindClosestSafeArea() 
    {
        float currentMagnitude = float.MaxValue;
        float itMagnitude = (itActor.transform.position - transform.position).magnitude;
        foreach (var area in safeAreas) 
        {
            float areaMag = (area.transform.position - transform.position).magnitude;
            if (areaMag < currentMagnitude /*&& areaMag > itMagnitude * 2f*/)
            {
                safeAreaCenter = area.transform.position;
                currentMagnitude = areaMag;
            }
        }
        return safeAreaCenter;
    }

    void OnDrawGizmosSelected()
    {
        // Visualize flee radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeRadius);
    
        // Visualize safe area
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(safeAreaCenter, safeAreaRadius);
    }
}
