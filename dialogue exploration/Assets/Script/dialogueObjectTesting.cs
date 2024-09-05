using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class objectDialogue
{
    public string id;
    public List<dialogueObject> dialogueLine = new List<dialogueObject>();
}
public class dialogueObjectTesting : MonoBehaviour
{
   public objectDialogue objectDialogue;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(objectDialogue);
    }

}
