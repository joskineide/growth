using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverallGameManager : MonoBehaviour {

    [SerializeField] private bool canInteractMenu;
    private bool isMute;
    private bool isColorBlind;
    private static OverallGameManager ogm;

    public bool checkMute(){
        return this.isMute;
    }

    public void setMute(bool mute){
        this.isMute = mute;
    }

    public bool checkColorBlind(){
        return this.isColorBlind;
    }

    public void setColorBlind(bool colorBlind){
        this.isColorBlind = colorBlind;
    }

    public bool checkInteractMenu(){
        return this.canInteractMenu;
    }

    public void setInteractMenu(bool interactMenu){
        this.canInteractMenu = interactMenu;
    }

    public void toggleMute(){
        this.isMute = !this.isMute;
    }

    public void toggleColorblind(){
        this.isColorBlind = !this.isColorBlind;
    }

    private void Awake()
    {
        if (ogm == null)
		{
			DontDestroyOnLoad (gameObject);
			ogm = this;
		}
		else if (ogm != this)
		{
			Destroy (gameObject);
		}
    }

	void Update () {
        AudioListener.volume = isMute ? 0 : 1;	
	}

    public void backToMainMenu(){
        Time.timeScale = 1f;
        Debug.Log("Going back to main menu");
        SceneManager.LoadScene(0);
    }
}

public static class Enums {
    public enum TileColor{
        Empty,
        Yellow,
        Green,
        Blue,
        Red,
        White
    }
} 
