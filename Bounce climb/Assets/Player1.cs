using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tasks to improve/complete :
/// 1. set input restrictions - DONE
/// 2. provide death animation instead of Destroy(gameobject)
/// 3. change dot from image to multiple circle sprites
/// </summary>

public class Player1 : MonoBehaviour {

    Vector2 touchStartPos;              //position where the touch began
    Vector2 queuedDirection;
    Vector2 motionDirection;
    Vector2 destination;                //point on the wall that the ball will hit when the ball collidies
    bool isStart;                       //bool value to seex if the game has started - game starts after the user inputs a touch swipe and releases the hand
    int leftOrRight;                   //left motion is 0 and right motion is 1 (coded as to make this variable store only 0 and 1)
    public float distanceFromOriginToWallCollider;  //shortest distance from walls' collider to origin (i.e. positive x-coordinate of either collider)
    public GameObject spike;
    public GameObject directionDot;     //the dot sprite that shows the touch direction

	// Use this for initialization
	void Start () {
        motionDirection = new Vector2(1, 1);       //this value will not matter as the user input will always overwrite this variable before movement
        isStart = true;
        directionDot.transform.localScale = new Vector3(0, 0, 0);   //the dots are invisible until the second input is given
    }
	
	// Update is called once per frame
	void Update () {
        UpdateDestination();
        CalculateDirection();
        if (!isStart)                       //only start moving when the user has input the direction
            Movement();
	}

    void CalculateDirection()
    {
        if(Input.touchCount>0)
        {
            Touch touchInput = Input.GetTouch(0);
            if (touchInput.phase == TouchPhase.Began)
            {
                touchStartPos = touchInput.position;                       //register the begining of a swipe
                Debug.Log("touch's start position - " + touchInput.position);
            }
            else if (touchInput.phase == TouchPhase.Moved)                  //input direction can be changed until the the user disjoints the touch
            {
                queuedDirection = touchInput.position - touchStartPos;      //calculate direction vector
                queuedDirection = InputRestriction(queuedDirection);
                Debug.Log("Direction - " + queuedDirection);
                //changing dot transform information but not in the first input
                if (!isStart)
                {
                    float zAngle = Mathf.Atan2(queuedDirection.y, queuedDirection.x);
                    directionDot.transform.position = new Vector3(destination.x, destination.y, 0);
                    directionDot.transform.eulerAngles = new Vector3(0, 0, zAngle * Mathf.Rad2Deg);
                    directionDot.transform.localScale = new Vector3(2, 1, 1);
                }
            }
            else if(touchInput.phase==TouchPhase.Ended)                        //input is finalised in this stage
            {
                Debug.Log("Direction set to " + queuedDirection);
                directionDot.transform.localScale = new Vector3(0, 0, 0);       //when the input is complete, the dots vanish - the dots are
                                                                                //only shown to make it easier for the user to make decisions
                 
                //call pseudo OnCollisionEnter2D function when the first input is provided
                if (isStart)
                {
                    motionDirection = queuedDirection;
                    leftOrRight = (int)(0.5 + Mathf.Sign(motionDirection.x));       //value is 0 when moving to the left and 1 when moving to the right
                    queuedDirection = new Vector2(-motionDirection.x, motionDirection.y);
                    CalculateDestination();
                    isStart = false;                                  //once the first input is given, the starting phase is over
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Occured");
        if (collision.gameObject.name == "Wall") 
        {
            motionDirection = queuedDirection;                  //once collision with wall occures, the ball's direction 
                                                                //is set to be the previously queued direction
            queuedDirection = new Vector2(-motionDirection.x, motionDirection.y);       //default value for queuedDirection is the angle of
                                                                                        //the motion when the coefficient of restitution is 1
            CalculateDestination();
            leftOrRight = (leftOrRight + 1) % 2;                //changes value from 0 to 1 or vice versa when changing the direction of motion
        }
        else if (collision.gameObject.name == "Spike")
        {
            Destroy(gameObject);
            Debug.Log("Dead\n\n");
        }
    }

    void Movement()
    {
        ////float velocityX = ((spike.GetComponent<spike_script>()).scrollSpeed) * motionDirection.y / motionDirection.x;
        float velocityX = 1 * motionDirection.x / motionDirection.y; ;       //uncomment the above line and comment this line before the game is finished
        Vector3 change = new Vector3(velocityX * Time.deltaTime, 0, 0);
        transform.position += change;
    }

    void CalculateDestination()
    {
        //the formula below is a forced formula that I came up with. It will work on our cases but not in every hypothetical cases
        destination.y = transform.position.y + Mathf.Abs((motionDirection.y / motionDirection.x) * (distanceFromOriginToWallCollider + Mathf.Abs(transform.position.x)));
        destination.x = distanceFromOriginToWallCollider * motionDirection.x / Mathf.Abs(motionDirection.x);    //if the direction is obtuse, the destination will be negative
        Debug.Log("Destination - " + destination);
    }

    void UpdateDestination()
    {
        ////destination.y -= (spike.GetComponent<spike_script>().scrollSpeed) * Time.deltaTime;
        destination.y -= 1 * Time.deltaTime;                                //same instruction as the comment in Movement()
    }


    //if the restricting angles are to be changed, change angles in the conditional statements - (10 and 80)
    //but the change should be symmetric around x=y line
    //i.e 45-35=10 and 45+35=80
    //else, the code needs to be changed
    Vector2 InputRestriction(Vector2 input)
    {
        if (Mathf.Atan2(input.y, input.x) < ((90 * leftOrRight + 10) * Mathf.Deg2Rad))
        {
            Vector2 returningVector = new Vector2(1, Mathf.Tan((90 * leftOrRight + 10) * Mathf.Deg2Rad));
            return returningVector;
        }
        else if (Mathf.Atan2(input.y, input.x) > ((90 * leftOrRight + 80) * Mathf.Deg2Rad))
        {
            Vector2 returningVector = new Vector2(1, Mathf.Tan((90 * leftOrRight + 80) * Mathf.Deg2Rad));
            return returningVector;
        }
        else
            return input;
    }
}