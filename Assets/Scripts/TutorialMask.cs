using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMask : MonoBehaviour
{

    private GroupAdder groupAdder;
    [SerializeField]
    private GameObject tutorialText;
    [SerializeField]
    private int curTutorial = -1;
    [SerializeField]
    private Sprite[] mask;
    [SerializeField][TextArea]
    private string[] texts;
    [SerializeField]
    private float[] posTextX, posTextY;

    void Start()
    {
        groupAdder = FindObjectOfType<GroupAdder>();
        tutorialText.GetComponent<Renderer>().sortingLayerName = "GameOver";
    }

    void Update()
    {
        if (curTutorial != groupAdder.getTutorialCount() - 1)
        {
            Debug.Log(curTutorial);
            curTutorial = groupAdder.getTutorialCount() - 1;
            this.GetComponent<SpriteRenderer>().sprite = mask[curTutorial];
            tutorialText.GetComponent<TextMesh>().text = texts[curTutorial];
            tutorialText.transform.position = new Vector3(posTextX[curTutorial], posTextY[curTutorial], -1);
        }

    }
}
