using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpButton : MonoBehaviour {

    private OverallGameManager overallManager;
    [SerializeField] private int type;
    private bool isEnabled;
    private GameObject selectedSprite;

    private void Start()
    {
        overallManager = FindObjectOfType<OverallGameManager>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (type == 0)
            {
                overallManager.setInteractMenu(true);
                transform.parent.gameObject.SetActive(false);
            }
            else if (type == 1)
            {
                overallManager.toggleMute();
            }
            else if (type == 2)
            {
                overallManager.toggleColorblind();
            }
        }
    }

    private void Update()
    {
        if(selectedSprite != null) selectedSprite.SetActive(isEnabled);
        if (type == 1)
            isEnabled = overallManager.checkMute();
        else if (type == 2)
            isEnabled = overallManager.checkColorBlind();
    }
}
