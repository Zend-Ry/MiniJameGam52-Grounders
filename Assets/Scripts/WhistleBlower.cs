using Unity.FPS.Gameplay;
using UnityEngine;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(AudioSource))]
public class WhistleBlower : MonoBehaviour
{
    // the times will be measured in seconds
    [SerializeField] float minimumWaitTime = 2.0f;
    [SerializeField] float maximumWaitTime = 10.0f;
    [SerializeField] float NoiseHeardRadius = 5.0f;
    [SerializeField] ActorController actorController;
    [SerializeField] AudioSource source;

    float waitTime = 0f;

    float timer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetTimer();
        // randomize the whistle blowers look direction for the first time
        SignalManager.MakeNoise.AddListener(MoveToSound);
        if (!actorController)
        {
            actorController = GetComponent<ActorController>();
        }

        if (!source)
            source = GetComponent<AudioSource>();
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

    void MoveToSound(Vector3 noisePosition)
    {
        if (!actorController)
            return;

        if ((actorController.transform.position - noisePosition).magnitude > NoiseHeardRadius)
            return;

        actorController.targetMoveLocation = noisePosition;
    }

    void ResetTimer()
    {
        waitTime = Random.Range(minimumWaitTime, maximumWaitTime);
        timer = 0.0f;
    }

    void BlowWhistle()
    {
        SignalManager.WhistleBlown.Emit();
        source.Play();
        ResetTimer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // shit ass way of doing this but im tired so idgaf
        var animController = collision.gameObject.GetComponent<ActorAnimationController>();

        if (animController == null)
            return;

        if (collision.gameObject.tag == "Actor")
        {
            animController.ActorFrozen();
            gameObject.GetComponent<ActorBOIDController>().isActive = false; // Disable AI
        }
        else if (collision.gameObject.tag == "Player")
        {
            animController.PlayerEgoDeath();
        }
    }
}