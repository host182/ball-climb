using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour {

    Vector2 touchStartPos;
    Vector2 direction;
    Vector2 motionDirection;
    Vector2 destination;
    bool isStart;                       //bool value to see if the game has started
    public GameObject spike;
    public GameObject wall;
    public GameObject directionDot;     //the dot sprite that shows the touch direction

	// Use this for initialization
	void Start () {
        motionDirection = new Vector2(1, 1);
        isStart = true;
        //directionDot.transform.localScale = new Vector3(0, 0, 0);
        //directionDot.transform.Translate(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        CalculateDirection();
        if (motionDirection.x == 0)
        {
            Destroy(gameObject);
            Debug.Log("Game Over");
        }
        else
            ////if (!isStart)
                Movement();
	}

    void CalculateDirection()
    {
        if(Input.touchCount>0)
        {
            Touch touchInput = Input.GetTouch(0);
            if (touchInput.phase == TouchPhase.Began)
            {
                touchStartPos = touchInput.position;
                Debug.Log("touch's start position - " + touchInput.position);
            }
            else if (touchInput.phase == TouchPhase.Moved)
            {
                direction = touchInput.position - touchStartPos;
                Debug.Log("Direction - " + direction);
                directionDot.transform.Translate(transform.position);
                directionDot.transform.Rotate(0, 0, Mathf.Atan2(direction.y, direction.x));
                directionDot.transform.localScale = new Vector3(10f, 10f, 10f);
            }
            else if(touchInput.phase==TouchPhase.Ended)
            {
                Debug.Log("Direction set");
                directionDot.transform.localScale = new Vector3(0, 0, 0);
                ////isStart = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    { 
        Debug.Log("Collision Occured");
        if (collision.gameObject.name == "Wall")
        {
            motionDirection = direction;
            direction = new Vector2(0, 0);
            //CalculateDestination();
        }
        else if (collision.gameObject.name == "Spike")
        {
            Destroy(gameObject);
            Debug.Log("Dead\n\n");
        }
    }

    void Movement()
    {
        ////float velocityX = ((spike.GetComponent<spike_script>()).scrollSpeed >) * motionDirection.y / motionDirection.x;
        float velocityX = 3 * motionDirection.y / motionDirection.x;
        Vector3 change = new Vector3(velocityX * Time.deltaTime, 0, 0);
        transform.position += change;
    }

    /*void CalculateDestination()
    {
        destination.y = (motionDirection.y / motionDirection.x) * Mathf.Abs(transform.position.x - wall.transform.position.x);
        destination.x = wall.transform.position.x;
    }*/
}
