using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        // ── Exposed Values ───────────────────────────────────────────
        public Vector2 MoveInput        { get; private set; }
        public bool    InteractPressed  { get; private set; }
        public bool    ToggleGlasses    { get; private set; }

        // ── Generated Input Class ────────────────────────────────────
        private GameInputs _inputs;

        // ────────────────────────────────────────────────────────────
        private void Awake()
        {
            _inputs = new GameInputs();
        }

        private void OnEnable()
        {
            _inputs.GameInput.Enable();

            // Subscribe button callbacks (satu kali fire saat pressed)
            _inputs.GameInput.Interact.performed      += OnInteract;
            _inputs.GameInput.ToggleGlasses.performed += OnToggleGlasses;
        }

        private void OnDisable()
        {
            _inputs.GameInput.Interact.performed      -= OnInteract;
            _inputs.GameInput.ToggleGlasses.performed -= OnToggleGlasses;

            _inputs.GameInput.Disable();
        }

        private void OnDestroy()
        {
            _inputs.Dispose();
        }

        private void Update()
        {
            // Move dibaca tiap frame (Value action)
            MoveInput = _inputs.GameInput.Move.ReadValue<Vector2>();

            // Reset one-frame pulse setelah dibaca
            InteractPressed = false;
            ToggleGlasses   = false;
        }

        // ── Callbacks ────────────────────────────────────────────────
        private void OnInteract(InputAction.CallbackContext ctx)
        {
            InteractPressed = true;
        }

        private void OnToggleGlasses(InputAction.CallbackContext ctx)
        {
            ToggleGlasses = true;
        }
    }
}
