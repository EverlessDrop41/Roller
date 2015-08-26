using UnityEngine;
using System.Collections;

public class GenericUIScript : MonoBehaviour {

	public void LoadScene(int sceneIndex)
    {
        Application.LoadLevel(sceneIndex);
    }

    public void LoadScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void ResetScene()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetMusicVolume(int vol)
    {
        PlayerPrefs.SetInt("MusicVolume", vol);
    }

    public void SetSFXVolume(int vol)
    {
        PlayerPrefs.SetInt("SFXVolume", vol);
    }
}
