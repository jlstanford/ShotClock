using System;
using System.Collections;
using System.Collections.Generic;
using BasketballVR.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class TitleSceneUIActions : MonoBehaviour
{
    public string PlayerName;

    public TMP_InputField nameInput;

    private int CharacterSelection = 0;

    // Start is called before the first frame update
    void Start()
    {
      
        nameInput = GameObject.Find("NameInput").GetComponent<TMP_InputField>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
        //
        // SharedResources.PlayerName = nameInput.ToString();
    }

    private void OnDisable()
    {
        
        if (nameInput != null)
        {
            Debug.Log(nameInput.text);
            PlayerName = nameInput.text;
            PlayerPrefs.SetString("playerName",PlayerName);
        }
        
        PlayerPrefs.SetInt("CharacterSelection", CharacterSelection);
    }

    public void GoToGame()
    {
        
        SceneManager.LoadScene("BasketballCourt", LoadSceneMode.Single);
    }
    

    

    public void SelectCharacter(int choice)
    {
        Debug.Log("button clicked");
        switch ( (Character)choice )
        {
            case(Character.BLUE):
                CharacterSelection = 0;
                break;
            case(Character.ORANGE):
                CharacterSelection = 1;
                break;
        }
    }

    public void OpenKeyboard()
    {
        TouchScreenKeyboard.Open("",TouchScreenKeyboardType.Default);
        Debug.Log("Opening Keyboard "+TouchScreenKeyboard.visible);
    }
    
    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("HoverEntered"+args.interactorObject);
    }
}
