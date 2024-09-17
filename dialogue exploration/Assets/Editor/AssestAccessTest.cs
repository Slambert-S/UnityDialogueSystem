using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssestAccessTest 
{

    public static List<GameObject> GetAllGameObjectsInFolder(string folderPath)
    {
        // Prepare the list to store the GameObject references
        List<GameObject> gameObjects = new List<GameObject>();

        // Get the GUIDs for all assets in the specified folder
        string[] assetGuids = AssetDatabase.FindAssets("t:GameObject", new[] { folderPath });

        // Loop through each asset GUID and load the corresponding GameObject
        foreach (string guid in assetGuids)
        {
            // Get the asset path from the GUID
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            // Load the GameObject at this path
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

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

}


