using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    public List<InputDevice> handHeldControllers;
    
    // Start is called before the first frame update
    void Start()
    {
        // var inputDevices = new List<InputDevice>();
        // // Debug.Log(inputDevices.Count);
        // InputDevices.GetDevices(inputDevices);
        handHeldControllers = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand, handHeldControllers);    
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxis("Horizontal") < 0 || Input.GetKeyDown("left"))
        {
            // Debug.Log("Going Left");
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);

        }
        else if (Input.GetAxis("Horizontal") > 0 || Input.GetKeyDown("right"))
        {
            // Debug.Log("Going Right");
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }

        if (Input.GetAxis("Vertical") < 0 || Input.GetKeyDown("down"))
        {
            // Debug.Log("going back");
            transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
        }
        else if (Input.GetAxis("Vertical") > 0 || Input.GetKeyDown("right"))
        {
            // Debug.Log("going forward");
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }

        

        
    }
}
