using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    public bool goesToScene;
    public int whereTo;
    public GameObject popUp;
    public bool canInteract;
    public OverallGameManager overallManager;

    private void Start()
    {
        overallManager = FindObjectOfType<OverallGameManager>(); 
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && overallManager.canInteractMenu)
        {
            if (goesToScene)
            {
                SceneManager.LoadScene(whereTo);
            }
            else
            {
                popUp.SetActive(true);
                overallManager.canInteractMenu = false;
            }
        }
    }
}
