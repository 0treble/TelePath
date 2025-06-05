using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI collectibleText;
    void Start()
    {
        collectibleText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCollectibleText(PlayerInventory playerInventory)
    {
        collectibleText.text = playerInventory.NumberOfCollectibles.ToString();
    }
}
