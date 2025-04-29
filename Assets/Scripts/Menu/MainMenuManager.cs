using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // For Dropdown

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown difficultyDropdown;

    public void StartGame()
    {
        // Set difficulty based on dropdown
        GameManager.Instance.SetDifficulty(difficultyDropdown.value);
        GameManager.Instance.StartGame();
    }

    public void OpenSettings()
    {
        FindObjectOfType<SettingsMenuManager>().OpenSettings();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}