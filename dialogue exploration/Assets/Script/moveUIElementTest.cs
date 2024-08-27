using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveUIElementTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject targetedPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveUiElement()
    {
        //To make this work both element need to have the same anchor point 
        //Kinda work
        //this.gameObject.GetComponent<RectTransform>().anchoredPosition = targetedPosition.GetComponent<RectTransform>().anchoredPosition;
        this.gameObject.transform.position = targetedPosition.transform.position;
    }
}
