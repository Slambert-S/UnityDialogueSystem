using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class naratorControl : MonoBehaviour
{
    public Image namePlate;
    public TextMeshProUGUI characterNameText;
    // Start is called before the first frame update
   
    public virtual void deActivateNarator()
    {
        if(namePlate != null)
        {
            namePlate.enabled = false;
        }

        if(characterNameText != null)
        {
            characterNameText.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public virtual void activateNarator()
    {
        if (namePlate != null)
        {
            namePlate.enabled = true;
        }

        if (characterNameText != null)
        {
            characterNameText.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
