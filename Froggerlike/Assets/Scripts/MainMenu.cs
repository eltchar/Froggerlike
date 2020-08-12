using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Slider sliderBGM;
    private Slider sliderSFX;
    private Slider sliderDifficulty;
    private void Start()
    {
        sliderBGM = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
        sliderBGM.value = AudioManager.instance.BGMVolume;
        sliderSFX = GameObject.Find("EffectsVolumeSlider").GetComponent<Slider>();
        sliderSFX.value = AudioManager.instance.SFXVolume;
        sliderDifficulty = GameObject.Find("DifficultySlider").GetComponent<Slider>();
        sliderDifficulty.value = (GameManagerScript.instance.difficultyLevel-0.25f)/0.25f;
        GameObject.Find("OptionsMenu").SetActive(false);
    }
    public void StartGame()
    {
        GameManagerScript.instance.MoveToLevel1();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeSFXVolume(float volume)
    {
        AudioManager.instance.ChangeSFXVolume(volume);
    }
    public void ChangeBGMVolume(float volume)
    {
        AudioManager.instance.ChangeBGMVolume(volume);
    }

    public void ChangeDifficultyFactor(float value)
    {
        switch (value)
        {
            case 1:
                GameManagerScript.instance.difficultyLevel = 0.5f;
                break;
            case 2:
                GameManagerScript.instance.difficultyLevel = 0.75f;
                break;
            case 3:
                GameManagerScript.instance.difficultyLevel = 1.0f;
                break;
            case 4:
                GameManagerScript.instance.difficultyLevel = 1.25f;
                break;
            case 5:
                GameManagerScript.instance.difficultyLevel = 1.5f;
                break;
            default:
                break;
        }
        
    }

}
