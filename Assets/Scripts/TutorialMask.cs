using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMask : MonoBehaviour
{

    private GroupAdder groupAdder;
    [SerializeField] private GameObject tutorialText;
    [SerializeField] private int curNodeGroup = 1; 
    // [SerializeField] private Sprite[] mask;
    // [SerializeField][TextArea] private string[] texts;
    // [SerializeField] private float[] posTextX, posTextY;
    // [SerializeField] private bool[] tutorialStepCanAct;
    [SerializeField] private List<TutorialStep> steps;
    [SerializeField] private bool waiting;
    [SerializeField] private TutorialStep curStep;
    [SerializeField] private bool canProceede = true;

    // private int tutorialStep = 0;

    // private bool canAct(){
    //     return tutorialStepCanAct[tutorialStep];
    // }

    private void Start()
    {
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
            Debug.Log("Updating Procede");
            Debug.Log("Is action: " + curStep.isAction + " Button Down: " + Input.GetMouseButtonDown(0) + 
                        " CurNodeGroup: " + curNodeGroup + " TutorialCount - 1: " + (groupAdder.getTutorialCount()));
            canProceede = true;
        }
    }
    private void handleStep(){

        Debug.Log("Start step");

        this.GetComponent<SpriteRenderer>().sprite = curStep.mask;
        // tutorialText.GetComponent<TextMesh>().text = texts[curTutorial]; get another sprite renderer to render the text

        if(curStep.isAction){
            Debug.Log("Is action step CurNodeGroup: " + curNodeGroup + "To Tutorial Count: " + (groupAdder.getTutorialCount() - 1));
            curNodeGroup = groupAdder.getTutorialCount(); //Ok, really need to get a grip
        }
    }

    private void scheduleStop(float stopDelay){
        Debug.Log("Schedule Stop");
        waiting = true;
        Invoke("stopTime", stopDelay);
    }

    private void stopTime(){
        Debug.Log("Stopping time");
        Time.timeScale = 0;
        waiting = false;
        proceedToNextStep();
    }

    private void continueTime(){
        Debug.Log("Continue Time");
        Time.timeScale = 1.0f;
        if(curStep.isStop) scheduleStop(curStep.stopDelay);
        else proceedToNextStep();
    }

    private void proceedToNextStep(){
        Debug.Log("Proceeding to next Step");
        if(steps.Count <= 0){
            Debug.Log("Tutorial ended");
            //TODO return to main menu
        } 
        steps.RemoveAt(0);
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
