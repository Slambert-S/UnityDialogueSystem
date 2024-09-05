using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadSpecificDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    public IdDialogue specificDialogue;
    //public string idToload;
    
    public void loadSpecificDialogueFromJson(string idToload)
    {
        string filePath = Application.persistentDataPath + "/TestDialogueData.json";
        string conversationData = System.IO.File.ReadAllText(filePath);
        IdDialogueList dialoguesList= JsonUtility.FromJson<IdDialogueList>(conversationData);

        specificDialogue = dialoguesList.dialogues.Find(obj => obj.id == idToload);
    }

    public void saveSpecificDialogueToJson()
    {
        string filePath = Application.persistentDataPath + "/TestDialogueData.json";
        string conversationData = System.IO.File.ReadAllText(filePath);
        IdDialogueList dialoguesList = JsonUtility.FromJson<IdDialogueList>(conversationData);

        foreach (IdDialogue dialogue in dialoguesList.dialogues)
        {
            if(dialogue.id == specificDialogue.id)
            {
                dialogue.dialogueLine = specificDialogue.dialogueLine;
                break;
            }
        }
        string updatedJson = JsonUtility.ToJson(dialoguesList);
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, updatedJson);
        Debug.Log("SAVE methode was read");
    }
}
