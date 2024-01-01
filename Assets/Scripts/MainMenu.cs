using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour {

    [SerializeField] Slider volumeSlider;
    

    void Start(){
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
    }

    public void StartGame(){
        SceneManager.LoadScene(1);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void changeVolume(float v){
        AudioListener.volume = v;
        PlayerPrefs.SetFloat("volume", v);
    }
}
