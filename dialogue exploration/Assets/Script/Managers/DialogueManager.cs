using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using RichTextSubstringHelper;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public List<Image> actorIconPosition = new List<Image>();
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    public Animator uiAnimator;
    public RawImage background;
    [SerializeField]
    private Queue<dialogueObject> lines = new Queue<dialogueObject>();
    private Color passiveActorcolor = new Color(0.624f, 0.624f, 0.624f, 1f);
    private Color activeActorcolor = new Color(1f, 1f, 1f, 1f);
    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;
    private bool isTyping = false;
    
    [SerializeField]
    private dialogueObject currentLine;

    [SerializeField]
    private List<Vector3> originalActorsPosition = new List<Vector3>();

    public naratorControl naratorController;
    [Header("General control")]
    public bool highlightMainActor;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //SaveActorPosition();


        }
       
    }

    public void StartDialogue(objectDialogue dialogue)
    {
       if(isDialogueActive == true)
       {
            return;
       }


        isDialogueActive = true;
        // animator.Play("show");
        lines.Clear();
        if(originalActorsPosition.Count == 0)
        {
            SaveActorPosition();

        }
        else
        {
            ResetActorPosition();
        }

        foreach (dialogueObject line in dialogue.dialogueLine)
        {
            lines.Enqueue(line);
        }
        uiAnimator.SetTrigger("trShow");
        


        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {

        //Instantly finish typing the line.
        if(isTyping == true)
        {
            showFullDialogueText(currentLine);
            return;
        }

        // Make sure the actor reached their last position
        if(currentLine != null)
        {
            this.GetComponent<mngMovingActor>().PlaceActorToDesiredPosition(currentLine, actorIconPosition);
           // Debug.LogWarning(" Actor have reset position")
        }

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();
        currentLine.line = gameObject.GetComponent<loadSpecificTranslatedDialogue>().getTranslatedLine(currentLine.name);
       // Debug.Log(gameObject.GetComponent<loadSpecificTranslatedDialogue>().getTranslatedLine(currentLine.name));

        //Clean up before loading the new line
        clearAllActor();
        this.GetComponent<MngPlaySound>().StopAllSoundEffect();


        ChangeBackground();

        bool hideALLActor = currentLine.hideAllActor;
        
        if(naratorController != null)
        {
            if (currentLine.naration == true)
            {
                naratorController.deActivateNarator();
            }
            else
            {
                naratorController.activateNarator();
            }
        }

        // We dont need to check if actor are showing because we always hide/ reset all actor visibility and colore a  bit before.
        if(hideALLActor == false)
        {
            // Set up :  set sprite and position  For : all actor
            SetUpAllActorSprite();

        }

        //Set up : Name , Color , height position For : Main actor
        SetUpMainActor(hideALLActor);

        //Move all sprite ir required.
        this.GetComponent<mngMovingActor>().HandleActorMouvement(currentLine, actorIconPosition);
        
        StopAllCoroutines();

        // Start Coroutine to start typing the text.
        StartCoroutine(TypeSentence(currentLine));

        //Play all sound section
        this.GetComponent<MngPlaySound>().HandlePlaySounds(currentLine);

    }

    IEnumerator TypeSentence(dialogueObject dialogueLine)
    {
        isTyping = true;
        dialogueArea.text = "";
        //Debug.Log(StringExt.RichTextSubString(dialogueLine.line, 5));
        

        for(int i = 0; i <= StringExt.RichTextLength(dialogueLine.line); i++)
        {
            dialogueArea.text = StringExt.RichTextSubString(dialogueLine.line, i);
            //dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
            //i++;
            
        }

        isTyping = false;
        /*
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            
             * open = 0
             * close  = 0
             * active tag = 0
             *  leftstring;
             * middle string;
             * right string
             * openTag string
             * 
             * if char = \ skip to next char
             * if char = <
             *  then open ++
             *  active tag ++
             *  
             *  if( open != 0)
             *  add char to open
             * 
             * 
            dialogueArea.text = StringExt.RichTextSubString(dialogueLine.line, i);
            //dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
            i++;

            

        }
        */
    }

    private void showFullDialogueText(dialogueObject dialogueLine)
    {
        StopAllCoroutines();
        dialogueArea.text = dialogueLine.line;
        isTyping = false;
    }

    void EndDialogue()
    {
       

        StartCoroutine(HideAllActorSprite());
        
        uiAnimator.ResetTrigger("trShow");
        
        uiAnimator.SetTrigger("trHide");
        
    }


    IEnumerator HideAllActorSprite()
    {
        //wait for the UI to be out of sight before hiding all actor just in case
        yield return new WaitForSeconds(0.5f);
        clearAllActor();
        ResetActorPosition();
        isDialogueActive = false;


    }
    private void clearAllActor()
    {
        foreach (Image actor in actorIconPosition)
        {
            actor.sprite = null;
            
            actor.color = new Color(0, 0, 0, 0);

        }
 
    }

    private void SetUpAllActorSprite()
    {
        //Set all used caracter sprite in their proper position
        foreach (ActorList actor in currentLine.actorList)
        {
            
            //Check if desired actor position is valid
            if (actor.actorPosition < 0 || actor.actorPosition >= actorIconPosition.Count)
            {
                Debug.Log("the targeted actor position is out of range");
                continue;
            }

            if (actorIconPosition[actor.actorPosition] == null)
            {
                Debug.Log("the targeted actor position does not exist");
                continue;
            }



            //Place the proper sprite
            if (actor.selectedSprite < actor.character.characterSprite.Count && actor.selectedSprite >= 0)
            {

                actorIconPosition[actor.actorPosition].sprite = actor.character.characterSprite[actor.selectedSprite];
            }
            else
            {
                //Using deffalut sprite to prevent problem
                actorIconPosition[actor.actorPosition].sprite = actor.character.characterSprite[0];
                Debug.Log($"Sprite #({actor.selectedSprite}) of actor ( {actor.character.name} ) was not found , using default sprite");

            }

            if(highlightMainActor == false)
            {
                actorIconPosition[actor.actorPosition].color = activeActorcolor;
            }
            else
            {
                actorIconPosition[actor.actorPosition].color = passiveActorcolor;
            }

        }
    }

    private void SetUpMainActor(bool hideActor)
    {
        //place name of main character
        //check if main actor value is valid
        //Set it as the name to use

        var mainActorIndex = currentLine.mainActorPosition;

        // TO DO [I dont know if i fixed that]: BUG fix : currentl checking  if the mainActorIndex refer to an index and not the position of an actor.

        //Would need to loop trought all actor to find if one of them is designed as the main actor
        bool mainActorFound = false;
        foreach (ActorList actor in currentLine.actorList)
        {
            if (actor.actorPosition == mainActorIndex)
            {
               mainActorFound = true;
                break;
            }
        }
        if (mainActorIndex < 0 || mainActorFound == false)
        {
            Debug.Log($"Main actor value {mainActorIndex} is not valid");
            characterName.text = "Invalid";
            return;
        }


        //May not be required now 
        /*
        if (currentLine.actorList[mainActorIndex] == null)
        {
            Debug.Log("Selected actor is null");
            return;
        }*/


        //Set up visual for the current main character
        if (actorIconPosition[mainActorIndex].sprite != null)
        {
            
            if(hideActor == false)
            {
                actorIconPosition[mainActorIndex].color = activeActorcolor;
            }
            actorIconPosition[mainActorIndex].transform.SetAsLastSibling();
            foreach (ActorList actor in currentLine.actorList)
            { 
                if(actor.actorPosition == mainActorIndex)
                {
                    characterName.text = actor.character.name;
                    break;
                }
            }
        }
    }

    private void ResetActorPosition()
    {
        int index = 0;
        foreach (Image actor in actorIconPosition)
        {
            actor.transform.position = originalActorsPosition[index];
            actor.transform.localScale = Vector3.one;
            index++;
            

        }
    }
    private void SaveActorPosition()
    {
        foreach (Image actor in actorIconPosition)
        {
            originalActorsPosition.Add(actor.transform.position);
        }
    }

    private void ChangeBackground()
    {
        if(currentLine.newBackgroundImage == null)
        {
            return;
        }

        if(background == null)
        {
            Debug.Log("No background ellement selected");
        }

        background.texture = currentLine.newBackgroundImage;

    }
    /*
    void HideAllActorSprite()
    {
        foreach(Image actor in actorIconPosition)
        {
            actor.color = new Color(0, 0, 0, 0);
        }
    }*/
}


