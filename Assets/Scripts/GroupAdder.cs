using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupAdder : MonoBehaviour {
    //tutorialPrefabs setta no inspetor mesmo, são os prefabs utilizados no tutorial #!!#
    [SerializeField] private GameObject[] allGroups, tutorialPrefabs;
    private OverallGameManager gameManager;
    private GameObject activeGroup;
    [SerializeField] private int groupID, dice, groupChose, tutorialCount;
    private bool isLoading, canTutorialChange;
    private GroupNoteToPlace activeGroupScript;
    private BoardManager bm;
    
    public int getTutorialCount(){
        return this.tutorialCount;
    }

    public int getDice(){
        return this.dice;
    }

    public GroupNoteToPlace getActiveGroup(){
        return this.activeGroup.GetComponent<GroupNoteToPlace>();
    }

    public void setLoading(bool isLoading){
        this.isLoading = isLoading;
    }
    public void setDice(int dice){
        this.dice = dice;
    }
    public void destroyActiveGroup(){
        Destroy(activeGroup.gameObject);
    }
	private void Start () {
        bm = FindObjectOfType<BoardManager>();
        gameManager = FindObjectOfType<OverallGameManager>();
        if(bm.checkTutorial()) {
            spawnTutorialPiece(0);
        }
	}
	void Update () {

        if(bm.checkTutorial()) {
            handleTutorial(); 
            return;
        }

        if (activeGroup == null && !isLoading)
        {
            groupChose = gameManager.generateRandomNodeShapeId();

            rollPieceRotation();

            activeGroup = Instantiate(allGroups[dice], transform.position, Quaternion.identity);

            if (!bm.checkScoring()) bm.CheckEnd();
        }

        if (isLoading)
        {
            if (activeGroup != null)
            {
                Destroy(activeGroup);
            }
            activeGroup = Instantiate(allGroups[dice], transform.position, Quaternion.identity);
            activeGroup.GetComponent<GroupNoteToPlace>().setLoading(true);
            isLoading = false;
        }

        checkUpdateGroupId();
	}

    private void checkUpdateGroupId(){
        if (activeGroup != null) activeGroup.GetComponent<GroupNoteToPlace>().setGroupId(groupID);
    }

    private void rollPieceRotation(){
        switch (groupChose){
            case 0:
                dice = 0;
                break;
            case 1:
                dice = Random.Range(1, 3);
                break;
            case 2:
                dice = Random.Range(3, 7);
                break;
            case 3:
                dice = Random.Range(7, 9);
                break;
            case 4:
                dice = 9;
                break;
            case 5:
                dice = Random.Range(10, 12);
                break;
            case 6:
                dice = Random.Range(12, 20);
                break;
            case 7:
                dice = Random.Range(20, 24);
                break;
            case 8:
                dice = Random.Range(24, 28);
                break;
        }
    }

    private void handleTutorial(){

        if (activeGroup == null && !isLoading && tutorialCount < tutorialPrefabs.Length) {
            spawnTutorialPiece(tutorialCount);
            tutorialCount++;
        }

        setTutorialPieceColors();

        checkUpdateGroupId();
    }

    private void spawnTutorialPiece(int pos){
        activeGroup = Instantiate(tutorialPrefabs[pos], transform.position, Quaternion.identity);
        activeGroupScript = activeGroup.GetComponent<GroupNoteToPlace>();
        canTutorialChange = true;
}
    private void setTutorialPieceColors(){

        if (canTutorialChange && activeGroupScript.getNodesAmount() > 0){
            switch(tutorialCount){
                case 1:
                    activeGroupScript.getNodeToPlace(0).changeId(1);
                    activeGroupScript.getNodeToPlace(1).changeId(1);
                    break;
                case 2:
                    activeGroupScript.getNodeToPlace(0).changeId(2);
                    activeGroupScript.getNodeToPlace(1).changeId(2);
                    activeGroupScript.getNodeToPlace(2).changeId(2);
                    break;
                case 3:
                    activeGroupScript.getNodeToPlace(0).changeId(3);
                    activeGroupScript.getNodeToPlace(1).changeId(4);
                    activeGroupScript.getNodeToPlace(2).changeId(3);
                    activeGroupScript.getNodeToPlace(3).changeId(2);
                    break;
            }
            canTutorialChange = false;
        }
    }
}
