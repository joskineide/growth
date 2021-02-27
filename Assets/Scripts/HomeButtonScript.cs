using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButtonScript : MonoBehaviour
{
    private BoardManager bm;
    private OverallGameManager gameManager;

    void Start(){
        bm = FindObjectOfType<BoardManager>();
        gameManager = FindObjectOfType<OverallGameManager>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) {
            if(bm.checkTutorial()){
                gameManager.backToMainMenu();
            } else if(!bm.checkScoring()){
                bm.SaveGame();
                gameManager.backToMainMenu();
            }
        }
        
    }
}
