using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveActor : MonoBehaviour
{
    public GameObject tempLeftOffPosition;
    [SerializeField]
    private bool isMoving = false;
    private int speed = 400;
    [SerializeField]
    private Vector3 targetPosition;
    private Vector3 direction;

    private void Start()
    {
        targetPosition = this.transform.position;
    }
    void Update()
    {
        if (isMoving)
        {
            Debug.Log("Actor : " + this.transform.localPosition + " Target : " + targetPosition);
            float distence = Vector3.Distance(this.transform.localPosition, targetPosition);
            if (distence >= 20)
            {
                //Using Space.Self to scale the mouvement to actore size.
                //transform.Translate(direction * speed * Time.deltaTime, Space.Self);
                // Move our position a step closer to the target.
                var step = speed * Time.deltaTime; // calculate distance to move
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, step);
            }
            else 
            {
                isMoving = false;
                //Todo Reset mouvement target;
            }
        }

    }


   
    
    public void SetTargetPosition(Vector3 newTargetPosition)
    {
        //targetPosition = transform.InverseTransformPoint(newTargetPosition);
        targetPosition =  newTargetPosition;
    }
    public void SetTargetDirection(Vector3 newTargetDirection)
    {
        direction = newTargetDirection;
    }
    public void SetMouvementSpeed(int newSpeed)
    {
        speed = newSpeed;
    }
    public void SetFacingDirection(float facingDirection){
        this.transform.localScale = new Vector3(facingDirection,1,1);
    }
    public void initialiseMouvement()
    {
        isMoving = true;
        Debug.Log("Debug : in initialise  mouvement");
    }

    public void ReachFinalPosition()
    {
        if (isMoving)
        {
            this.transform.localPosition = targetPosition;
            isMoving = false;
        }
    }
}
