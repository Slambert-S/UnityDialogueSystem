using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdDialogueCharacter
{
    public CharacterDataScriptableObject characterData;
    public int characterSprite;
}

[System.Serializable]
public class IdDialogueLine
{
    public IdDialogueCharacter character;
    [TextArea(3, 10)]
    public string line;

}

[System.Serializable]
public class IdDialogue
{
    public string id;
    public List<IdDialogueLine> dialogueLine = new List<IdDialogueLine>();
}
[System.Serializable]
public class IdDialogueList
{
    public List<IdDialogue> dialogues = new List<IdDialogue>();
}
public class dialogueWithIDTesting : MonoBehaviour
{
    public IdDialogueList dialoguesList;
    // Start is called before the first frame update
    public void SaveToJson()
    {
        string conversationData = JsonUtility.ToJson(dialoguesList);
        string filePath = Application.persistentDataPath + "/TestDialogueData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, conversationData);
        Debug.Log("SAVE methode was read");
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/TestDialogueData.json";
        string conversationData = System.IO.File.ReadAllText(filePath);
        dialoguesList = JsonUtility.FromJson<IdDialogueList>(conversationData);
        Debug.Log("Data is loaded");

    }

    public void Start()
    {
        LoadFromJson();
    }
}
