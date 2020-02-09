using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeToPlace : MonoBehaviour {
    [HideInInspector]
    public int nodeID;
    [HideInInspector]
    public int posX;
    [HideInInspector]
    public int posY;
    [HideInInspector]
    public BoardManager bm;
    [HideInInspector]
    public GroupNoteToPlace gntp;
    [HideInInspector]
    public SpriteRenderer actualSprite;
    [HideInInspector]
    public Sprite[] sprites;
    public bool isLoading;

	void Start () {
        bm = FindObjectOfType<BoardManager>();
        gntp = GetComponentInParent<GroupNoteToPlace>();
        actualSprite = GetComponent<SpriteRenderer>();
        if(!isLoading)
            nodeID = Random.Range(1, 5);
        actualSprite.sprite = sprites[nodeID - 1];

	}
    void Update()
    {
        if (isLoading)
        {
            actualSprite.sprite = sprites[nodeID - 1];
            isLoading = false;
        }
    }

    public bool CheckAvailable() 
    {
        posX = Mathf.RoundToInt(transform.position.x + (bm.boardSize[0] / 2));
        posY = Mathf.RoundToInt(transform.position.y + (bm.boardSize[1] / 2));
        if (posX >= 0 && posX < bm.boardSize[0] && posY >= 0 && posY < bm.boardSize[1])
        {
            if (bm.nodes[posX,posY].curId == 0)
            {
                return true;
            }
        }
        return false;
    }

    public void changeId(int toId)
    {
        nodeID = toId;
        actualSprite.sprite = sprites[nodeID-1];
    }
}
