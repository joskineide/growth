using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMask : MonoBehaviour
{

    private GroupAdder groupAdder;
    [SerializeField] private GameObject tutorialText;
    private int curNodeGroup = 1; 
    [SerializeField] private List<TutorialStep> steps;
    private bool waiting = false;
    private TutorialStep curStep;
    private bool canProceede = false;

    private OverallGameManager gameManager;

    // private int tutorialStep = 0;

    // private bool canAct(){
    //     return tutorialStepCanAct[tutorialStep];
    // }

    private void Start()
    {
        gameManager = FindObjectOfType<OverallGameManager>();
        groupAdder = FindObjectOfType<GroupAdder>();
        // curNodeGroup = groupAdder.getTutorialCount();
        tutorialText.GetComponent<Renderer>().sortingLayerName = "GameOver";
        curStep = steps[0];
        StartCoroutine(waitForProcede());
    }

    private IEnumerator waitForProcede(){
        Debug.Log("Waiting");
        while(true) 
        {
            if(canProceede){
                canProceede = false;
                handleStep();
                continueTime();
                yield break;
            }
            yield return null;
        }
    }
    
    private void Update(){
        if(!canProceede &&
            (
                (!curStep.isAction && Input.GetMouseButtonDown(0)) ||
                (curStep.isAction && curNodeGroup != groupAdder.getTutorialCount())
            )
            && !waiting){
            canProceede = true;
        }
    }
    private void handleStep(){

        this.GetComponent<SpriteRenderer>().sprite = curStep.mask;
        // tutorialText.GetComponent<TextMesh>().text = texts[curTutorial]; get another sprite renderer to render the text

        if(curStep.isAction){
            Debug.Log("Is action step CurNodeGroup: " + curNodeGroup + "To Tutorial Count: " + (groupAdder.getTutorialCount() - 1));
            curNodeGroup = groupAdder.getTutorialCount(); //Ok, really need to get a grip
        }
    }

    private void scheduleStop(float stopDelay){
        waiting = true;
        Invoke("stopTime", stopDelay);
    }

    private void stopTime(){
        Time.timeScale = 0;
        waiting = false;
        proceedToNextStep();
    }

    private void continueTime(){
        Time.timeScale = 1.0f;
        if(curStep.isStop) scheduleStop(curStep.stopDelay);
        else proceedToNextStep();
    }

    private void proceedToNextStep(){
        steps.RemoveAt(0);
        if(steps.Count <= 0){
            gameManager.backToMainMenu();
            return;
        } 
        curStep = steps[0];
        StartCoroutine(waitForProcede());
    }

    [System.Serializable] private class TutorialStep{
        public float stopDelay;
        public bool isAction;
        public Sprite mask;
        public bool isStop;
    }

}
