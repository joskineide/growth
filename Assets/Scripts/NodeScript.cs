using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour {

    [SerializeField] private int[] nodePos;
    [SerializeField] private int curId, lastId;
    [SerializeField] private Sprite[] curSprite;
    private BoardManager bm;
	private bool isSparking;
	private float sparkTimer;
	private Animator anim;

    public int getPosX(){
        return nodePos[0];
    }

    public int getPosY(){
        return nodePos[1];
    }

    public void resetNode(){
        ChangeTo(0);
        anim.SetBool("hardResset", true);
    }

    public int getCurId(){
        return this.curId;
    }

    public int getLastId(){
        return this.lastId;
    }

	void Awake () {
        bm = FindObjectOfType<BoardManager>();
		anim = GetComponent<Animator> ();
        nodePos = new int[2];
        nodePos[0] = (int)transform.position.x + (bm.getBoardSizeX() / 2);
        nodePos[1] = (int)transform.position.y + (bm.getBoardSizeY() / 2);
        bm.setNode(nodePos[0], nodePos[1], this);
        ChangeTo(0);
		sparkTimer = Random.Range (50f, 600f);
	}
    public void ChangeTo(Enums.TileColor tileColor)
    {
        // Debug.Log("Changing color x: " + nodePos[0] + " y: " + nodePos[1] + " Color: " + tileColor);
        if (tileColor != Enums.TileColor.Empty)
        {
            anim.SetBool("hardResset", false);
        }
        lastId = curId;
        anim.SetInteger ("lastType", curId);
        curId = (int) tileColor;
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
