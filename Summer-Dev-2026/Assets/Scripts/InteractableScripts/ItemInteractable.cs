using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("взаимодействие с предметом");
        Destroy(gameObject);
    }

}
