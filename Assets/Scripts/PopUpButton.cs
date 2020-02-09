using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpButton : MonoBehaviour {

    public OverallGameManager overallManager;
    public int type;
    public bool isEnabled;
    public GameObject selectedSprite;

    private void Start()
    {
        overallManager = FindObjectOfType<OverallGameManager>();
    }

    // Update is called once per frame
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (type == 0)
            {
                overallManager.canInteractMenu = true;
                transform.parent.gameObject.SetActive(false);
            }
            else if (type == 1)
            {
                overallManager.isMute = !overallManager.isMute;
            }
            else if (type == 2)
            {
                overallManager.isColorBlind = !overallManager.isColorBlind;
            }
        }
    }

    private void Update()
    {
        if(selectedSprite != null) selectedSprite.SetActive(isEnabled);
        if (type == 1)
            isEnabled = overallManager.isMute;
        else if (type == 2)
            isEnabled = overallManager.isColorBlind;
    }
}
