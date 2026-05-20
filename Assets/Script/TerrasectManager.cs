using System;
using UnityEngine;

public class TerrasectManager : MonoBehaviour
{
    [Header("Collection Settings")]
    [SerializeField] private int terrasectPieceNeeded;
    [SerializeField] private int terrasectPieceCollected;

    [Header("Crafting/Puzzle Settings")]
    [SerializeField] private PuzzleInputManager puzzleInputManager; // Masukkan script PuzzleInputManager di sini
    [SerializeField] private Vector2 randomAreaMin; // Batas kiri-bawah area acak (X, Y)
    [SerializeField] private Vector2 randomAreaMax; // Batas kanan-atas area acak (X, Y)

    private GameObject[] TerrasectPiece;
    private bool isTerrasectCrafted = false;
    public static event Action onAllPiecesCollected;

    private void Start()
    {
        TerrasectPiece = GameObject.FindGameObjectsWithTag("TerrasectPiece");
        terrasectPieceCollected = 0;

        // Pastikan input puzzle mati saat game baru mulai agar kepingan tidak bisa ditarik
        if (puzzleInputManager != null)
        {
            puzzleInputManager.enabled = false;
        }
    }

    public void CollectTerrasectPiece()
    {
        terrasectPieceCollected++;
        Debug.Log($"Terrasect Piece Collected: {terrasectPieceCollected}/{terrasectPieceNeeded}");

        if (terrasectPieceCollected >= terrasectPieceNeeded)
        {
            Debug.Log("All Terrasect Pieces Collected! You can now start crafting the controller.");
            onAllPiecesCollected?.Invoke();
        }
    }

    public void StartCrafting()
    {
        // 1. Logic Aktifkan PuzzleInputManager untuk memungkinkan interaksi
        if (puzzleInputManager != null)
        {
            puzzleInputManager.enabled = true; // Menyalakan script agar player bisa klik & drag
        }
        else
        {
            Debug.LogWarning("Script PuzzleInputManager belum di-drag ke Inspector TerrasectManager!");
        }

        // 2. Logic Acak Posisi & Aktifkan Object
        for (int i = 0; i < TerrasectPiece.Length; i++)
        {
            // Bikin posisi X dan Y acak berdasarkan batasan yang kamu set di Inspector
            float randomX = UnityEngine.Random.Range(randomAreaMin.x, randomAreaMax.x);
            float randomY = UnityEngine.Random.Range(randomAreaMin.y, randomAreaMax.y);

            // Terapkan posisi acaknya (Z tetap 0 karena ini 2D)
            TerrasectPiece[i].transform.position = new Vector3(randomX, randomY, 0f);

            // Aktifkan piece-nya
            TerrasectPiece[i].SetActive(true);
            TerrasectPiece[i].GetComponent<JigsawPiece>().enabled = true; // Pastikan JigsawPiece aktif agar bisa di-drag
        }

        Time.timeScale = 0f; // Pause game saat crafting dimulai, bisa diubah sesuai kebutuhan (misal: biar player tetap bisa bergerak tapi tidak bisa keluar area crafting)
    }

    public bool IsAllPiecesCollected()
    {
        return terrasectPieceCollected >= terrasectPieceNeeded;
    }

    public bool IsTerrasectCrafted()
    {
        return isTerrasectCrafted;
    }

    public void SetTerrasectCrafted(bool crafted)
    {
        isTerrasectCrafted = crafted;
    }
}