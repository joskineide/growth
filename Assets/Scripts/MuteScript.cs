using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteScript : MonoBehaviour {

	public OverallGameManager ogm;
	public bool isActive;
	public Sprite[] states;

	void Start()
	{
		ogm = FindObjectOfType<OverallGameManager>();
		isActive = ogm.isMute;
		if (isActive)
			GetComponent<SpriteRenderer>().sprite = states[0];
		else
			GetComponent<SpriteRenderer>().sprite = states[1];
	}

	 private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isActive = !isActive;
			ogm.isMute = isActive;
			if (isActive)
				GetComponent<SpriteRenderer>().sprite = states[0];
			else
				GetComponent<SpriteRenderer>().sprite = states[1];
        }
    }
}
