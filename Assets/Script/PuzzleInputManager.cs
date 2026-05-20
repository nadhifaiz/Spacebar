using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleInputManager : MonoBehaviour
{
    private Camera _mainCamera;
    private JigsawPiece _heldPiece;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    // Default Disable JigsawPiece agar tidak bisa langsung diambil sebelum semua Terrasect Piece terkumpul
    private void Update()
    {
        // 1. GRAB: Deteksi klik awal (Tekan)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryGrabPiece();
        }

        // 2. DRAG: Kursor ditahan dan digeser (Hold)
        if (Mouse.current.leftButton.isPressed && _heldPiece != null)
        {
            // Ubah posisi layar ke koordinat dunia 2D
            Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _heldPiece.MovePiece(mousePos);
        }

        // 3. DROP: Deteksi klik dilepas (Release)
        if (Mouse.current.leftButton.wasReleasedThisFrame && _heldPiece != null)
        {
            _heldPiece.DropPiece();
            _heldPiece = null; // Kosongkan tangan
        }
    }

    private void TryGrabPiece()
    {
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Tembak laser 2D ke posisi kursor untuk mengecek benda apa yang disentuh
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            // Cek apakah benda yang disentuh punya script JigsawPiece
            _heldPiece = hit.collider.GetComponent<JigsawPiece>();
        }
    }
}