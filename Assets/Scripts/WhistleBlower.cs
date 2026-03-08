using UnityEngine;

public class WhistleBlower : MonoBehaviour
{
    // the times will be measured in seconds
    [SerializeField] float minimumWaitTime = 2.0f;
    [SerializeField] float maximumWaitTime = 10.0f;
    float waitTime = 0f;
    float timer = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetTimer();
        // randomize the whistle blowers look direction for the first time
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime) 
        {
            BlowWhistle();
        }
    }

    void ResetTimer() 
    {
        waitTime = Random.Range(minimumWaitTime, maximumWaitTime);
        timer = 0.0f;
    }

    void BlowWhistle() 
    {
        SignalManager.WhistleBlown.Emit();
        ResetTimer();
    }
}
