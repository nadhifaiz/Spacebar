using UnityEngine;

public class TerrasectPieceController : MonoBehaviour, IInteractable
{
    
    private JigsawPiece jigsawPiece;

    private void Start()
    {
        jigsawPiece = GetComponent<JigsawPiece>();
        if (jigsawPiece == null)
        {
            Debug.LogError("JigsawPiece component not found on TerrasectPieceController.");
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


    private void EnableJigsawBehavior()
    {
        if (jigsawPiece != null)
        {
            // Enable the JigsawPiece component to allow dragging and dropping
            Debug.Log("Enabling JigsawPiece behavior for " + gameObject.name);
            jigsawPiece.enabled = true;
        }
    }
}
