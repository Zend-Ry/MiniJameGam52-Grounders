using UnityEngine;
using UnityEngine.InputSystem;

namespace Unity.FPS.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Tooltip("Sensitivity multiplier for moving the camera around")]
        public float LookSensitivity = 1f;

        [Tooltip("Limit to consider an input when using a trigger on a controller")]
        public float TriggerAxisThreshold = 0.4f;

        public bool InvertYAxis = false;
        public bool InvertXAxis = false;

        
        //PlayerCharacterController _mPlayerCharacterController;

        private InputAction _mMoveAction;
        private InputAction _mJumpAction;
        private InputAction _mGrounders;

        void Start()
        {
            //_mPlayerCharacterController = GetComponent<PlayerCharacterController>();
            //DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController, PlayerInputHandler>(
            //    _mPlayerCharacterController, this, gameObject);
            //_mGameFlowManager = FindFirstObjectByType<GameFlowManager>();
            //DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, PlayerInputHandler>(_mGameFlowManager, this);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            
            _mMoveAction = InputSystem.actions.FindAction("Player/Move");    
            _mJumpAction = InputSystem.actions.FindAction("Player/Jump");    
            _mGrounders = InputSystem.actions.FindAction("Player/RightHand");
            
            // Favorites Menu
            
            _mMoveAction.Enable();
            _mJumpAction.Enable();
            _mGrounders.Enable();
        }

        void LateUpdate()
        {
            
        }
        
        public bool CanProcessInput()
        {
            //return Cursor.lockState == CursorLockMode.Locked && !_mGameFlowManager.GameIsEnding;
            return false;
        }

        
        // Movement Interactions
        public Vector3 GetMoveInput()
        {
            if (CanProcessInput())
            {
                var input = _mMoveAction.ReadValue<Vector2>();
                Vector3 move = new Vector3(input.x, 0f, input.y);

                // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
                move = Vector3.ClampMagnitude(move, 1);

                return move;
            }

            return Vector3.zero;
        }
        
        public bool GetJumpInputDown()
        {
            if (CanProcessInput())
            {
                return _mJumpAction.WasPressedThisFrame();
            }

            return false;
        }
        public bool GetJumpInputHeld()
        {
            if (CanProcessInput())
            {
                return _mJumpAction.IsPressed();
            }

            return false;
        }
        public bool GetJumpInputReleased()
        {
            if (CanProcessInput())
            {
                return _mJumpAction.WasReleasedThisFrame();
            }

            return false;
        }

        public bool GetGroundersInput()
        {
            if (CanProcessInput())
            {
                return _mGrounders.IsPressed();
            }

            return false;
        }
    }
}