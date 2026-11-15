using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RollBall : MonoBehaviour
{

    Rigidbody ballBody;

    InputAction bowlButt;

    bool holdingButt;
    bool roll;
    bool finishedRoll;

    public float rollPower;

    public LayerMask pinLayer;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private bool resetGame = false;

    GameObject[] pins; 

    public GameObject dirArrow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballBody = GetComponent<Rigidbody>();
        bowlButt = InputSystem.actions.FindAction("Bowl");
        startPosition = transform.position;
        startRotation = transform.rotation;

        //creating an array by finding all GameObjects with the Pin tag
        pins = GameObject.FindGameObjectsWithTag("Pin");
    }

    // Update is called once per frame
    void Update()
    {
        //checking if ball has rolled and if the game is resetting -> both coming back as false allows the ball to be aimed/thrown
        if(!finishedRoll && !resetGame)
        {
            if (bowlButt.WasPressedThisFrame())
            {
                //if pressing button -> want to aim, add power

                holdingButt = true;
                dirArrow.SetActive(true);
            }
            else if (bowlButt.WasReleasedThisFrame())
            {
                //if button release -> roll ball

                holdingButt = false;
                roll = true;
                dirArrow.SetActive(false);
                finishedRoll = true;
            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame) //Lets reset the game :)
        {
            BallReset();
        }
    }

    private void FixedUpdate()
    {
        if(holdingButt && ballBody.linearVelocity == Vector3.zero)
        {
            //find direction mouse is pointing
            Debug.DrawRay(transform.position, FindDirection() * 1000f, Color.cyan);
        }

        if (roll)
        {
            //roll based on mouse direction
            StartRoll();
        }
    }

    void StartRoll()
    {
        if(Physics.Raycast(transform.position, FindDirection(), 1000f, pinLayer) == true)
        {
            ballBody.isKinematic = false;
            ballBody.AddForce(rollPower * transform.TransformDirection(FindDirection()), ForceMode.Impulse);
            roll = false;
        }
    }

    Vector3 FindDirection()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = 10f;

        Vector3 direction = Camera.main.ScreenToWorldPoint(mousePosition);
        direction.z = 10f;

        return direction.normalized;
    }

    public void BallReset() //Same as ResetPins in PinManager -> restoring to orginal position values and setting all booleans back to starting state (aka false for all)
    {
        ballBody.isKinematic = true;

        ballBody.linearVelocity = Vector3.zero;
        ballBody.angularVelocity = Vector3.zero;

        ballBody.position = startPosition;
        ballBody.rotation = startRotation;

        roll = false;
        holdingButt = false;
        finishedRoll = false;

        PinsReset();
    }

    public void PinsReset() //Running PinManagers ResetPin Method -> for every pin in the array, they will reset until the array is over
    {
        foreach (GameObject pin in pins)
        {
            pin.GetComponent<PinManager>().ResetPin();
        }
        PinManager.score = 0;
    }

    private void OnCollisionEnter(Collision collision) //Checking if the ball has collided with either the Gutter or a Pin
    {
        //Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (finishedRoll && !resetGame)
        {
            if (collision.gameObject.CompareTag("Pin") || collision.gameObject.CompareTag("Gutter"))
            {
                //Debug.Log("Ball Hit something");
                StartCoroutine(GameResetAfterDelay());
            }
        }
    }

    IEnumerator GameResetAfterDelay() //A coroutine to reset the game stage after a set amount of time (in this cause 6.5 second delay)
    {
        resetGame = true;
        yield return new WaitForSeconds(6.5f);
        BallReset();
        resetGame = false;
    }
}
