using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour {

    Vector2 touchStartPos;
    Vector2 direction;
    Vector2 motionDirection;
    Rigidbody2D rgd;
    public GameObject spike;

	// Use this for initialization
	void Start () {
        rgd = gameObject.GetComponent<Rigidbody2D>();
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
                touchStartPos = touchInput.position;
            else if (touchInput.phase == TouchPhase.Moved)
                direction = touchInput.position - touchStartPos;
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
        float velocityX = (spike.scrollSpeed) * motionDirection.y / motionDirection.x;
        Vector3 change = new Vector3(0, velocityX * Time.deltaTime, 0);
        transform.position += change;
    }

}
