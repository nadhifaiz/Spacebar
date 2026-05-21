using UnityEngine;
using System;

public class JigsawPiece : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private float _snapTolerance = 0.5f; // Jarak toleransi agar bisa 'nempel'

    // Perhatikan: _targetSlot sekarang murni private, tidak pakai [SerializeField]
    private Transform _targetSlot;

    private Vector3 _startPos;
    private bool _isLocked = false;

    // Event ini akan berteriak setiap kali satu potongan berhasil terpasang
    public static event Action OnPiecePlaced;

    // ── INI BAGIAN TERPENTINGNYA ──
    // Method ini akan dipanggil otomatis oleh MultiSpawner setelah prefab dilahirkan
    public void SetTarget(Transform target)
    {
        _targetSlot = target;
    }
    // ─────────────────────────────

    private void OnEnable()
    {
        // Simpan posisi acak awal agar bisa kembali jika ditaruh di tempat yang salah
        _startPos = transform.position;
        gameObject.transform.localScale = new Vector3(2f, 2f, 1f);
    }

    // Dipanggil terus-menerus saat mouse digeser
    public void MovePiece(Vector2 newPosition)
    {
        if (_isLocked) return;
        transform.position = newPosition;
    }

    // Dipanggil saat klik dilepas
    public void DropPiece()
    {
        // Cegah error jika target belum terisi
        if (_isLocked || _targetSlot == null) return;

        // Hitung jarak antara posisi sekarang dengan posisi target
        float distance = Vector2.Distance(transform.position, _targetSlot.position);

        if (distance <= _snapTolerance)
        {
            // Posisi benar! Snap ke titik pas dan kunci.
            transform.position = _targetSlot.position;
            _isLocked = true;

            // Panggil event agar JigsawManager tahu
            OnPiecePlaced?.Invoke();
        }
        else
        {
            // Posisi salah! Kembalikan ke tempat semula.
            transform.position = _startPos;
        }
    }
}