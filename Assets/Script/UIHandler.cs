using UnityEngine;
using Game.Player;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject notifATCPanel;
    [SerializeField] private GameObject TerrasectCraftButton;
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Teleport Settings")]
    [SerializeField] private Transform playerTransform; 
    [SerializeField] private Transform pastSpawnPoint; // drag GameObject di Inspector

    private Vector3 _savedFuturePosition; // snapshot posisi future sebelum teleport
    private bool _isTeleported = false;  

    private TerrasectManager terrasectManager;
    private bool _puzzleCompleted = false;

    private void Awake()
    {
        terrasectManager = TerrasectManager.Instance;
    }

    private void OnEnable()
    {
        TerrasectManager.onAllPiecesCollected += ShowNotificationATCCollected;
        TerrasectManager.onAllPiecesCollected += EnableCraftButton;
        JigsawManager.OnPuzzleCompleted += HandlePuzzleCompleted;
        TimeSwitchManager.OnForcedReturnToFuture += ForceReturnToFuture;
    }
 
    private void OnDisable()
    {
        TerrasectManager.onAllPiecesCollected -= ShowNotificationATCCollected;
        TerrasectManager.onAllPiecesCollected -= EnableCraftButton;
        JigsawManager.OnPuzzleCompleted -= HandlePuzzleCompleted;
        TimeSwitchManager.OnForcedReturnToFuture -= ForceReturnToFuture;
    }

    private void Update()
    {
        if (inputHandler.TerrasectPressed)
        {
            if (!_puzzleCompleted)
            {
                Debug.Log("Tombol Terrasect ditekan!");
 
                if (terrasectManager.IsAllPiecesCollected() && !terrasectManager.IsTerrasectCrafted())
                {
                    Debug.LogWarning("Tombol T Terdeteksi");
                    OnCraftButtonClicked();
                    terrasectManager.SetTerrasectCrafted(true);
                }
                else if (terrasectManager.IsTerrasectCrafted())
                {
                    Debug.Log("Terrasect crafted tapi puzzle belum selesai.");
                }
            }
            else
            {
                TeleportPlayer();
            }
        }
    }
 
    private void HandlePuzzleCompleted()
    {
        _puzzleCompleted = true;
        puzzlePanel.SetActive(false);
        Destroy(puzzlePanel);
        Debug.Log("Puzzle selesai! Panel ditutup. Tombol T sekarang untuk teleport.");
    }
 
    private void TeleportPlayer()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("playerTransform belum di-assign di Inspector!");
            return;
        }

        if (pastSpawnPoint == null)
        {
            Debug.LogWarning("pastSpawnPoint belum di-assign di Inspector!");
            return;
        }

        TimeSwitchManager.Instance.SwitchTimeline();

        if (!_isTeleported)
        {
            // Simpan posisi Future sebelum teleport
            _savedFuturePosition = playerTransform.position;

            // Teleport ke posisi Past yang ditentukan lewat Inspector
            playerTransform.position = pastSpawnPoint.position;

            _isTeleported = true;
            Debug.Log($"Masuk Past - posisi future disimpan: {_savedFuturePosition}, teleport ke: {pastSpawnPoint.position}");
        }
        else
        {
            // Balik ke posisi Future yang tersimpan
            playerTransform.position = _savedFuturePosition;

            _isTeleported = false;
            Debug.Log($"Balik Future - kembali ke posisi: {_savedFuturePosition}");
        }
    }

    private void ForceReturnToFuture()
    {
        if (!_isTeleported) return;

        // Balik ke posisi Future yang tersimpan (sama seperti teleport manual)
        playerTransform.position = _savedFuturePosition;

        _isTeleported = false;
        Debug.Log($"Dipaksa balik ke Future karena timer habis! Kembali ke: {_savedFuturePosition}");
    }

    public void ShowNotificationATCCollected()
    {
        if (notifATCPanel != null)
        {
            Debug.Log("Showing notification: All Terrasect Pieces Collected!");
            notifATCPanel.SetActive(true);
        }
    }

    public void EnableCraftButton()
    {
        if (TerrasectCraftButton != null)
        {
            Debug.Log("Enabling crafting button.");
            TerrasectCraftButton.SetActive(true);
        }
    }

    public void OnCraftButtonClicked()
    {
        Debug.Log("Crafting button clicked! Starting crafting process...");
        puzzlePanel.SetActive(true);
        terrasectManager.StartCrafting();
    }
}