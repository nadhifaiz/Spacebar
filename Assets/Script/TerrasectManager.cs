using System;
using System.Collections.Generic; // Wajib ditambah untuk pakai List
using UnityEngine;

public class TerrasectManager : MonoBehaviour
{
    [Header("Collection Settings")]
    [SerializeField] private int terrasectPieceNeeded;
    [SerializeField] private int terrasectPieceCollected;

    [Header("Crafting/Puzzle Settings")]
    [SerializeField] private PuzzleInputManager puzzleInputManager;
    [SerializeField] private Vector2 randomAreaMin;
    [SerializeField] private Vector2 randomAreaMax;

    // UBAH: Dari Array (GameObject[]) menjadi List agar bisa diisi satuan
    private List<GameObject> TerrasectPieceList = new List<GameObject>();

    private bool isTerrasectCrafted = false;
    public static event Action onAllPiecesCollected;
    public static TerrasectManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // HAPUS GameObject.FindGameObjectsWithTag dari sini
        terrasectPieceCollected = 0;

        if (puzzleInputManager != null)
        {
            puzzleInputManager.enabled = false;
        }
    }

    // ─── FUNGSI BARU: Untuk menerima pendaftaran dari kepingan puzzle ───
    public void RegisterTerrasectPiece(GameObject piece)
    {
        if (!TerrasectPieceList.Contains(piece))
        {
            TerrasectPieceList.Add(piece);
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
        if (puzzleInputManager != null)
        {
            Debug.Log("Crafting started! Enabling PuzzleInputManager for player interaction.");
            puzzleInputManager.enabled = true;
        }
        else
        {
            Debug.LogWarning("Script PuzzleInputManager belum di-drag ke Inspector TerrasectManager!");
        }

        // ─── PERBAIKAN: Ambil posisi kamera utama saat ini ───
        Vector3 cameraPos = Camera.main.transform.position;

        for (int i = 0; i < TerrasectPieceList.Count; i++)
        {
            // Ambil nilai acak seperti biasa (ini berfungsi sebagai jarak offset)
            float randomXOffset = UnityEngine.Random.Range(randomAreaMin.x, randomAreaMax.x);
            float randomYOffset = UnityEngine.Random.Range(randomAreaMin.y, randomAreaMax.y);

            // ─── PERBAIKAN: Jumlahkan posisi kamera dengan offset acak ───
            // Z kita set ke 0 agar kepingan tetap berada di layer 2D dunia, 
            // sementara X dan Y mengikuti kemana pun kamera berada.
            Vector3 newSpawnPos = new Vector3(cameraPos.x + randomXOffset, cameraPos.y + randomYOffset, 0f);

            TerrasectPieceList[i].transform.position = newSpawnPos;

            TerrasectPieceList[i].SetActive(true);
            TerrasectPieceList[i].GetComponent<JigsawPiece>().enabled = true;
        }

        Time.timeScale = 0f;
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