using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActorMouvement
{
    
    public bool walkOutLeft;
    public bool walkOutRight;
    public Vector3 moveToo;
    public bool moveBackward;
}
[System.Serializable]
public class ActorList
{
    public CharacterDataScriptableObject character;
    public int selectedSprite;
    public int actorPosition;
    public ActorMouvement actorMouvement;
    public int actorSoundEffect = -1;
}
[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/DialogueScriptableObject", order = 1)]
public class dialogueObject : ScriptableObject
{
    // Start is called before the first frame update
    [Tooltip("The actor with this value in (ActorPosition) will be selected as the Main actor.")]
    public int mainActorPosition;
    public bool naration;
   // public int selectedSprite;
    [TextArea(3, 10)]
    public string line;
    public List <ActorList> actorList = new List<ActorList>();
    [Header("General control")]
    public bool hideAllActor;

    public bool playActorSoundEffect = false;
    public AudioClip dialogueSoundEffect = null;
    public AudioClip backgroundMusic = null;



}
