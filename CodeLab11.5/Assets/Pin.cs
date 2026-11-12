using UnityEngine;

public class Pin : MonoBehaviour
{
    public static int score = 0;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rb;
    private bool scored = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        // Check if the pin has fallen by angles > 45degree
        if (!scored && Vector3.Angle(transform.up, Vector3.up) > 45f)
        {
            score++;
            scored = true;
        }
    }

    public void ResetPin()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPosition;
        transform.rotation = startRotation;

  
        scored = false;
    }
}