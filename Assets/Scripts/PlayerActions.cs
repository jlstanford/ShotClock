using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class PlayerActions : MonoBehaviour
{
    public bool gripHeld;
    /// <summary>
    /// What we're colliding with
    /// </summary>
    public GameObject collidingObject;
    //
    public GameObject heldObject;
    //
    public Player player;

    public float throwForce;
    public Game game;

    public Vector3 controllerCenterOfMass;
    public List<XRNodeState> nodeStates;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;


    void Start()
    {
        collidingObject = null;
        gripHeld = false;
        heldObject = null;
        player = GetComponentInParent<Player>();
        game = GetComponentInParent<Game>();
        throwForce = 5;
        // animator = GetComponent<Animator>();
        controllerCenterOfMass = GetComponent<Rigidbody>().centerOfMass;
    }

  
    
    public void Grab(Transform controller)
    {
        if(heldObject.GetComponent<Ball>()){
            GameObject ball = heldObject;
            Debug.Log("VR Grabbing Ball!");
            
            Debug.Log(this+"picked up the "+ball.GetComponent<Ball>());
            
            ball.transform.SetParent(controller);
            ball.transform.position = controller.position;
           
          
            ball.GetComponent<Ball>().isBeingShot = false;
            ball.GetComponent<Ball>().isBeingHeld = true;
           
            ball.GetComponent<Rigidbody>().useGravity = false;
            ball.GetComponent<Rigidbody>().isKinematic = true;
           
        }
        
    }
    
    public void Release()
    {
        //throw
        if(heldObject.GetComponent<Ball>()){
            var ball = heldObject.GetComponent<Ball>();
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            Debug.Log("Releasing the ball");
            
           nodeStates = new List<XRNodeState>();
            InputTracking.GetNodeStates(nodeStates);
            Vector3 controllerVelocity;
            foreach (var nodeState in nodeStates)
            {
                nodeState.TryGetVelocity(out controllerVelocity);
                Debug.Log("XR Node State - getting velocity: "+nodeState.TryGetVelocity(out controllerVelocity));
                Debug.Log("XR Node State - velocity is: " +controllerVelocity);
            
                Debug.Log("VR Ball is being thrown at:"+controllerVelocity*throwForce);
                
                rb.velocity = controllerVelocity * throwForce;
                Debug.Log("ball velocity: "+rb.velocity);
            }
            
            rb.isKinematic = false;
           
            rb.useGravity = true;
            ball.isBeingShot = true;
            
           
            ball.isBeingHeld = false;
            game.ShotCount++;
            
            heldObject.transform.SetParent(GetComponentInParent<Game>().transform);
            heldObject = null;
            
        }
        
    } 
    
    // Update is called once per frame
    void Update()
    {


        var inputDevices = new List<InputDevice>();
        var controllerDevices = new List<InputDevice>();

        var rightControllers = new List<InputDevice>();
        var leftControllers = new List<InputDevice>();
        
        InputDevices.GetDevices(inputDevices);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller| InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Right, controllerDevices);
        
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, leftControllers);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, rightControllers);
        
       

        

        foreach (InputDevice rightController in rightControllers)
        {
            bool triggerState = false;
            Debug.Log("rightController: "+ rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerState).ToString());
            
            //move this out into its own left controller loop
            foreach (InputDevice leftController in leftControllers)
            {
               
                Debug.Log("leftController: "+leftController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerState).ToString());
                bool triggerValue;
            
                //break this up by controller loop
                
                // if either the right trigger or left trigger is pressed
                if (leftController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue == true || rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue == true)
                {
                    
                    Debug.Log("Trigger button is pressed. "+ triggerValue);
                    gripHeld = true;
                    Debug.Log("colliding with object: "+collidingObject);
                                  
                    //check that either controller collided with a rigidbody
                    if (leftController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) &&
                        triggerValue == true)
                    {
                        
                        leftHandAnimator.SetBool("TriggerPulled",triggerValue);
                        if (collidingObject && collidingObject.GetComponent<Rigidbody>())
                        {
                            heldObject = collidingObject;
                            Debug.Log("LeftController transform: "+GameObject.Find("LeftController").GetComponent<Transform>().position);
                            Grab(GameObject.Find("LeftController").GetComponent<Transform>());
                            Debug.Log("Left is holding "+collidingObject);
                        }
                    }
                    
                    if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) &&
                        triggerValue == true)
                    {
                        rightHandAnimator.SetBool("TriggerPulled",triggerValue);
                        Debug.Log("colliding with "+collidingObject);
                        if (collidingObject && collidingObject.GetComponent<Rigidbody>())
                        {
                            heldObject = collidingObject;
                            Debug.Log("RightController transform: "+GameObject.Find("RightController").GetComponent<Transform>().position);
                            Grab(GameObject.Find("RightController").GetComponent<Transform>());
                            Debug.Log("Right is holding "+collidingObject);
                        }
                    }

                }
                else if(leftController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue == false && rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue == false)
                {

                    Debug.Log("Trigger button is depressed. " + triggerValue);
                    gripHeld =  false;
                    leftHandAnimator.SetBool("TriggerPulled",triggerValue);
                    rightHandAnimator.SetBool("TriggerPulled",triggerValue);
                    
                    if ( heldObject != null)
                    {
                        Release();
                        Debug.Log("dropping "+collidingObject);
                    }

                }
            }
        }

    }
    
    private void OnTriggerEnter(Collider other) {
        collidingObject = other.gameObject;

        Debug.Log("Trigger entered");
        Debug.Log("entered: "+collidingObject);
        
        Debug.Log("player "+player+" collided!");
        Debug.Log(GetComponentInParent<Player>()+" colliding with "+collidingObject.tag);
        if(collidingObject.tag == "GameBall"  ) {
            
            Debug.Log("VR touching ball");
            // player.currentState = PlayerManager.grabbing;
        }
        
        // if(collidingObject.tag == "GameBall"  && playerIsGrabbing) {
            
        // Debug.Log("VR touching ball");
        //      GrabWith(grabbingController)
        // }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == collidingObject)
        {
            collidingObject = null;
        }
    }
}
