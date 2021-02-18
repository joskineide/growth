using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleScript : MonoBehaviour {

	private BoardManager bm;
	private float xOffset;
	private float yOffset;
	[SerializeField] private float border;
	void Start () {
		bm = FindObjectOfType<BoardManager> ();	

		xOffset = bm.getBoardSizeX() % 2 == 0f ? 0.5f : 0;
		yOffset = bm.getBoardSizeY() % 2 == 0f ? 0.5f : 0;

		transform.position = new Vector3(bm.transform.position.x - xOffset,
										bm.transform.position.y - yOffset,
										transform.position.z);

		transform.localScale = new Vector3 (bm.getBoardSizeX() + border, 
											bm.getBoardSizeY() + border, 
											transform.localScale.z);
	}
}
