using UnityEngine;

/// <summary>
/// Taruh di brankas di scene Past.
/// Saat player berinteraksi, core tersimpan → flag "core_saved" aktif
/// → CausalityReactor di Future akan membuat core future jadi tidak rusak.
/// </summary>
public class CoreVault : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string flagId = "core_saved"; // Harus sama dengan CausalityReactor di Future
    [SerializeField] private GameObject interactPrompt;    // UI "Tekan E untuk simpan core"

    private bool _playerNearby = false;
    private bool _coreSaved = false;

    private void Update()
    {
        // Deteksi input interact saat player dekat dan core belum disimpan
        if (_playerNearby && !_coreSaved)
        {
            if (Input.GetKeyDown(KeyCode.E))
                SaveCore();
        }
    }

    private void SaveCore()
    {
        _coreSaved = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        // Set flag → CausalityReactor di Future akan bereaksi
        PuzzleStateManager.Instance.SetFlag(flagId, true);
        Debug.Log($"Core berhasil disimpan di brankas! Flag '{flagId}' aktif.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || _coreSaved) return;
        _playerNearby = true;
        if (interactPrompt != null) interactPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _playerNearby = false;
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }
}
