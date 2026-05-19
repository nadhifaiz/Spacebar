using UnityEngine;

namespace Game.Player
{
    /// <summary>
    /// Handles linear top-down movement via Rigidbody2D.
    /// Reads from PlayerInputHandler — no direct Input calls here.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;

        // ── References ───────────────────────────────────────────────
        private Rigidbody2D        _rb;
        private PlayerInputHandler _input;

        // ── State (dibaca PlayerAnimator) ────────────────────────────
        public Vector2 LastMoveDirection { get; private set; } = Vector2.down;
        public bool    IsMoving          { get; private set; }

        // ────────────────────────────────────────────────────────────
        private void Awake()
        {
            _rb    = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInputHandler>();

            // Typical top-down Rigidbody2D setup
            _rb.gravityScale   = 0f;
            _rb.freezeRotation = true;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector2 dir = _input.MoveInput;

            // Normalize diagonal movement so speed is consistent
            if (dir.sqrMagnitude > 1f)
                dir.Normalize();

            _rb.linearVelocity = dir * _moveSpeed;

            IsMoving = dir.sqrMagnitude > 0.01f;

            // Simpan arah terakhir untuk animasi idle
            if (IsMoving)
                LastMoveDirection = dir;
        }
    }
}
