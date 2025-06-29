using IDosGames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    public TMP_Text currencyText;

    private void Start()
    {
        UpdateCurrency();
    }

    private void UpdateCurrency()
    {
        int goldAmount = UserInventory.GetVirtualCurrencyAmount("GOLD"); // можно заменить на "CO" или "IG"
        currencyText.text = $"GOLD: {goldAmount}";
    }
}
