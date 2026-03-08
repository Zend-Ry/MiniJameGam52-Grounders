using UnityEngine;
using UnityEngine.InputSystem;

namespace Unity.FPS.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private InputAction _mMoveAction;
        
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _mMoveAction = InputSystem.actions.FindAction("Player/Move");      
            
            // Favorites Menu
            
            _mMoveAction.Enable();
        }

        public void DisableMoveInput()
        {
            _mMoveAction.Disable();
        }

        // Movement Interactions
        public Vector2 GetMoveInput()
        {
            var input = _mMoveAction.ReadValue<Vector2>();
            Vector2 move = new Vector2(input.x, input.y);

            // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
            move = Vector2.ClampMagnitude(move, 1);

            return move;
        }
    }
}