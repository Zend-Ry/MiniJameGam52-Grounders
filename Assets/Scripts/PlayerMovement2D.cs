using Unity.FPS.Gameplay;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] PlayerInputHandler InputHandler = null;
    
    Vector2 moveDir = Vector2.zero;
    [SerializeField] float speed = 5.0f;

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
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        moveDir = InputHandler.GetMoveInput();
        Vector2 position = transform.position;
        position += moveDir * speed * Time.deltaTime;
        transform.position = position;
    }
}
