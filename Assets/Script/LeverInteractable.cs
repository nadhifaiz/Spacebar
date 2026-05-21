using UnityEngine;

/// <summary>
/// Taruh di lever di scene Past.
/// Saat player tarik lever → flag "lever_pulled" aktif
/// → CausalityReactor di Future akan membuka pintu yang menghalangi.
/// </summary>
public class LeverInteractable : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string flagId = "lever_pulled"; // Harus sama dengan CausalityReactor di pintu Future
    [SerializeField] private GameObject interactPrompt;       // UI "Tekan E untuk tarik lever"
    [SerializeField] private Animator leverAnimator;          // Animator lever (opsional)

    private bool _playerNearby = false;
    private bool _leverPulled = false;

    private void Update()
    {
        if (_playerNearby && !_leverPulled)
        {
            if (Input.GetKeyDown(KeyCode.E))
                PullLever();
        }
    }

    private void PullLever()
    {
        _leverPulled = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (leverAnimator != null)
            leverAnimator.SetTrigger("Pull");

        // Set flag → pintu di Future terbuka via CausalityReactor
        PuzzleStateManager.Instance.SetFlag(flagId, true);
        Debug.Log($"Lever ditarik! Flag '{flagId}' aktif. Pintu di Future terbuka.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || _leverPulled) return;
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
