using System;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;

public class ActorAnimationController : MonoBehaviour
{
    [SerializeField] List<Sprite> upwardsSprites = new List<Sprite>(3);
    [SerializeField] List<Sprite> downwardsSprites = new List<Sprite>(3);
    [SerializeField] List<Sprite> sidewaySprites = new List<Sprite>(2);

    Vector2 _moveDir = Vector2.zero;
    [SerializeField] SpriteRenderer spriteRenderer;

    private float _animationTimer = 0f;
    private int _animationIndex = 0;

    PlayerInputHandler playerInput;
    // private MovementHandler movementHandler; // This would be for AI actor movement but unused currently

    private void Start()
    {
        spriteRenderer.transform.localScale = new Vector2(16, 16); // Hacky adjust scale for pixel art
        
        if (playerInput == null)
            playerInput = TryGetComponent<PlayerInputHandler>(out playerInput) ? playerInput : null;
    }

    private void LateUpdate()
    {
        Animate();
        
        // Get Actor Movement
        if (playerInput)
        {
            _moveDir = playerInput.GetMoveInput();
            
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
    }
    
    private void Animate()
    {
        _animationTimer += Time.deltaTime;
        if (_animationTimer >= 0.23f)
        {
            _animationTimer = 0f;
            _animationIndex = (_animationIndex + 1) % 2;
            // This is where you would cycle through the sprites for animation based on the direction
            // For example, if moving upwards, you would cycle through upwardsSprites list
        }
    }
    
    private void SetAnimTimer(float time)
    {
        _animationTimer = time;
    }
}
