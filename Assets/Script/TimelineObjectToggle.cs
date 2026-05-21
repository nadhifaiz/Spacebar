using UnityEngine;

/// <summary>
/// Taruh di object yang hanya muncul di salah satu timeline.
/// Misalnya: bangunan rusak hanya di Future, bangunan utuh hanya di Past.
/// </summary>
public class TimelineObjectToggle : MonoBehaviour
{
    [Tooltip("Centang jika object ini hanya ada di Past. Kosongkan jika hanya di Future.")]
    [SerializeField] private bool existsInPast = true;

    private void OnEnable()
    {
        // Subscribe ke event TimeSwitchManager
        if (TimeSwitchManager.Instance != null)
        {
            TimeSwitchManager.Instance.onEnterPast.AddListener(OnEnterPast);
            TimeSwitchManager.Instance.onEnterFuture.AddListener(OnEnterFuture);

            // Set state awal
            ApplyState(TimeSwitchManager.Instance.IsInPast);
        }
    }

    private void OnDisable()
    {
        if (TimeSwitchManager.Instance != null)
        {
            TimeSwitchManager.Instance.onEnterPast.RemoveListener(OnEnterPast);
            TimeSwitchManager.Instance.onEnterFuture.RemoveListener(OnEnterFuture);
        }
    }

    private void OnEnterPast() => ApplyState(true);
    private void OnEnterFuture() => ApplyState(false);

    private void ApplyState(bool isInPast)
    {
        // Muncul jika timeline cocok
        gameObject.SetActive(existsInPast == isInPast);
    }
}
