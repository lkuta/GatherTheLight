using UnityEngine;
using System.Collections;

//Adding this allows us to access members of the UI namespace including Text.
using UnityEngine.UI;

public class CompletePlayerController : MonoBehaviour
{

    public float speed;             //Floating point variable to store the player's movement speed.
    public Text countText;          //Store a reference to the UI Text component which will display the number of pickups collected.

    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    public int count;              //Integer to store the number of pickups collected so far.
    public int hitFinish = 0;       //Determines if the fairy collides with the exit

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();

        //Initialize count to zero.
        count = 0;

        //Call our SetCountText function which will update the text with the current value for count.
        SetCountText();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        // Should stop the sprite from rotating when hitting an object:
        rb2d.freezeRotation = true;

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.AddForce(movement * speed);
    }

    //OnTriggerEnter2D is called whenever this object overlaps with a trigger collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        if (other.gameObject.CompareTag("PickUp")) {
            Debug.Log("in Pickup");
            //... then set the other object we just collided with to inactive.
            other.gameObject.SetActive(false);

            //Add one to the current value of our count variable.
            count = count + 1;

            //Update the currently displayed count by calling the SetCountText function.
            SetCountText();

            //Add a light to the player object:
            Light playerLight = GetComponent<Light>();
            playerLight.color = new Color32(79, 222, 230, 0);
            playerLight.intensity = 1;
            playerLight.range = playerLight.range + (float)0.25;

            // Update Background color:
            SpriteRenderer back = GameObject.Find("Background").GetComponent<SpriteRenderer>();
            Color32 currentColor = back.color;
            back.color = new Color32((byte)((int)currentColor.r + 7), (byte)((int)currentColor.g + 7), (byte)((int)currentColor.b + 7), currentColor.a);

            // Update player sprite color:
            SpriteRenderer fairy = GameObject.Find("Player").GetComponent<SpriteRenderer>();
            Color32 currentfColor = fairy.color;
            fairy.color = new Color32((byte)((int)currentfColor.r + 7), (byte)((int)currentfColor.g + 7), (byte)((int)currentfColor.b + 7), currentfColor.a);
        }

     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the other object hit is tagged with "Finish," and if so, then send a nice variable over to the game Controller aka. TimeController
        if (collision.gameObject.CompareTag("Finish")) {
            timeController time = GameObject.Find("timeGameObject").GetComponent<timeController>();
            if(time.timeRemaining >= 0) {
                hitFinish = 1;
            }

        }
    }

        //This function updates the text displaying the number of objects we've collected and displays our victory message if we've collected all of them.
        void SetCountText()
    {
        //Set the text property of our our countText object to "Count: " followed by the number stored in our count variable.
        countText.text = "Count: " + count.ToString();
    }
}
