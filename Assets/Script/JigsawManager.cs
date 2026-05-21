using UnityEngine;

public class JigsawManager : MonoBehaviour
{
    [SerializeField] private int _totalPieces; // Isi dengan jumlah total potongan puzzle (misal: 9)
    private int _placedPieces = 0;

    public static event System.Action OnPuzzleCompleted;

    private void OnEnable()
    {
        JigsawPiece.OnPiecePlaced += HandlePiecePlaced; // Subscribe event
    }

    private void OnDisable()
    {
        JigsawPiece.OnPiecePlaced -= HandlePiecePlaced; // Unsubscribe event
    }

    private void HandlePiecePlaced()
    {
        _placedPieces++;

        if (_placedPieces >= _totalPieces)
        {
            Debug.Log("Puzzle Berhasil Diselesaikan!");
            OnPuzzleCompleted?.Invoke();
            foreach (JigsawPiece piece in FindObjectsOfType<JigsawPiece>())
            {
                Destroy(piece.gameObject);
            }
            Time.timeScale=1;
            // Tambahkan logika selanjutnya di sini:
            // - Mainkan animasi menang
            // - Kembali ke Scene Utama
            // - Buka pintu / berikan item
        }
    }
}