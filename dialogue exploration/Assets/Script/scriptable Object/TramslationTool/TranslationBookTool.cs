using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]

public class TranslatedEntry
{
    public string key;
    public string englishText;
    public string frenchText;
}

[System.Serializable]
public class TempStep
{
      public List<TranslatedEntry> translationDico = new List<TranslatedEntry>();
}

[CreateAssetMenu(fileName = "TranslationBook", menuName = "ScriptableObjects/TranslationBook", order = 1)]
public class TranslationBookTool  : ScriptableObject
{
    public string filePath;
    public List<dialogueObject> listOfScriptableObjectDialogue = new List<dialogueObject>();
    public List<TranslatedEntry> translationDictionary = new List<TranslatedEntry>();

    public TempStep testingStep;

    public void updateList(List<dialogueObject> dialogueList)
    {
        listOfScriptableObjectDialogue = dialogueList;
        translationDictionary.Clear();
        foreach (dialogueObject obj in listOfScriptableObjectDialogue)
        {
            TranslatedEntry temp = new TranslatedEntry();
            temp.key = obj.name;
            temp.englishText = obj.line;
            testingStep.translationDico.Add(temp);
            //translationDictionary.Add(temp);
        }

        
    }

    public void SaveToJson()
    {
        string conversationData = JsonUtility.ToJson(testingStep,true);
        string localfilePath = Application.persistentDataPath + "/TestDialogueData.json";
        Debug.Log(localfilePath);
        System.IO.File.WriteAllText(localfilePath, conversationData);
        Debug.Log("SAVE methode was read");
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(TranslationBookTool))]
public class editorGatherAllDialogue : Editor
{
    private SerializedProperty listOfObject;
    private SerializedProperty filePathString;

    private void OnEnable()
    {
        listOfObject = serializedObject.FindProperty("testingThigsOUt");
        filePathString = serializedObject.FindProperty("filePath");    
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TranslationBookTool translationBookTarget = (TranslationBookTool)target;
        string newFilePath = filePathString.stringValue;
        if (GUILayout.Button("do the thing", GUILayout.Width(90f)))
        {

           
            Debug.Log("do the thing button pressed");
            translationBookTarget.updateList(GetAllGameObjectsInFolder(newFilePath));
            
        };

        if (GUILayout.Button("Save to JSON", GUILayout.Width(90f)))
        {

            translationBookTarget.SaveToJson();

        };
    }

    public static List<dialogueObject> GetAllGameObjectsInFolder(string folderPath)
    {
        // Prepare the list to store the GameObject references
        List<dialogueObject> gameObjects = new List<dialogueObject>();

        // Get the GUIDs for all assets in the specified folder
        string[] assetGuids = AssetDatabase.FindAssets("t:dialogueObject", new[] { folderPath });

        // Loop through each asset GUID and load the corresponding GameObject
        foreach (string guid in assetGuids)
        {
            // Get the asset path from the GUID
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            // Load the GameObject at this path
            dialogueObject obj = AssetDatabase.LoadAssetAtPath<dialogueObject>(assetPath);

            // Add the GameObject to the list if it exists
            if (obj != null)
            {
                gameObjects.Add(obj);
                Debug.Log("GameObject found: " + obj.name + " at " + assetPath);
            }
        }

        // Return the list of GameObjects
        return gameObjects;
    }

    //Debug.log("doing this here");
}
#endif