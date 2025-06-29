using System.Text;
using TMPro;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public TMP_Text inventoryText;

    private void Start()
    {
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        var items = IDosGames.UserInventory.GetAll();

        if (items == null || items.Count == 0)
        {
            inventoryText.text = "Инвентарь пуст.";
            return;
        }

        StringBuilder sb = new StringBuilder("🎒 Инвентарь:\n");

        foreach (var item in items)
        {
            sb.AppendLine($"{item.ItemID} — {item.Amount}");
        }

        inventoryText.text = sb.ToString();
    }
}
