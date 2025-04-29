using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        settingsPanel.SetActive(false); // Start hidden
        LoadSettings();
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0f;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Make sure game time is normal again
        SceneManager.LoadScene("MainMenuScene"); // <- Put your exact Main Menu scene name
    }

    public void SetMusicVolume(float volume)
    {
        if (volume <= 0.001f) volume = 0.001f; // Prevent Log10(0)

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        if (volume <= 0.001f) volume = 0.001f; // Prevent Log10(0)

        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            musicVolumeSlider.value = savedMusicVolume;
            SetMusicVolume(savedMusicVolume);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume");
            sfxVolumeSlider.value = savedSFXVolume;
            SetSFXVolume(savedSFXVolume);
        }
    }
}
