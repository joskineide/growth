using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeToPlace : MonoBehaviour {

    private int nodeID;
    private int posX;
    private int posY;
    private BoardManager bm;
    private GroupNoteToPlace gntp;
    private SpriteRenderer actualSprite;
    [SerializeField] private Sprite[] sprites;
    private bool isLoading;

    public int getPosX(){
        return this.posX;
    }

    public int getPosY(){
        return this.posY;
    }

    public bool checkLoading(){
        return this.isLoading;
    }

    public void setLoading(bool loading){
        this.isLoading = loading;
    }

    public void setNodeId(int id){
        this.nodeID = id;
    }

    public int getNodeId(){
        return this.nodeID;
    }

    public Enums.TileColor getTileColor(){
        return (Enums.TileColor) this.nodeID;
    }

	void Start () {
        bm = FindObjectOfType<BoardManager>();
        gntp = GetComponentInParent<GroupNoteToPlace>();
        actualSprite = GetComponent<SpriteRenderer>();
        if(!isLoading) updateId(Random.Range(1,5));
	}

    public void updateId(int id){
        this.nodeID = id;
        actualSprite.sprite = sprites[nodeID - 1];
    }
    void Update()
    {
        if (isLoading)
        {
            updateId(nodeID);
            isLoading = false;
        }
    }

    public bool CheckAvailable() 
    {
        posX = Mathf.RoundToInt(transform.position.x + (bm.getBoardSizeX() / 2));
        posY = Mathf.RoundToInt(transform.position.y + (bm.getBoardSizeY() / 2));
        return posX >= 0 && posX < bm.getBoardSizeX() && 
                posY >= 0 && posY < bm.getBoardSizeY() &&
                bm.getNodeId(posX,posY) == 0;
    }

    public void changeId(int toId)
    {
        nodeID = toId;
        actualSprite.sprite = sprites[nodeID-1];
    }
}
