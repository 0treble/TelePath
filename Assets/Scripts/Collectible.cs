using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool isCollected = false; // To ensure collectible is only collected once

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return; // Exit if already collected

        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            isCollected = true; // Mark as collected
            playerInventory.CollectiblesCollected(other.transform.position);
            gameObject.SetActive(false); // Disable collectible after collection
        }
    }
}
