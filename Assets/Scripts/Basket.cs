using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Basket : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject collidingObject;
    public Basket basket;
    public Game game;
    public AudioSource BasketAudio;
    public AudioClip Swish;

    void Start()
    {
        basket = this;
        game = GameObject.Find("Game").GetComponent<Game>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other) {
        collidingObject = other.gameObject;

        Debug.Log("Trigger entered");
        Debug.Log("entered: "+collidingObject);
        
        Debug.Log("basket "+ basket+" collided!");
        Debug.Log(GetComponentInParent<Basket>()+" colliding with "+collidingObject.tag);
        if(collidingObject.tag == "GameBall"  )
        {
            collidingObject.GetComponent<Ball>().Scored = true;
            Debug.Log("Basket went through the goal!");
            updateScore(game);
            BasketAudio.PlayOneShot(Swish);

        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == collidingObject)
        {
            collidingObject = null;
        }
    }

    private void updateScore(Game game)
    {
        game.scoreAPoint();
    }
}
