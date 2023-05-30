using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovement characterController;

    public PlayerActions actions;

    public Camera playerCam;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<PlayerMovement>();
        actions = GetComponent<PlayerActions>();
        playerCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
