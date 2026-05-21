using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mengatur perpindahan timeline Past <-> Future.
/// Past punya countdown 30 detik, kalau habis otomatis balik ke Future.
/// </summary>
public class TimeSwitchManager : MonoBehaviour
{
    public static TimeSwitchManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float pastTimeLimit = 30f; // Batas waktu di Past

    [Header("Events")]
    public UnityEvent onEnterPast;
    public UnityEvent onEnterFuture;
    public UnityEvent onPastTimerExpired; // Untuk UI countdown

    public bool IsInPast { get; private set; } = false;
    public float PastTimeRemaining { get; private set; }

    // Event untuk UI
    public static event System.Action<float> OnTimerTick;   // kirim sisa waktu tiap frame
    public static event System.Action OnForcedReturnToFuture; // saat timer habis

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Update()
    {
        if (!IsInPast) return;

        PastTimeRemaining -= Time.deltaTime;
        OnTimerTick?.Invoke(PastTimeRemaining);

        if (PastTimeRemaining <= 0f)
        {
            PastTimeRemaining = 0f;
            ReturnToFuture(forced: true);
        }
    }

    /// <summary>Dipanggil oleh input / tombol untuk switch timeline.</summary>
    public void SwitchTimeline()
    {
        if (IsInPast)
            ReturnToFuture(forced: false);
        else
            EnterPast();
    }

    private void EnterPast()
    {
        IsInPast = true;
        PastTimeRemaining = pastTimeLimit;
        onEnterPast?.Invoke();
        Debug.Log("Masuk ke Past! Countdown dimulai.");
    }

    private void ReturnToFuture(bool forced)
    {
        IsInPast = false;
        onEnterFuture?.Invoke();

        if (forced)
        {
            onPastTimerExpired?.Invoke();
            OnForcedReturnToFuture?.Invoke();
            Debug.Log("Waktu di Past habis! Balik ke Future paksa.");
        }
        else
        {
            Debug.Log("Balik ke Future secara manual.");
        }
    }
}
