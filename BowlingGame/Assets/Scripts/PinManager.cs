using UnityEngine;

public class PinManager : MonoBehaviour
{
    public static int score = 0;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody pinBody;
    bool isScored = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //grabbing the pins rigidbody and starting position/rotation
        pinBody = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //checking if the pin has been knocked over
        if(!isScored && Vector3.Angle(transform.up, Vector3.up) > 45f)
        {
            score++;
            isScored = true;
        }
    }

    public void ResetPin() //taking original stored values for the pins and resetting to original positions
    {
        pinBody.linearVelocity = Vector3.zero;
        pinBody.angularVelocity = Vector3.zero;

        transform.position = startPosition;
        transform.rotation = startRotation;

        isScored = false;
    }
}
