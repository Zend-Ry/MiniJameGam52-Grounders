using Unity.FPS.Gameplay;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler), typeof(AudioSource))]
public class PlayerMovement2D : Controller
{
    [SerializeField] public PlayerInputHandler InputHandler = null;
    
    Vector2 moveDir = Vector2.zero;
    [SerializeField] float speed = 5.0f;

    [SerializeField] float noiseChance = 0.0005f;
    [SerializeField] NoiseMaker noiseMaker;
    
    bool audioIsPlaying = false;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] float chosenFootstepSfxFrequency = 0.3f; // This can be adjusted to change how often the footstep sound plays
    float m_FootstepDistanceCounter = 0f; // Counter to track distance for footstep sounds

    private void Awake()
    {
        // do not need to check if there is one on the object 
        // or if the handler was properly assigned as this script requires
        // the game object to have the PlayerInputHandler script.
        if (!InputHandler)
            InputHandler = GetComponent<PlayerInputHandler>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!noiseMaker)
            noiseMaker = GetComponent<NoiseMaker>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdatePlayerPosition();
    }
    void UpdatePlayerPosition() 
    {
        moveDir = InputHandler.GetMoveInput();
        Vector2 position = transform.position;
        position += moveDir * (speed * Time.deltaTime);
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        
        if (m_FootstepDistanceCounter >= 1f * chosenFootstepSfxFrequency)
        {
            m_FootstepDistanceCounter = 0f;
            audioSource.PlayOneShot(footstepClip);
        }
        
        if (moveDir.magnitude > 0)
            m_FootstepDistanceCounter += moveDir.magnitude * Time.deltaTime;
        
        float random = Random.Range(0.0f, 1.0f);
        if (random <= noiseChance)
        {
            if (noiseMaker)
                noiseMaker.CreateNoise();
        }
    }
}
