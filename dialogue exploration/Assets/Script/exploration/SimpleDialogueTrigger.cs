using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using UnityEngine;


[System.Serializable]
public class SimpleDialogueCharacter
{
    public string name;
    public Sprite icone;
    public CharacterDataScriptableObject characterData;
    public int characterSprite;
}

[System.Serializable]
public class SimpleDialogueLine
{
    public SimpleDialogueCharacter character;
    [TextArea(3,10)]
    public string line;
   
}

[System.Serializable]
public class SimpleDialogue
{
    public List<SimpleDialogueLine> dialogueLine = new List<SimpleDialogueLine>();
}

public class SimpleDialogueTrigger : MonoBehaviour
{
    public SimpleDialogue dialogue;
    // Start is called before the first frame update
    public void TriggerDialogue()
    {
        SimpleDialogueManager.Instance.StartDialogue(dialogue);
    }

    public void SaveToJson()
    {
        string conversationData =JsonUtility.ToJson(dialogue);
        string filePath = Application.persistentDataPath + "/TestDialogueData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, conversationData);
        Debug.Log("SAVE methode was read");
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/TestDialogueData.json";
        string conversationData = System.IO.File.ReadAllText(filePath);
        dialogue = JsonUtility.FromJson<SimpleDialogue>(conversationData);
        Debug.Log("Data is loaded");

    }
}
