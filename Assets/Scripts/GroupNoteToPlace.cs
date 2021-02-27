using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupNoteToPlace : MonoBehaviour {

    private int groupID, tutorialCount;
    [HideInInspector]
    private BoardManager bm;
    [HideInInspector]
    private NodeToPlace[] ntp;
    [HideInInspector]
    private bool canPlace;
    [HideInInspector]
    private Vector3 initialPos;
    private bool selected, isLoading, canSound;
    private AudioSource audioSrc;
    [SerializeField] private AudioClip pickUpSound;
    private bool[,,] tutorialExpected;

    public int getNodesAmount(){
        if(ntp == null) return 0;
        return this.ntp.Length;
    }

    public NodeToPlace getNodeToPlace(int i){
        return this.ntp[i];
    }

    public void setLoading(bool loading){
        this.isLoading = loading;
    }

    public void setGroupId(int groupID){
        this.groupID = groupID;
    }


	public float size;
    // Use this for initialization
    void Start () {
        canSound = true;
        audioSrc = GetComponent<AudioSource>();
        initialPos = transform.position;
        ntp = GetComponentsInChildren<NodeToPlace>();
        bm = FindObjectOfType<BoardManager>();
        if(bm.checkTutorial())
        {   
            //Aqui vai settar todos as posições onde os grupos podems ser encaixados #!!#
            tutorialExpected = new bool[3,10,10];
            tutorialExpected[0,3,8] = true;
            tutorialExpected[1,7,8] = true;
            tutorialExpected[2,3,0] = true;
        }
		transform.localScale = new Vector2 (size, size);
        if (isLoading)
        {
            for (int i = 0; i < ntp.Length; i++)
            {
                // Debug.Log("Group ID" + groupID + "Adding node..."+i);
                ntp[i].setLoading(true);
                ntp[i].setNodeId(PlayerPrefs.GetInt("G" + groupID + "Node" + i));
            }
        }
	}
    void Update()
    {
        if (!Input.GetMouseButton(0) && transform.position != initialPos && selected == true)
        {
            if (!bm.checkScoring())
            {
                canPlace = true;
                for (int i = 0; i < ntp.Length; i++)
                {
                    if (!ntp[i].CheckAvailable())
                    {
                        if(!bm.checkTutorial())
                        {
                            canPlace = false;
                        }
                    }
                }
                if(bm.checkTutorial()){
                    if(ntp[0].getPosX() >= 0 &&  ntp[0].getPosX() < bm.getBoardSizeX()                    
                        && ntp[0].getPosY() >= 0 && ntp[0].getPosY() < bm.getBoardSizeY())
                    {
                        // Debug.Log("Posx:"+ntp[0].getPosX() +" PosY:"+ntp[0].getPosY());
                        canPlace = tutorialExpected[ntp.Length-2,ntp[0].getPosX(), ntp[0].getPosY()];
                    }
                    else
                    {
                        canPlace = false;
                    }
                }
                if (canPlace)
                {
                    for (int i = 0; i < ntp.Length; i++)
                    {
                        bm.addScore(5);
                        bm.getNode(ntp[i].getPosX(), ntp[i].getPosY()).ChangeTo(ntp[i].getTileColor());
                    }
                    bm.playPlaceSound();
                    // Debug.Log("PLACIN DOWN");
                    bm.CheckBoard();
                    bm.CheckClusterCanScore();
                    transform.position = initialPos;
                    Destroy(gameObject);
                }
            }
            if (canPlace == false)
            {
                audioSrc.PlayOneShot(pickUpSound);
                // Debug.Log("CANT PLACE DOWN FOOL");
                transform.position = initialPos;
                selected = false;
            }
            if (!canSound)
                canSound = true;
            transform.localScale = new Vector2 (size, size);

        }

    }
    void OnMouseDrag()
    {
        if (!bm.checkGameOver())
        {
            transform.localScale = new Vector2(1, 1);
            if (!bm.checkScoring())
            {
                if (canSound)
                {
                    audioSrc.PlayOneShot(pickUpSound);
                    // Debug.Log("PICKIN UP");
                    canSound = false;
                }
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9.9f);

                Vector3 groupPos = Camera.main.ScreenToWorldPoint(mousePos) + new Vector3(0, (GetComponent<BoxCollider2D>().size.y / 2) + 1, 0);
                transform.position = groupPos;
                selected = true;
            }
        }
    }
	void OnMouseOver()
	{
		transform.localScale = new Vector2 (size, size);
	}
}
