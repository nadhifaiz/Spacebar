using UnityEngine;

public class TerrasectPieceController : MonoBehaviour, IInteractable
{

    private void Start()
    {
        // Melapor ke Manager: "Bos, aku sudah spawn! Masukkan aku ke daftar!"
        if (TerrasectManager.Instance != null)
        {
            TerrasectManager.Instance.RegisterTerrasectPiece(this.gameObject);
        }
    }

    public void OnPlayerEnter()
    {
        // Optional: Highlight the piece or show UI prompt
    }

    public void OnPlayerExit()
    {
        // Optional: Remove highlight or hide UI prompt
    }

    public void OnInteract()
    {
        TerrasectManager.Instance.CollectTerrasectPiece();
        gameObject.SetActive(false); // Remove the piece from the scene
    }
}
