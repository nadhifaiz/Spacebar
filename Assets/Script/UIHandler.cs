using UnityEngine;
using Game.Player;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject notifATCPanel; // Panel untuk notifikasi kalau semua Terrasect Piece sudah dikumpulkan
    [SerializeField] private GameObject TerrasectCraftButton; // Tombol untuk crafting controller, bisa diaktifkan setelah semua piece terkumpul
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private TerrasectManager terrasectManager;
    [SerializeField] private PlayerInputHandler inputHandler; // Untuk mendeteksi input tombol Terrasect

    private void OnEnable()
    {
        TerrasectManager.onAllPiecesCollected += ShowNotificationATCCollected;
        TerrasectManager.onAllPiecesCollected += EnableCraftButton;
    }

    private void OnDisable()
    {
        TerrasectManager.onAllPiecesCollected -= ShowNotificationATCCollected;
        TerrasectManager.onAllPiecesCollected -= EnableCraftButton;
    }

    private void Update()
    {
        if (inputHandler.TerrasectPressed)
        {
            // Cek jika semua piece sudah terkumpul dan belum pernah di-craft sebelumnya
            if (terrasectManager.IsAllPiecesCollected() && !terrasectManager.IsTerrasectCrafted())
            {
                Debug.LogWarning("Tombol T Terdeteksi");
                OnCraftButtonClicked(); // Panggil fungsi untuk memulai crafting
                terrasectManager.SetTerrasectCrafted(true); // Pastikan crafting hanya bisa dilakukan sekali
            }
            else if (terrasectManager.IsTerrasectCrafted())
            {
                Debug.Log("Kamu menggunakan Terrasect Controller! (Implementasikan efeknya di sini)");
                // Implementasikan efek penggunaan Terrasect Controller di sini, misalnya membuka area baru, mengaktifkan mekanik khusus, dll.
            }
        }
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
        // Logika untuk memulai crafting controller
        Debug.Log("Crafting button clicked! Starting crafting process...");
        puzzlePanel.SetActive(true); // Tampilkan panel puzzle
        terrasectManager.StartCrafting();
    }
}