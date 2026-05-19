using UnityEngine;
using Game.Player;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 0.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private PlayerInputHandler inputHandler;

    private IInteractable currentInteractable;

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactionPoint.position, interactionRadius, interactableLayer);

        if (colliders.Length > 0)
        {
            IInteractable interactable = colliders[0].GetComponent<IInteractable>();

            if (interactable != null && interactable != currentInteractable)
            {
                currentInteractable?.OnPlayerExit();
                currentInteractable = interactable;
                currentInteractable.OnPlayerEnter();
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnPlayerExit();
                currentInteractable = null;
            }
        }

        // Trigger Interaksi dengan tombol E
        if (inputHandler.InteractPressed && currentInteractable != null)
        {
            currentInteractable.OnInteract();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionPoint.position, interactionRadius);
        }
    }
}