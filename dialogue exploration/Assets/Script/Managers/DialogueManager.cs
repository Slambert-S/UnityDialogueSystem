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
    private Color activeActorcolor = new Color(1f, 1f, 1f, 0f);

    private List<Image> fadingOutList = new List<Image>();
    private List<Image> fadingInList = new List<Image>();

    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;
    private bool isTyping = false;
    private IEnumerator typingCoroutine;

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

        //The saved actor position and reset is used to move all character position  back to their original position. 
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


            HelperFinishAllAnimation();

            return;
        }

        //Not sure if this will be useful in the furutre. [03/10/2024]
        // This check is use to quickly finish the animation if they are still going when the text is done typing and the user try to load the next line [03/10/2024]
        // Make sure the actor reached their last position
        if (currentLine != null)
        {
            HelperFinishAllAnimation();
        }

        

        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();
        currentLine.line = gameObject.GetComponent<loadSpecificTranslatedDialogue>().getTranslatedLine(currentLine.name);
       

        //Clean up before loading the new line
        StopAllCoroutines();
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

        //Move all sprite if required.
        this.GetComponent<mngMovingActor>().HandleActorMouvement(currentLine, actorIconPosition);


        // Start Coroutine to start typing the text.
        typingCoroutine = TypeSentence(currentLine);
        StartCoroutine(typingCoroutine);

        //Play all sound section
        this.GetComponent<MngPlaySound>().HandlePlaySounds(currentLine);

    }

    private void HelperFinishAllAnimation()
    {
        this.GetComponent<mngMovingActor>().PlaceActorToDesiredPosition(currentLine, actorIconPosition);
        FinishAllChildFadeOUt();
        FinishAllFadOperation();
    }

    private void showFullDialogueText(dialogueObject dialogueLine)
    {
        StopCoroutine(typingCoroutine);
        dialogueArea.text = dialogueLine.line;
        isTyping = false;
    }

    void EndDialogue()
    {
       

        StartCoroutine(HideAllActorSprite());
        
        uiAnimator.ResetTrigger("trShow");
        
        uiAnimator.SetTrigger("trHide");
        
    }


    
    private void clearAllActor()
    {
        foreach (Image actor in actorIconPosition)
        {
            //actor.sprite = null;
            
            actor.color = new Color(0, 0, 0, 0);
/*
            foreach(Transform child in actor.gameObject.transform)
            {
                
                Destroy(child.gameObject);
            }
*/
        }
 
    }

    private void FinishAllFadOperation()
    {
        foreach(Image objectToFadeOut in fadingOutList)
        {
            SetImageAlpha(objectToFadeOut, 0f);
        }
         fadingOutList.Clear();

        foreach (Image objectToFadeIn in fadingInList)
        {
            SetImageAlpha(objectToFadeIn, 1f);
        }
        fadingInList.Clear();

    }

    private void SetImageAlpha(Image targetedImage, float alphaValue)
    {
      
        Color tagetcolor = targetedImage.color;
        tagetcolor.a = alphaValue;
        targetedImage.color = tagetcolor;
    }
    private void FinishAllChildFadeOUt()
    {
        foreach (Image actor in actorIconPosition)
        {
            foreach (Transform child in actor.gameObject.transform)
            {

                Destroy(child.gameObject);
            }

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
                
                //Test duplicating a sprite
                GameObject copy = Instantiate(actorIconPosition[actor.actorPosition].gameObject, actorIconPosition[actor.actorPosition].transform.position , Quaternion.identity);
           
                copy.transform.SetParent(actorIconPosition[actor.actorPosition].transform);
                copy.transform.localScale = new Vector3(1, 1, 1);
                copy.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);

                //Debug.Log(actorIconPosition[actor.actorPosition].sprite);

                StartCoroutine(FadeOut(copy.GetComponent<Image>(), actor, true));
                
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

            if (actor.fadeIn == true)
            {
                StartCoroutine(FadeIn(actorIconPosition[actor.actorPosition], actor));
            }
            else
            {
                //Ass actor sprite have their Alpha set to zero to never show unwanted sprite.
                //This line is required to show all active actor
                SetImageAlpha(actorIconPosition[actor.actorPosition], 1f);
              
            }

            if (actor.fadeOut == true)
            {
                StartCoroutine(FadeOut(actorIconPosition[actor.actorPosition], actor, false));
                foreach(Transform  child in actorIconPosition[actor.actorPosition].transform)
                {
                    if(child.gameObject.GetComponent<Image>() != null)
                    {
                        SetImageAlpha(child.gameObject.GetComponent<Image>() , 0);
                    }
                }
            }



        }
    }

  
    private void SwapColorKeepAlpha(Image actorToChangeColor , Color colorToChangeTo)
    {
        Image yourSpriteRenderer = actorToChangeColor.GetComponent<Image>();
        float alpha = yourSpriteRenderer.color.a;
        colorToChangeTo.a = alpha;
        yourSpriteRenderer.color= colorToChangeTo;
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
               // actorIconPosition[mainActorIndex].color = activeActorcolor;
                SwapColorKeepAlpha(actorIconPosition[mainActorIndex], activeActorcolor);
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

   public IEnumerator TypeSentence(dialogueObject dialogueLine)
    {
        isTyping = true;
        dialogueArea.text = "";
        //Debug.Log(StringExt.RichTextSubString(dialogueLine.line, 5));


        for (int i = 0; i <= StringExt.RichTextLength(dialogueLine.line); i++)
        {
            dialogueArea.text = StringExt.RichTextSubString(dialogueLine.line, i);
            //dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
            //i++;

        }

        isTyping = false;

    }

    //This debug section work on controlling the alpha of an actor
    //Testing to control alpha level
    IEnumerator FadeIn(Image actorVisalFade, ActorList actor)
    {

        //Debug.Log("In fade in");
        Image yourSpriteRenderer = actorVisalFade.GetComponent<Image>();
        float alphaVal = yourSpriteRenderer.color.a;
        Color tmp = yourSpriteRenderer.color;
        
        //Add the Image to the list to keep track of it if we need to finishe the fade early
        fadingInList.Add(yourSpriteRenderer);

        while (yourSpriteRenderer != null && yourSpriteRenderer.color.a < 1f)
        { 
            alphaVal += 0.1f;
            tmp.a = alphaVal;
            yourSpriteRenderer.color = tmp;

            yield return new WaitForSeconds(0.05f); // update interval
        }

        fadingInList.Remove(yourSpriteRenderer);
        fadingInList.RemoveAll(item => item == null);
    }
    IEnumerator FadeOut(Image actorVisalFade, ActorList actor, bool destroyAtTheEnd)
    {
        //Debug.Log("In fade out");
        Image yourSpriteRenderer = actorVisalFade.GetComponent<Image>();
        Color TempToMaxAlpha = actorVisalFade.GetComponent<Image>().color;
        TempToMaxAlpha.a = 1f;
        yourSpriteRenderer.color = TempToMaxAlpha;

        fadingOutList.Add(yourSpriteRenderer);

        float alphaVal = yourSpriteRenderer.color.a;
        Color tmp = yourSpriteRenderer.color;

        while (yourSpriteRenderer != null && yourSpriteRenderer.color.a > 0f)
        {
            
         
            alphaVal -= 0.1f;
            tmp.a = alphaVal;
            yourSpriteRenderer.color = tmp;
         
            yield return new WaitForSeconds(0.05f); // update interval
        }

        fadingOutList.Remove(yourSpriteRenderer);
        fadingOutList.RemoveAll(item => item == null);

        if (destroyAtTheEnd == true)
        {
            if(actorVisalFade != null)
            {
                Destroy(actorVisalFade.gameObject);
            }
        }
    }

    IEnumerator HideAllActorSprite()
    {
        //wait for the UI to be out of sight before hiding all actor just in case
        yield return new WaitForSeconds(0.5f);
        clearAllActor();
        ResetActorPosition();
        isDialogueActive = false;


    }

}


