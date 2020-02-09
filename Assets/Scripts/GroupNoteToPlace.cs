using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupNoteToPlace : MonoBehaviour {

    public int groupID, tutorialCount;
    [HideInInspector]
    public BoardManager bm;
    [HideInInspector]
    public NodeToPlace[] ntp;
    [HideInInspector]
    public bool canPlace;
    [HideInInspector]
    public Vector3 initialPos;
    public bool selected, isLoading, canSound;
    public AudioSource audioSrc;
    public AudioClip pickUpSound;
    public bool[,,] tutorialExpected;


	public float size;
    // Use this for initialization
    void Start () {
        canSound = true;
        audioSrc = GetComponent<AudioSource>();
        initialPos = transform.position;
        ntp = GetComponentsInChildren<NodeToPlace>();
        bm = FindObjectOfType<BoardManager>();
        if(bm.isTutorial)
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
                Debug.Log("Group ID" + groupID + "Adding node..."+i);
                ntp[i].isLoading = true;
                ntp[i].nodeID = PlayerPrefs.GetInt("G" + groupID + "Node" + i);
            }
        }
	}
    void Update()
    {
        if (!Input.GetMouseButton(0) && transform.position != initialPos && selected == true)
        {
            if (!bm.isScoring)
            {
                canPlace = true;
                for (int i = 0; i < ntp.Length; i++)
                {
                    if (!ntp[i].CheckAvailable())
                    {
                        if(!bm.isTutorial)
                        {
                            canPlace = false;
                        }
                    }
                }
                if(bm.isTutorial){
                    if(ntp[0].posX >= 0 &&  ntp[0].posX < bm.boardSize[0] && ntp[0].posY >= 0 && ntp[0].posY < bm.boardSize[1])
                    {
                        Debug.Log("Posx:"+ntp[0].posX+" PosY:"+ntp[0].posY);
                        canPlace = tutorialExpected[ntp.Length-2,ntp[0].posX,ntp[0].posY];
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
                        bm.score += 5;
                        bm.nodes[ntp[i].posX, ntp[i].posY].ChangeTo(ntp[i].nodeID);
                    }
                    bm.audioSrc.PlayOneShot(bm.placeSound);
                    Debug.Log("PLACIN DOWN");
                    bm.CheckBoard();
                    bm.CheckClusterCanScore();
                    transform.position = initialPos;
                    Destroy(gameObject);
                }
            }
            if (canPlace == false)
            {
                audioSrc.PlayOneShot(pickUpSound);
                Debug.Log("CANT PLACE DOWN FOOL");
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
        if (!bm.gameOver)
        {
            transform.localScale = new Vector2(1, 1);
            if (!bm.isScoring)
            {
                if (canSound)
                {
                    audioSrc.PlayOneShot(pickUpSound);
                    Debug.Log("PICKIN UP");
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
