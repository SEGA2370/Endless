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
        // We will later open settings panel here
        Debug.Log("Open Settings Menu");
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}