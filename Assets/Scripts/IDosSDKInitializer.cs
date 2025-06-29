using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDosGames;

public class IDosSDKInitializer : MonoBehaviour
{
    private void Awake()
    {
        var settings = IDosGamesSDKSettings.Instance;

        if (settings == null)
        {
            Debug.LogError("IDosGamesSDKSettings not found. Please check the Resources/Settings folder.");
            return;
        }

        settings.TitleID = "8HC7K5TB";
        Debug.Log($"iDosGames SDK settings loaded. TitleID: {settings.TitleID}");
    }
}