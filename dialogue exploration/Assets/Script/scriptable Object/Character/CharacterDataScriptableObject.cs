using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/CharacterScriptableObject", order = 1)]
public class CharacterDataScriptableObject : ScriptableObject
{
    public string characterName;
    public List<Sprite> characterSprite;
    public bool faceRightByDefault;
    public List<AudioClip> audioClipList;
}
