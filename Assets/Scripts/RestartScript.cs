using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour {

    public BoardManager bm;

    void Start(){
        bm = FindObjectOfType<BoardManager>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) &&!bm.isScoring)
        {
            FindObjectOfType<BoardManager>().RestartBoard();
        }
    }
}
