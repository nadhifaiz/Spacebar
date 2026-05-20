using UnityEngine;
using System;

public class JigsawPiece : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private Transform _targetSlot; // Tarik Empty GameObject target ke sini
    [SerializeField] private float _snapTolerance = 0.5f; // Jarak toleransi agar bisa 'nempel'

    private Vector3 _startPos;
    private bool _isLocked = false;

    // Event ini akan berteriak setiap kali satu potongan berhasil terpasang
    public static event Action OnPiecePlaced;

    private void OnEnable()
    {
        // Simpan posisi acak awal agar bisa kembali jika ditaruh di tempat yang salah
        _startPos = transform.position;
        gameObject.transform.localScale = new Vector3(2f, 2f, 1f); // Sesuaikan ukuran potongan jika perlu
    }

    // Dipanggil terus-menerus saat mouse digeser
    public void MovePiece(Vector2 newPosition)
    {
        if (_isLocked) return; // Kalau sudah pas, jangan bisa digeser lagi
        transform.position = newPosition;
    }

    // Dipanggil saat klik dilepas
    public void DropPiece()
    {
        if (_isLocked) return;

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