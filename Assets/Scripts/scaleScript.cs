using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleScript : MonoBehaviour {

	public BoardManager BM;
	private float sizeX;
	private float sizeY;
	private float xOffset;
	private float yOffset;
	public float border;
	// Use this for initialization
	void Start () {
		BM = FindObjectOfType<BoardManager> ();	

		if (BM.boardSize [0] % 2 != 0f) {
			xOffset = 0;
		} else { xOffset = 0.5f;
		}

		if (BM.boardSize [1] % 2 != 0f) {
			yOffset = 0;
		} else { yOffset = 0.5f;
		}

		transform.position = new Vector3(BM.transform.position.x - xOffset,BM.transform.position.y - yOffset,transform.position.z);
		sizeX = BM.boardSize [0];
		sizeY = BM.boardSize [1];

		transform.localScale = new Vector3 (sizeX + border,sizeY + border,transform.localScale.z );

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
