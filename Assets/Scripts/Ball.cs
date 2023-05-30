using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Ball : MonoBehaviour
{
    public Ball ball;
    public bool isBeingHeld;
    public bool isBeingShot;
    public bool wasShot;
    public bool Scored;
    public Vector3 grabOffset;
    public GameObject prefab;
    public Transform ballStart;
    public Vector3 ballStartPos;

    public AudioSource BallAudio;
    // Start is called before the first frame update
    void Start()
    {
        ball = this;
        wasShot = false;
        isBeingHeld = false;
        isBeingShot = false;
        Scored = false;
        ballStart = ball.transform;
        ballStartPos = ballStart.position;
        // BallAudio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    
}
