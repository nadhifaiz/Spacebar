using UnityEngine;
using TMPro;

/// <summary>
/// UI countdown saat player berada di Past.
/// Taruh di Canvas. Assign text dan panel-nya di Inspector.
/// </summary>
public class PastCountdownUI : MonoBehaviour
{
    [SerializeField] private GameObject countdownPanel; // Panel yang muncul saat di Past
    [SerializeField] private TextMeshProUGUI timerText; // Text "29.5"
    [SerializeField] private TextMeshProUGUI warningText; // Text peringatan saat hampir habis
    [SerializeField] private float warningThreshold = 10f; // Mulai warning di detik ke-10

    private void OnEnable()
    {
        TimeSwitchManager.OnTimerTick += UpdateTimer;
        TimeSwitchManager.OnForcedReturnToFuture += HidePanel;

        // Subscribe event masuk/keluar Past dari UnityEvent via code
        // (alternatif: assign lewat Inspector di TimeSwitchManager)
    }

    private void OnDisable()
    {
        TimeSwitchManager.OnTimerTick -= UpdateTimer;
        TimeSwitchManager.OnForcedReturnToFuture -= HidePanel;
    }

    private void Start()
    {
        if (countdownPanel != null)
            countdownPanel.SetActive(false);
    }

    private void UpdateTimer(float timeRemaining)
    {
        // Tampilkan panel jika belum aktif
        if (countdownPanel != null && !countdownPanel.activeSelf)
            countdownPanel.SetActive(true);

        if (timerText != null)
            timerText.text = timeRemaining.ToString("F1") + "s";

        // Tampilkan warning jika waktu hampir habis
        if (warningText != null)
            warningText.gameObject.SetActive(timeRemaining <= warningThreshold);
    }

    // Dipanggil saat balik ke Future (manual atau paksa)
    public void HidePanel()
    {
        if (countdownPanel != null)
            countdownPanel.SetActive(false);
    }

    // Dipanggil saat masuk Past — bisa di-assign di onEnterPast UnityEvent Inspector
    public void ShowPanel()
    {
        if (countdownPanel != null)
            countdownPanel.SetActive(true);
    }
}
