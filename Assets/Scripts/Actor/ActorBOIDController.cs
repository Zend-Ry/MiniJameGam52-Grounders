using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorBOIDController : Controller
{
    [SerializeField] float fleeRadius = 3f;
    [SerializeField] Vector2 safeAreaCenter = Vector2.zero;
    [SerializeField] float safeAreaRadius = 10f;
    [SerializeField] float speed = 2.8f;
    [SerializeField] GameObject itActor;

    // Noise variables
    [SerializeField] float noiseChance = 0.0005f;
    [SerializeField] NoiseMaker noiseMaker;

    List<GameObject> safeAreas = new List<GameObject>();
    
    public Vector2 moveDir = Vector2.zero;
    
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
        if (itActor != null)
        {
            Vector2 directionToItActor = (Vector2)(transform.position - itActor.transform.position);
            if (directionToItActor.magnitude <= fleeRadius)
            {
                // Flee from IT Actor
                moveDir = directionToItActor.normalized;
                noiseChance = 0.0005f + (timeFleeing * 0.01f); // Increase noise chance when fleeing over time
                timeFleeing += Time.deltaTime;
                Debug.Log("Chance to make noise while fleeing: " + noiseChance);
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
            if (areaMag < currentMagnitude/* && areaMag > itMagnitude*/) 
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
