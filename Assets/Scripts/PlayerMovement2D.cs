using Unity.FPS.Gameplay;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerMovement2D : Controller
{
    [SerializeField] public PlayerInputHandler InputHandler = null;
    
    Vector2 moveDir = Vector2.zero;
    [SerializeField] float speed = 5.0f;

    [SerializeField] float noiseChance = 0.0005f;
    [SerializeField] NoiseMaker noiseMaker;

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
        position += moveDir * speed * Time.deltaTime;
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        float random = Random.Range(0.0f, 1.0f);
        if (random <= noiseChance)
        {
            if (noiseMaker)
                noiseMaker.CreateNoise();
        }
    }
}
