using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteScript : MonoBehaviour {

	private OverallGameManager ogm;
	private bool isActive;
	[SerializeField] private Sprite[] states;

	void Start()
	{
		ogm = FindObjectOfType<OverallGameManager>();
		isActive = ogm.checkMute();
		updateSprite();
	}

	private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isActive = !isActive;
			ogm.setMute(isActive);
			updateSprite();
        }
    }

	private void updateSprite(){
		GetComponent<SpriteRenderer>().sprite = isActive ? states[0] : states[1];
	}
}
