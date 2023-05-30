using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUIActions : MonoBehaviour
{
    
    public float BucketCount;
    public float ShotsMissed;
    //TODO: refactor to shots attempted
    public float ShotCount;
    public Double ShotAccuracy;
    [SerializeField] private GameObject bucketDisplay;
    [SerializeField] private TextMeshProUGUI bucketTextCmp;
    [SerializeField] private GameObject missDisplay;
    [SerializeField] private TextMeshProUGUI missTextCmp;
    [SerializeField] private GameObject accuracyDisplay;
    [SerializeField] private TextMeshProUGUI accuracyPercentageTextCmp;
    [SerializeField] private GameObject shotsAttemptedDisplay;
    [SerializeField] private TextMeshProUGUI shotsAttemptedTextCmp;

    private void OnEnable()
    {
        BucketCount = PlayerPrefs.GetFloat("BucketCount");
        ShotCount = PlayerPrefs.GetFloat("ShotCount");
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log(ShotCount.ToString());
        
        bucketDisplay = GameObject.Find("BucketCount");
        bucketTextCmp = bucketDisplay.GetComponent<TextMeshProUGUI>();
        missDisplay = GameObject.Find("MissCount");
        missTextCmp = missDisplay.GetComponent<TextMeshProUGUI>();
        accuracyDisplay = GameObject.Find("AccuracyPercentage");
        accuracyPercentageTextCmp = accuracyDisplay.GetComponent<TextMeshProUGUI>();
        shotsAttemptedDisplay = GameObject.Find("ShotCount");
        shotsAttemptedTextCmp = shotsAttemptedDisplay.GetComponent<TextMeshProUGUI>();
        
        calculateStats();

        bucketTextCmp.text = BucketCount.ToString();
        missTextCmp.text = ShotsMissed.ToString();
        accuracyPercentageTextCmp.text = $"{ShotAccuracy.ToString()}%";
        shotsAttemptedTextCmp.text = ShotCount.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    
    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }

    private void calculateStats()
    {
        ShotsMissed = ShotCount - BucketCount;
        ShotAccuracy = Math.Round((BucketCount / ShotCount),2)*100;
        Debug.Log(ShotsMissed.ToString());
        Debug.Log(ShotAccuracy.ToString());
        Debug.Log(Math.Round(ShotAccuracy,2).ToString());
    }

    public void QuitGame()
    {
        
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
