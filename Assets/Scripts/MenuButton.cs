using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    [SerializeField] private bool goesToScene, canInteract;
    [SerializeField] private int whereTo;
    [SerializeField] private GameObject popUp;
    [SerializeField] private OverallGameManager overallManager;

    private void Start()
    {
        overallManager = FindObjectOfType<OverallGameManager>(); 
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && overallManager.checkInteractMenu())
        {
            if (goesToScene)
            {
                SceneManager.LoadScene(whereTo);
            }
            else
            {
                popUp.SetActive(true);
                overallManager.setInteractMenu(false);
            }
        }
    }
}
