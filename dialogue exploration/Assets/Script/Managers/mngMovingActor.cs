using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mngMovingActor : MonoBehaviour
{
    public GameObject leftOffScreenPosition;
    public GameObject rightOffscreenPosition;
    public int mouvementSpeed;

    [SerializeField]
    private Vector3 _targetPosition;
    [SerializeField]
    private Vector3 _direction;
    private int facingDirection;
   

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Calculate the current position and the targeted position
    //pass the info to the proper actor and  allow him to move
    /// <summary>
    /// Check all active actor for  requested mouvement. If needed, calculate the direction and distance requested and start  the mouvment of the actor.
    /// </summary>
    /// <param name="currentLine">The current (testingDialogue) object</param>
    /// <param name="characterPosition">List of all actor </param>
    public void HandleActorMouvement(dialogueObject currentLine, List<Image> characterPosition)
    {
        foreach (ActorList actor in currentLine.actorList)
        {
            Image actorObjectRef = characterPosition[actor.actorPosition];
            bool needToMove = false;
            if (actor.actorMouvement.walkOutLeft)
            {
                // characterPosition[actor.actorPosition].GetComponent<moveActor>().WalkOutLeft();
                WalkOutLeft(actorObjectRef.transform);
                needToMove = true;
                
            }
            else if (actor.actorMouvement.walkOutRight)
            {
                WalkOutRight(actorObjectRef.transform);
                needToMove = true;

            }else if(actor.actorMouvement.walkBackIn)
            {
                WalkBackInWithOfset(actorObjectRef.transform, actor.actorMouvement.moveToo);
                needToMove = true;
            }
            else if(actor.actorMouvement.moveToo != new Vector3(0, 0, 0))
            {
                WalkToSpecificPosition(actorObjectRef.transform, actor.actorMouvement.moveToo);
                needToMove = true;
            }

            if (actor.actorMouvement.moveBackward)
            {
                if(_direction.x > 0)
                {
                    facingDirection = 1;
                }
                else
                {
                    facingDirection = -1;
                }

            }
            else
            {
                if (_direction.x > 0)
                {
                    facingDirection = -1;
                }
                else
                {
                    facingDirection = 1;
                }

            }
            
           

            if (needToMove)
            {
                //if used here all right facing character will swap there facing direction when moving.
                if (actor.character.faceRightByDefault)
                {
                    facingDirection = facingDirection * -1;
                    Debug.LogWarning("In swaping right looking actor");
                }
                actorObjectRef.GetComponent<moveActor>().SetTargetPosition(_targetPosition);
                actorObjectRef.GetComponent<moveActor>().SetTargetDirection(_direction);
                actorObjectRef.GetComponent<moveActor>().SetMouvementSpeed(mouvementSpeed);
                actorObjectRef.GetComponent<moveActor>().SetFacingDirection(facingDirection);
                actorObjectRef.GetComponent<moveActor>().initialiseMouvement();
            }


            
        }
    }

    public void PlaceActorToDesiredPosition(dialogueObject currentLine, List<Image> characterPosition)
    {
        foreach (ActorList actor in currentLine.actorList)
        {
            Image actorObjectRef = characterPosition[actor.actorPosition];
            actorObjectRef.GetComponent<moveActor>().ReachFinalPosition();

        }
    }

    private void WalkOutLeft(Transform actorPosition)
    {
        _targetPosition = leftOffScreenPosition.transform.localPosition;
        // _targetPosition = actorPosition.InverseTransformPoint(leftOffScreenPosition.transform.position);
        Debug.Log("(patate) Target local position : "+_targetPosition + "Target world position: "+ leftOffScreenPosition.transform.position + " actor localPosition : " + actorPosition.position);
        _direction = _targetPosition - actorPosition.localPosition;
        _direction = _direction.normalized;
    }
    private void WalkOutRight(Transform actorPosition)
    {
        _targetPosition = rightOffscreenPosition.transform.localPosition;
        _direction = _targetPosition - actorPosition.localPosition;
        _direction = _direction.normalized;
    }

    private void WalkToSpecificPosition(Transform actorPosition, Vector3 positionOfset)
    {
        _targetPosition = actorPosition.localPosition + positionOfset;
        _direction = _targetPosition - actorPosition.localPosition;
        _direction = _direction.normalized;

    }

    private void WalkBackInWithOfset(Transform actorPosition, Vector3 positionOfset)
    {
        _targetPosition = actorPosition.localPosition + positionOfset;
        float distanceLeft = Vector3.Distance(actorPosition.localPosition, leftOffScreenPosition.transform.localPosition);
        float distanceRight = Vector3.Distance(actorPosition.localPosition, rightOffscreenPosition.transform.localPosition);

        if (distanceLeft < distanceRight) {
            actorPosition.localPosition = leftOffScreenPosition.transform.localPosition;
        }
        else
        {
            actorPosition.localPosition = rightOffscreenPosition.transform.localPosition;
        }

        _direction = _targetPosition - actorPosition.localPosition;
        _direction = _direction.normalized;
    }
}
