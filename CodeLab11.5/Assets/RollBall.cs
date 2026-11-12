using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class RollBall : MonoBehaviour
{
    Rigidbody rigidBody;
    private InputAction bowlButton;

    private bool holdingButton;
    private bool roll;
    private bool hasRolled;

    [SerializeField] private float rollPower;
    [SerializeField] private float aimRange = 0.5f;


    private Vector3 startPosition;
    private Quaternion startRotation;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        bowlButton = InputSystem.actions.FindAction("Bowl");
        //rigidBody.AddForce(transform.forward * 1000f);
        startPosition = transform.position;
        startRotation = transform.rotation;


    }

    // Update is called once per frame
    void Update()
    {
        if (!hasRolled && !resetting)
        {
            if (bowlButton.WasPressedThisFrame())
            {
                holdingButton = true;
            }
            else if (bowlButton.WasReleasedThisFrame())
            {
                holdingButton = false;
                roll = true;
            }
        }

            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                ResetBall();
            }
    }

    void FixedUpdate()
    {
        if (holdingButton && rigidBody.linearVelocity == Vector3.zero)
        {
            //find the direction the mouse is pointing towards
            Debug.DrawRay(transform.position, FindDirection() * 1000f, Color.white);
        }

        if (roll)
        {
            StartRoll();
        }
    }

    void StartRoll()
    {

        rigidBody.isKinematic = false;
        rigidBody.AddForce(FindDirection() * rollPower, ForceMode.Impulse);

        roll = false;
        hasRolled = true;

    }

    Vector3 FindDirection()
    {
        float mouseX = Mouse.current.position.ReadValue().x;
        float screenWidth = Screen.width;

        // Convert mouse X to a value between -1 and 1
        float x = (mouseX / screenWidth) * 2f - 1f;

        Vector3 direction = new Vector3(x * aimRange, 0f, 1f);

        return direction.normalized;
    }

    public void ResetBall()
    {
        rigidBody.isKinematic = true;

        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        rigidBody.position = startPosition;
        rigidBody.rotation = startRotation;

        roll = false;
        holdingButton = false;
        hasRolled = false;

        ResetPins();
    }
    void ResetPins()
    {
        Pin[] pins = FindObjectsByType<Pin>(FindObjectsSortMode.None);
        
        //for each pin
        foreach (Pin pin in pins)
        {
            pin.ResetPin();
        }
        Pin.score = 0;
    }
    private bool resetting = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasRolled && !resetting)
        {
            if (collision.gameObject.CompareTag("Person") ||
                collision.gameObject.CompareTag("Gutter"))
            {
                StartCoroutine(ResetAfterDelay());
            }
        }
    }

    //wait 2s to reset
    IEnumerator ResetAfterDelay()
    {
        resetting = true;

        yield return new WaitForSeconds(2f);

        ResetBall();

        resetting = false;
    }
}
