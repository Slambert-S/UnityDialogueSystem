using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]

public class DialogueTranslation 
{
    public string key;
    public string englishText;
    public string frenchText;
}

[CreateAssetMenu(fileName = "Translation", menuName = "ScriptableObjects/TranslatedLine", order = 1)]
public class testingEditor : ScriptableObject
{
   
    public List<dialogueObject> listOfObject = new List<dialogueObject>() ;
    public List<DialogueTranslation> translationBook = new List<DialogueTranslation>() ;
   
    public void updateList(List<dialogueObject> list)
    {
        listOfObject = list;

        foreach (dialogueObject obj in listOfObject)
        {
            DialogueTranslation temp = new DialogueTranslation();
            temp.key = obj.name;
            temp.englishText = obj.line;
            translationBook.Add(temp);
        }
    }

    
}


#if UNITY_EDITOR
[CustomEditor(typeof(testingEditor))]
public class editorRegroupDialogue : Editor
{
    private SerializedProperty listOfObject;

    private void OnEnable()
    {
        listOfObject = serializedObject.FindProperty("testingThigsOUt");
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        testingEditor potato = (testingEditor)target;
        
        if (GUILayout.Button("do the thing", GUILayout.Width(90f)))
        {

            string folderPath = "Assets/Script/scriptable Object/dialogue Line/DemoScene_1";
            Debug.Log("do the thing button pressed");
            potato.updateList(GetAllGameObjectsInFolder(folderPath));
           // patate = GetAllGameObjectsInFolder(folderPath);   

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











