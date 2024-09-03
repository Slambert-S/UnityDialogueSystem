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
 
            float distence = Vector3.Distance(this.transform.position, targetPosition);
            if (distence >= 2)
            {
                transform.Translate(direction * speed * Time.deltaTime);
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
        targetPosition = newTargetPosition;
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
            this.transform.position = targetPosition;
            isMoving = false;
        }
    }
}
