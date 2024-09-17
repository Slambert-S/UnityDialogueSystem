using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class loadSpecificTranslatedDialogue : MonoBehaviour
{
    public enum Languages { English,French}
    public Languages selectedLanguages;
    private Dictionary<string, TranslatedEntry> translation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getTranslatedLine(string key)
    {
        
        if(translation == null)
        {
            translation = LoadTranslation();
            //
        }
        string translatedLine = "pre-translation";
        switch (selectedLanguages)
        {
            case Languages.English:
                translatedLine = translation[key].englishText;
                break;
            case Languages.French:
                translatedLine = translation[key].frenchText;
                break;
            default:
                break;
        }
                  
        return translatedLine;
    }

    public static Dictionary<string, TranslatedEntry> LoadTranslation()
    {
        // Load the JSON file from the Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>("DialogueDataTranslated");

        // Deserialize the JSON data into a TranslationDictionary object
        TempStep dictionary = JsonUtility.FromJson<TempStep>(jsonFile.text);

        // Create a dictionary for faster lookup by key
        Dictionary<string, TranslatedEntry> translationMap = new Dictionary<string, TranslatedEntry>();

        foreach (TranslatedEntry entry in dictionary.translationDico)
        {
            translationMap[entry.key] = entry;
        }

        return translationMap;
    }
}
