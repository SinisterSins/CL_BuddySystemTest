using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    int score = 0;
    GameObject[] pins;
    private List<Vector3> pinPositions;
    private List<Quaternion> pinRotations;
    private Vector3 ballPosition;
    [SerializeField] GameObject ball;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pins = GameObject.FindGameObjectsWithTag("Pin");

        pinPositions = new List<Vector3>();
        pinRotations = new List<Quaternion>();
        foreach (var pin in pins) //recording pins position and rotation
        {
            pinPositions.Add(pin.transform.position);
            pinRotations.Add(pin.transform.rotation);
        }

        ballPosition = GameObject.FindGameObjectWithTag("Ball").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //CountPinsDown();

        //Resetting Stage and Game but like primarily the stage :)
        if(Keyboard.current.rKey.wasPressedThisFrame || ball.transform.position.y < -10)
        {
            GameReset();
        }
    }

    //void CountPinsDown()
    //{
    //    for(int i = 0; i < pins.Length; i++)
    //    {
    //        if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 && pins[i].activeSelf)
    //        {
    //            score++;
    //            pins[i].SetActive(false);
    //        }
    //    }
    //}

    void GameReset()
    {
        //Running the ResetStage
        StartCoroutine(ResetStage());
    }

    IEnumerator ResetStage()
    {
        for(int i = 0; i < pins.Length; i++)
        {
            yield return new WaitForSeconds(2);
            var pinPhysics = pins[i].GetComponent<Rigidbody>();
            pinPhysics.linearVelocity = Vector3.zero;
            pinPhysics.position = pinPositions[i];
            pinPhysics.rotation = pinRotations[i];
            pinPhysics.linearVelocity = Vector3.zero;
            pinPhysics.angularVelocity = Vector3.zero;

            var ball = GameObject.FindGameObjectWithTag("Ball");
            ball.transform.position = ballPosition;
            ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
