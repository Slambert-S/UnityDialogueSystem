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
public class CaracterList
{
    public CharacterDataScriptableObject actor;
    public int selectedSprite;
    public int actorPosition;
    public ActorMouvement actorMouvement;
}
[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/DialogueScriptableObject", order = 1)]
public class dialogueObject : ScriptableObject
{
    // Start is called before the first frame update
    public int mainActor;
   // public int selectedSprite;
    [TextArea(3, 10)]
    public string line;
    public List <CaracterList> charaterlist = new List<CaracterList>();

   

}
