using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        // ── Exposed Values ───────────────────────────────────────────
        public Vector2 MoveInput { get; private set; }
        public bool InteractPressed { get; private set; }
        public bool ToggleGlasses { get; private set; }
        public bool TerrasectPressed { get; private set; }
        public bool JigsawPressed { get; private set; }

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
            _inputs.GameInput.Interact.performed += OnInteract;
            _inputs.GameInput.ToggleGlasses.performed += OnToggleGlasses;

            // Subscribe tambahan untuk mekanik baru
            _inputs.GameInput.Terrasect.performed += OnTerrasect;
            _inputs.GameInput.Jigsaw.performed += OnJigsaw;
        }

        private void OnDisable()
        {
            _inputs.GameInput.Interact.performed -= OnInteract;
            _inputs.GameInput.ToggleGlasses.performed -= OnToggleGlasses;

            // Unsubscribe tambahan agar tidak bocor memori
            _inputs.GameInput.Terrasect.performed -= OnTerrasect;
            _inputs.GameInput.Jigsaw.performed -= OnJigsaw;

            _inputs.GameInput.Disable();
        }

        private void OnDestroy()
        {
            _inputs.Dispose();
        }

        private void LateUpdate()
        {
            // Move dibaca tiap frame (Value action)
            MoveInput = _inputs.GameInput.Move.ReadValue<Vector2>();

            // Reset one-frame pulse setelah dibaca
            InteractPressed = false;
            ToggleGlasses = false;

            // Reset input baru setelah dibaca oleh script lain
            TerrasectPressed = false;
            JigsawPressed = false;
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

        private void OnTerrasect(InputAction.CallbackContext ctx)
        {
            TerrasectPressed = true;
        }

        private void OnJigsaw(InputAction.CallbackContext ctx)
        {
            JigsawPressed = true;
        }
    }
}