using UnityEngine;

public class CausalityReactor : MonoBehaviour
{
    [Header("Causality Settings")]
    [Tooltip("Samakan dengan nama Flag ID yang diset di Masa Lalu")]
    [SerializeField] private string targetFlagId;

    [Tooltip("Apakah objek ini muncul atau hilang jika Flag aktif?")]
    [SerializeField] private bool activeWhenFlagSet = true;

    private void OnEnable()
    {
        // 1. Subscribe ke teriakan Manager
        PuzzleStateManager.OnFlagChanged += HandleFlagChanged;

        // 2. Cek status awal saat map dimuat (berjaga-jaga kalau player bolak-balik timeline)
        if (PuzzleStateManager.Instance != null)
        {
            bool currentState = PuzzleStateManager.Instance.GetFlag(targetFlagId);
            ApplyState(currentState);
        }
    }

    private void OnDisable()
    {
        // Berhenti mendengar jika objek ini mati
        PuzzleStateManager.OnFlagChanged -= HandleFlagChanged;
    }

    private void HandleFlagChanged(string changedFlagId, bool newValue)
    {
        // Jika yang berubah adalah Flag milikku, maka bereaksilah!
        if (changedFlagId == targetFlagId)
        {
            ApplyState(newValue);
        }
    }

    private void ApplyState(bool flagState)
    {
        // Logic: Jika activeWhenFlagSet = true, maka saat flag = true objek akan nyala.
        // Sebaliknya, kamu bisa bikin objek yang HANCUR di masa depan kalau di masa lalunya flag = true.
        bool finalState = flagState == activeWhenFlagSet;
        gameObject.SetActive(finalState);

        // Catatan: Kalau efeknya bukan SetActive (misal ganti warna/animasi), ganti kodenya di baris ini.
    }
}