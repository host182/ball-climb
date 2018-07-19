using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour {

    Vector2 touchStartPos;
    Vector2 direction;
    Vector2 motionDirection;
    public GameObject spike;

	// Use this for initialization
	void Start () {
        motionDirection = new Vector2(1, 1);
	}
	
	// Update is called once per frame
	void Update () {
        CalculateDirection();
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
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Wall")
        {
            motionDirection = direction;
            direction = new Vector2(0, 0);
        }
        else if (collision.gameObject.name == "Spike")
        {
            Destroy(gameObject);
            Debug.Log("Dead\n\n");
        }

    }

    void Movement()
    {
        //float velocityX = ((spike.GetComponent<spike_script>()).scrollSpeed >) * motionDirection.y / motionDirection.x;
        float velocityX = 3 * motionDirection.y / motionDirection.x;
        Vector3 change = new Vector3(velocityX * Time.deltaTime, 0, 0);
        transform.position += change;
    }

}
