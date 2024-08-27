using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class newDialogueManager : MonoBehaviour
{
    public static newDialogueManager Instance;
    public List<Image> characterIconPosition = new List<Image>();
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    public Animator uiAnimator;
    [SerializeField]
    private Queue<dialogueObject> lines = new Queue<dialogueObject>();
    private Color passiveActorcolor = new Color(0.624f, 0.624f, 0.624f, 1f);
    private Color activeActorcolor = new Color(1f, 1f, 1f, 1f);
    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;

    private dialogueObject currentLine;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartDialogue(objectDialogue dialogue)
    {
       
        isDialogueActive = true;
        // animator.Play("show");
        lines.Clear();

        foreach (dialogueObject line in dialogue.dialogueLine)
        {
            lines.Enqueue(line);
        }
        uiAnimator.SetTrigger("trShow");
        
        

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();

        //int spriteIndex = 0;

        // dialogueObject ->character list
        //                ->Line
        //

        // characterList[x] -> actor
        //                  -> sprite
        //                  -> position
        //Check if the selected character index is in the scope of the sprite list.


        clearAllActor();
         
        // Set up :  set sprite and position  For : all actor
        SetUpAllActorSprite();

        //Set up : Name , Color , height position For : Main actor
        SetUpMainActor();

        //Move all sprite ir required.
        this.GetComponent<mngMovingActor>().HandleActorMouvement(currentLine, characterIconPosition);
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));

        
    }

    IEnumerator TypeSentence(dialogueObject dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);

        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;

        StartCoroutine(HideAllActorSprite());
        
        uiAnimator.ResetTrigger("trShow");
        
        uiAnimator.SetTrigger("trHide");
    }


    IEnumerator HideAllActorSprite()
    {
        //wait for the UI to be out of sight before hiding all actor just in case
        yield return new WaitForSeconds(0.5f);
        clearAllActor();

    }
    private void clearAllActor()
    {
        foreach (Image actor in characterIconPosition)
        {
            actor.sprite = null;
            actor.color = new Color(0, 0, 0, 0);

        }
 
    }

    private void SetUpAllActorSprite()
    {
        //Set all used caracter sprite in their proper position
        foreach (CaracterList actor in currentLine.charaterlist)
        {
            
            //Check if desired actor position is valid
            if (actor.actorPosition < 0 || actor.actorPosition >= characterIconPosition.Count)
            {
                Debug.Log("the targeted actor position is out of range");
                continue;
            }

            if (characterIconPosition[actor.actorPosition] == null)
            {
                Debug.Log("the targeted actor position does not exist");
                continue;
            }



            //Place the proper sprite
            if (actor.selectedSprite < actor.actor.characterSprite.Count && actor.selectedSprite >= 0)
            {

                characterIconPosition[actor.actorPosition].sprite = actor.actor.characterSprite[actor.selectedSprite];
            }
            else
            {
                //Using deffalut sprite to prevent problem
                characterIconPosition[actor.actorPosition].sprite = actor.actor.characterSprite[0];
                Debug.Log($"Sprite #({actor.selectedSprite}) of actor ( {actor.actor.name} ) was not found , using default sprite");

            }
            characterIconPosition[actor.actorPosition].color = passiveActorcolor;

        }
    }

    private void SetUpMainActor()
    {
        //place name of main character
        //check if main actor value is valid
        //Set it as the name to use

        var mainActorIndex = currentLine.mainActor;
        if (mainActorIndex < 0 || mainActorIndex >= currentLine.charaterlist.Count)
        {
            Debug.Log($"Main actor value {mainActorIndex} is not valid");
            characterName.text = "Invalid";
            return;
        }
        if (currentLine.charaterlist[mainActorIndex] == null)
        {
            Debug.Log("Selected actor is null");
            return;
        }


        //Set up visual for the current main character
        if (characterIconPosition[mainActorIndex].sprite != null)
        {
            Debug.Log("MainActor is null");
            characterIconPosition[mainActorIndex].color = activeActorcolor;
            characterIconPosition[mainActorIndex].transform.SetAsLastSibling();
            characterName.text = currentLine.charaterlist[mainActorIndex].actor.characterName;
        }
    }

    /*
    void HideAllActorSprite()
    {
        foreach(Image actor in characterIconPosition)
        {
            actor.color = new Color(0, 0, 0, 0);
        }
    }*/
}
