using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallGameManager : MonoBehaviour {

    public bool canInteractMenu;
    public bool isMute;
    public bool isColorBlind;
    public static OverallGameManager ogm;


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

    // Use this for initialization
    void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
         if (isMute)	
         {
             AudioListener.volume = 0;
         }
         else
         {
            AudioListener.volume = 1;
         }	
	}
}
