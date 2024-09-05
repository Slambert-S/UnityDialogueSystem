using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDialogueManager : MonoBehaviour
{
    public static SimpleDialogueManager Instance;
    // Start is called before the first frame update

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<SimpleDialogueLine> lines = new Queue<SimpleDialogueLine>();

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void StartDialogue(SimpleDialogue dialogue)
    {
        isDialogueActive = true;
       // animator.Play("show");
        lines.Clear();

        foreach (SimpleDialogueLine Dialogueline in dialogue.dialogueLine)
        {
            lines.Enqueue(Dialogueline);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if(lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        SimpleDialogueLine currentLine = lines.Dequeue();

        int spriteIndex = 0;

        //Check if the selected character index is in the scope of the sprite list.
        if(currentLine.character.characterSprite < currentLine.character.characterData.characterSprite.Count)
        {
            if(currentLine.character.characterSprite >= 0)
            {
                spriteIndex = currentLine.character.characterSprite;
            }
        }
        characterIcon.sprite = currentLine.character.characterData.characterSprite[spriteIndex];
        //characterIcon.transform.SetAsLastSibling();
        characterName.text = currentLine.character.characterData.name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(SimpleDialogueLine dialogueLine)
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
        //animator.Play("hide");
    }
}
