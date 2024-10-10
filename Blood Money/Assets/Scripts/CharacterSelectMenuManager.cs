using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text p1CharacterName;
    [SerializeField] private TMP_Text p2CharacterName;

    [SerializeField] private Image p1SplashImage;
    [SerializeField] private Image p2SplashImage;

    [SerializeField] private CharacterData[] characterData;

    private CharacterData p1Data;
    private CharacterData p2Data;

    public enum currPlayerSelecting
    {
        playerOneSelecting,
        playerTwoSelecting
    };

    public enum gameMode
    {
        hundredHealth,
        fiftyHealth,
        twentyFiveHealth,
        hundredFiftyHealth,
        oneHealth,
    }
    [SerializeField] private TMP_Text modeSelectText;

    gameMode selectedGameMode;
    currPlayerSelecting currPlayer;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        p1SplashImage.gameObject.SetActive(false);
        p2SplashImage.gameObject.SetActive(false);
        SelectGameMode(gameMode.hundredHealth);
    }



    public void ChooseCharacter(CharacterData _char)
    {
        if (currPlayer == currPlayerSelecting.playerOneSelecting)
        {
            p1CharacterName.text = _char.characterName;

            p1SplashImage.gameObject.SetActive(true);
            p1SplashImage.sprite = _char.defaultSprite;

            p1Data = _char;
            currPlayer = currPlayerSelecting.playerTwoSelecting;
            return;
        }

        if (currPlayer == currPlayerSelecting.playerTwoSelecting)
        {
            p2CharacterName.text = _char.characterName;

            p2SplashImage.gameObject.SetActive(true);
            p2SplashImage.sprite = _char.defaultSprite;

            p2Data = _char;
            return;
        }
    }

    public void StringToGameMode(string _gameModeString)
    {
        switch (_gameModeString)
        {
            case "100":
                SelectGameMode(gameMode.hundredHealth);
                break;
            case "150":
                SelectGameMode(gameMode.hundredFiftyHealth);
                break;

            case "50":
                SelectGameMode(gameMode.fiftyHealth);
                break;

            case "25":
                SelectGameMode(gameMode.twentyFiveHealth);
                break;

            case "1":
                SelectGameMode(gameMode.oneHealth);
                break;
        }
    }


    private void SelectGameMode(gameMode _selectedGameMode)
    {
        selectedGameMode = _selectedGameMode;

        switch (selectedGameMode)
        {
            case gameMode.fiftyHealth:
                modeSelectText.text = "Selected Mode: 50 Health";
                break;

            case gameMode.hundredFiftyHealth:
                modeSelectText.text = "Selected Mode: 150 Health";
                break;

            case gameMode.twentyFiveHealth:
                modeSelectText.text = "Selected Mode: 25 Health";
                break;

            case gameMode.hundredHealth:
                modeSelectText.text = "Selected Mode: 100 Health";
                break;

            case gameMode.oneHealth:
                modeSelectText.text = "Selected Mode: 1 Health";
                break;
        }
    }
}
