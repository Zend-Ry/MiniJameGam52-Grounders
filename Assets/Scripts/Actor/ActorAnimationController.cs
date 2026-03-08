using System;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorAnimationController : MonoBehaviour
{
    [SerializeField] List<Sprite> upwardsSprites = new List<Sprite>(3);
    [SerializeField] List<Sprite> downwardsSprites = new List<Sprite>(3);
    [SerializeField] List<Sprite> sidewaySprites = new List<Sprite>(2);
    [SerializeField] List<Sprite> deathSprites = new List<Sprite>(2);

    Vector2 _moveDir = Vector2.zero;
    [SerializeField] SpriteRenderer spriteRenderer;

    private float _animationTimer = 0f;
    private int _animationIndex = 0;

    private Controller controller;
    private PlayerMovement2D playerInput;
    private ActorBOIDController boidController;
    private ActorController actorAiController;
    bool death;
    bool playerEgoDeath;
    float deathTimer;
    float timer;
    // private MovementHandler movementHandler; // This would be for AI actor movement but unused currently

    private void Start()
    {
        spriteRenderer.transform.localScale = new Vector2(16, 16); // Hacky adjust scale for pixel art
        
            if (controller == null)
                controller = GetComponent<PlayerMovement2D>() ?? 
                             (Controller)GetComponent<ActorBOIDController>() ?? 
                             GetComponent<ActorController>();
            
            playerInput = controller as PlayerMovement2D;
            boidController = controller as ActorBOIDController;
            actorAiController = controller as ActorController;
    }

    private void LateUpdate()
    {
        Animate();
        if (death == true)
        {
            timer += Time.deltaTime;
            if (timer > deathTimer)
                gameObject.SetActive(false);
            return;
        }
        else if (playerEgoDeath == true)
        {
            timer += Time.deltaTime;
            if (timer > deathTimer)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene(0);
            }
            return;
        }

        // Get Actor Movement
        if (playerInput)
            _moveDir = playerInput.InputHandler.GetMoveInput();
        else if (actorAiController)
            _moveDir = actorAiController.GetMoveDir();
        else if (boidController)
            _moveDir = boidController.moveDir;
        else
            _moveDir = Vector2.zero; // Default to no movement if no input or AI controller found

        // Check for no movement and reset to idle sprite
            if (_moveDir == Vector2.zero)
            {
                // TODO: Change to last facing direction idle sprite
                spriteRenderer.sprite = sidewaySprites[0]; // Assuming the first sprite in the upwards list is the idle sprite
                SetAnimTimer(0f);
                return;
            }

            switch (_moveDir)
            {   // Up and Down sprites loop frames 2 and 3 due to frame 1 being static
                case var v when v.y > 0:
                    spriteRenderer.sprite = upwardsSprites[_animationIndex + 1];
                    break;
                case var v when v.y < 0:
                    spriteRenderer.sprite = downwardsSprites[_animationIndex + 1];
                    break;
                case var v when v.x > 0:
                    _animationIndex = (_animationIndex + 1) % sidewaySprites.Count;
                    spriteRenderer.sprite = sidewaySprites[_animationIndex];
                    spriteRenderer.flipX = false;
                    break;
                case var v when v.x < 0:
                    _animationIndex = (_animationIndex + 1) % sidewaySprites.Count;
                    spriteRenderer.sprite = sidewaySprites[_animationIndex];
                    spriteRenderer.flipX = true;
                    break;
            }
    }
    
    private void Animate()
    {
        _animationTimer += Time.deltaTime;
        if (_animationTimer >= 0.23f)
        {
            _animationTimer = 0f;
            _animationIndex = (_animationIndex + 1) % 2;
        }
    }

    public void ActorFrozen()
    {
        death = true;
        spriteRenderer.sprite = deathSprites[_animationIndex + 1];

    }

    public void PlayerEgoDeath()
    {
        playerEgoDeath = true;
        spriteRenderer.sprite = deathSprites[_animationIndex + 1];
    }

    private void SetAnimTimer(float time)
    {
        _animationTimer = time;
    }
}
