using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject uiLayout;
    public int buttonClick;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClickEnter()
    {
        
        buttonClick += 1;
        if(buttonClick % 2 == 0)
        {
            uiLayout.SetActive(false);
        }
        else
        {
            uiLayout.SetActive(true);
        }
    }

}
