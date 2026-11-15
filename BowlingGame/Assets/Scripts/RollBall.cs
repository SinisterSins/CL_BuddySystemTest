using UnityEngine;
using UnityEngine.InputSystem;

public class RollBall : MonoBehaviour
{

    Rigidbody ballBody;

    InputAction bowlButt;

    bool holdingButt;
    bool roll;
    public float rollPower;

    public LayerMask pinLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballBody = GetComponent<Rigidbody>();
        bowlButt = InputSystem.actions.FindAction("Bowl");
    }

    // Update is called once per frame
    void Update()
    {
        if (bowlButt.WasPressedThisFrame())
        {
            //if pressing button -> want to aim, add power

            holdingButt = true;
        }
        else if (bowlButt.WasReleasedThisFrame())
        {
            //if button release -> roll bowl

            holdingButt = false;
            roll = true;
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
}
