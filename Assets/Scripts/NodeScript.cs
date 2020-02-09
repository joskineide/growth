using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour {

    [HideInInspector]
    public int[] nodePos;
    [HideInInspector]
    public int curId, lastId;
    [HideInInspector]
    public Sprite[] curSprite;
    [HideInInspector]
    public BoardManager bm;


	private bool isSparking;
	private float sparkTimer;

	public Animator anim;

	void Awake () {
        bm = FindObjectOfType<BoardManager>();
		anim = GetComponent<Animator> ();
        nodePos = new int[2];
        nodePos[0] = (int)transform.position.x + (bm.boardSize[0] / 2);
        nodePos[1] = (int)transform.position.y + (bm.boardSize[1] / 2);
        bm.nodes[nodePos[0], nodePos[1]] = this;
        ChangeTo(0);
		sparkTimer = Random.Range (50f, 600f);
	}
    public void ChangeTo(int i)
    {
        if (i != 0)
        {
          anim.SetBool("hardResset", false);
        }
        lastId = curId;
        anim.SetInteger ("lastType", curId);
        curId = i;
        GetComponent<SpriteRenderer>().sprite = curSprite[i];
    }
	void Update()
	{
		anim.SetInteger ("type", curId);
		if (!isSparking) {
			sparkTimer -= Time.deltaTime;
			if (sparkTimer <= 0) {
				isSparking = true;
			}
		} else
		{
			sparkTimer = Random.Range (50f, 600f);
			isSparking = false;
		}



	}
}
