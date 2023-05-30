using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BasketballVR.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //player and ball must be monobehaviour so I
    // can be type specific
    
    public Player player;

    public Ball ball;
    public Basket basket;

    public static Game game;
    public float ShotCount;

    public int score;

    public GameObject shotsMade;
    public GameObject shotTimer;
    public GameObject countdown;
    public GameObject playerNameComponent;

    public TMP_Text textCmp;

    public TMP_Text shotTimerText;

    public TMP_Text countdownText;
    public TMP_Text playerNameText;

    public GameObject ballPrefab;
    public String playerName;
    public Character selectedChar;
    private Image playerImage;
    public Sprite BlueCharacter;
    public Sprite OrangeCharacter;
    public AudioSource CourtSpeakers;
    public AudioClip CountdownBeep;
    public AudioClip StartBeep;
    public AudioClip EndBuzzer;
    public Material RightStylizedHandMaterial;
    public Material LeftStylizedHandMaterial;
    public Material OrangeCharacterMaterial;
    public Material BlueCharacterMaterial;
    public GameObject TheLights;

    private void OnEnable()
    {
        playerNameComponent = GameObject.Find("PlayerName");
        playerNameText = playerNameComponent.GetComponent<TextMeshProUGUI>();
        Debug.Log(playerNameText);
        Debug.Log(playerNameText.text);
        Debug.Log(PlayerPrefs.GetString("playerName"));
        playerName = PlayerPrefs.GetString("playerName");
       
        selectedChar = (Character)PlayerPrefs.GetInt("CharacterSelection");
        Debug.Log( "Player is using "+selectedChar);
        
    }

    // Start is called before the first frame update
    void Start()
    { 
        
        game = this;
        player = GetComponentInChildren<Player>();
        ball = GameObject.Find("Basketball").GetComponent<Ball>();
        basket = GetComponentInChildren<Basket>();
        score = 0;
        ShotCount = 0;

        
        shotsMade = GameObject.Find("ShotsMade");
        textCmp = shotsMade.GetComponent<TextMeshProUGUI>();
        
        shotTimer = GameObject.Find("ShotTimer");
        shotTimerText = shotTimer.GetComponent<TextMeshProUGUI>();

        countdown = GameObject.Find("Countdown");
        countdownText = countdown.GetComponent<TextMeshProUGUI>();

        playerImage = GameObject.Find("PlayerImage").GetComponent<Image>();

        CourtSpeakers = GameObject.Find("CourtSpeakers").GetComponent<AudioSource>();

        RightStylizedHandMaterial = GameObject.Find("RightStylizedHand").GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        LeftStylizedHandMaterial = GameObject.Find("LeftStylizedHand").GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        Debug.Log("Initial hand material " + RightStylizedHandMaterial);

        
        TheLights = GameObject.Find("Lights");
        
        playerNameText.text = playerName;
        

        textCmp.text = score.ToString();
        ballPrefab = ball.prefab;

        
        SetPlayerImage();
        StartCoroutine(CountdownFromThree());
        
    }

    private void SetPlayerImage()
    {
        switch ( selectedChar )
        {
            case(Character.BLUE):
                Debug.Log("Player chose blue");
                playerImage.sprite = BlueCharacter;
               
                GameObject.Find("RightStylizedHand").GetComponent<SkinnedMeshRenderer>().sharedMaterial =
                    BlueCharacterMaterial;
                GameObject.Find("LeftStylizedHand").GetComponent<SkinnedMeshRenderer>().sharedMaterial =
                    BlueCharacterMaterial;

                GameObject.Find("LeftSphere").GetComponent<MeshRenderer>().sharedMaterial = BlueCharacterMaterial;
                GameObject.Find("RightSphere").GetComponent<MeshRenderer>().sharedMaterial = BlueCharacterMaterial;
                break;
            case(Character.ORANGE):
                playerImage.sprite = OrangeCharacter;
                Debug.Log("Player chose orange");
                
                GameObject.Find("RightStylizedHand").GetComponent<SkinnedMeshRenderer>().sharedMaterial =
                    OrangeCharacterMaterial;
                GameObject.Find("LeftStylizedHand").GetComponent<SkinnedMeshRenderer>().sharedMaterial =
                    OrangeCharacterMaterial;
                GameObject.Find("LeftSphere").GetComponent<MeshRenderer>().sharedMaterial = OrangeCharacterMaterial;
                GameObject.Find("RightSphere").GetComponent<MeshRenderer>().sharedMaterial = OrangeCharacterMaterial;
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        textCmp.text = score.ToString();
        
        if (ball.isBeingShot)
        {
            ball.isBeingShot = false;
            ball.wasShot = true;
            StartCoroutine(DestroyBall());
        }
        if (GameObject.FindWithTag("GameBall") == null)
        {
            ball = Respawn(ballPrefab).GetComponent<Ball>();
        }
        
    }

    public void scoreAPoint()
    {
        score += 1;
        PlayerPrefs.SetFloat("BucketCount",score);
    }

    private GameObject Respawn(GameObject respawnObject)
    {
        return Instantiate(respawnObject, new Vector3(0,0.27f,0.6f),Quaternion.identity);
    }

    private IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(1.25f);
        Destroy(GameObject.FindWithTag("GameBall"));
    }

    private IEnumerator CountdownFromThree()
    {
        int count = 3;

        while (count > 0)
        {
            countdownText.text = count.ToString();
            CourtSpeakers.PlayOneShot(CountdownBeep);
            yield return new WaitForSecondsRealtime(1);
            count--;
        }

        KillTheLights();
        countdownText.text = "Start!";
        CourtSpeakers.PlayOneShot(StartBeep);
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(CountdownFromSixty());

    }

    private void KillTheLights()
    {
        TheLights.SetActive(false);
    }

    private IEnumerator CountdownFromSixty()
    {
        countdown.SetActive(false);
        int count = 60;
        shotTimerText.text = count.ToString();
        while (count > 0)
        {
            
            count--; 
            yield return new WaitForSecondsRealtime(1);
            shotTimerText.text = count.ToString();
            
        }
        
        countdown.SetActive(true);
        countdownText.text = "Time's Up!";
        
        //TODO: Extract to EndGame()
        
        CourtSpeakers.PlayOneShot(EndBuzzer);
        ////TODO: Load end scene smoothly
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadSceneAsync("EndGameStats", LoadSceneMode.Single);
        
    }

    private void OnDisable()
    {
        Debug.Log(ShotCount.ToString());
        PlayerPrefs.SetFloat("ShotCount",ShotCount);
    }
}
