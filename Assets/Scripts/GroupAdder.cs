using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupAdder : MonoBehaviour {
    //tutorialPrefabs setta no inspetor mesmo, são os prefabs utilizados no tutorial #!!#
    public GameObject[] allGroups, tutorialPrefabs;
    public GameObject activeGroup;
    public int groupID, dice, wDice, groupChose, tWeight, wCheck, tutorialCount;
    public int[] initialWeight;
    public bool isLoading, canTutorialChange;
    GroupNoteToPlace activeGroupScript;
    private BoardManager bm;

	void Start () {
        SetupStats(initialWeight);
        bm = FindObjectOfType<BoardManager>();
	}
    void SetupStats(int[] nodeW)
    {
        for (int i = 0; i < nodeW.Length; i++)
        {
            tWeight += nodeW[i];
        }
    }

	void Update () {
        //Aqui fala quais vão ser o ud de cada peça spawnada #!!#
        if (canTutorialChange && activeGroupScript.ntp.Length > 0){
            switch(tutorialCount){
                case 1:
                    activeGroupScript.ntp[0].changeId(1);
                    activeGroupScript.ntp[1].changeId(1);
                    break;
                case 2:
                    activeGroupScript.ntp[0].changeId(2);
                    activeGroupScript.ntp[1].changeId(2);
                    activeGroupScript.ntp[2].changeId(2);
                    break;
                case 3:
                    activeGroupScript.ntp[0].changeId(3);
                    activeGroupScript.ntp[1].changeId(4);
                    activeGroupScript.ntp[2].changeId(3);
                    activeGroupScript.ntp[3].changeId(2);
                    break;
            }
            canTutorialChange = false;
        }

        if(!bm.isTutorial){
            if (activeGroup == null && !isLoading)
            {
                groupChose = 0;
                wCheck = 0;
                wDice = Random.Range(0, tWeight);
                for (int i = 0; i < initialWeight.Length; i++)
                {
                    wCheck += initialWeight[i];
                    if (wDice < wCheck)
                    {
                        groupChose = i;
                        break;
                    }
                }
                switch (groupChose)
                {
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
                activeGroup = Instantiate(allGroups[dice], transform.position, Quaternion.identity);
                if (!bm.isScoring)
                {
                    bm.CheckEnd();
                }
            }
            if (isLoading)
            {
                if (activeGroup != null)
                {
                    Destroy(activeGroup);
                }
                activeGroup = Instantiate(allGroups[dice], transform.position, Quaternion.identity);
                activeGroup.GetComponent<GroupNoteToPlace>().isLoading = true;
                isLoading = false;
            }
        }
        else
        {
            if (activeGroup == null && !isLoading)
            {
                if (tutorialCount < tutorialPrefabs.Length)
                {
                    activeGroup = Instantiate(tutorialPrefabs[tutorialCount], transform.position, Quaternion.identity);
                    activeGroupScript = activeGroup.GetComponent<GroupNoteToPlace>();
                    canTutorialChange = true;
                    tutorialCount++;
                }
                else
                {
                    tutorialCount = 4; 
                }
            }
        }
        if (activeGroup != null)
        {
            activeGroup.GetComponent<GroupNoteToPlace>().groupID = groupID;
        }
	}
}
