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

    //All available Scriptable objects of type Character Data
    [SerializeField] private CharacterData[] characterData;

    //The individual data that each player has
    private CharacterData p1Data;
    private CharacterData p2Data;

    public enum currPlayerSelecting
    {
        playerOneSelecting,
        playerTwoSelecting,
        bothPlayersSelected
    };

    public enum gameMode
    {
        hundredHealth,
        fiftyHealth,
        twentyFiveHealth,
        hundredFiftyHealth,
        oneHealth,
    }
    float startHealth;

    [SerializeField] private TMP_Text modeSelectText;

    //Enum values
    gameMode selectedGameMode;
    currPlayerSelecting currPlayer;

    [SerializeField] private BattleSceneManager battleManager;

    public GameObject levelObject;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        p1SplashImage.gameObject.SetActive(false);
        p2SplashImage.gameObject.SetActive(false);
        SelectGameMode(gameMode.hundredHealth);
    }

    private void Update()
    {
        //If we have gotten past the all the menu scenes, its time to start the game
        if (SceneManager.GetActiveScene().buildIndex > 3)
        {
            battleManager = Instantiate(battleManager, Vector2.zero, Quaternion.identity);

            //Add all the player Data to the array of players
            battleManager.playerCharData.Add(p1Data);
            battleManager.playerCharData.Add(p2Data);
            battleManager.startingHealth = startHealth;
            battleManager.levelObjects = levelObject;

            Destroy(gameObject);
            Debug.Log("Destroyed Self");
            return;
        }

        if (currPlayer == currPlayerSelecting.bothPlayersSelected)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
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
            currPlayer = currPlayerSelecting.bothPlayersSelected;
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

    public void GoBack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Destroy(this.gameObject);
    }

    private void SelectGameMode(gameMode _selectedGameMode)
    {
        selectedGameMode = _selectedGameMode;

        switch (selectedGameMode)
        {
            case gameMode.fiftyHealth:
                modeSelectText.text = "Selected Mode: 50 Health";
                startHealth = 50;
                break;

            case gameMode.hundredFiftyHealth:
                modeSelectText.text = "Selected Mode: 150 Health";
                startHealth = 150;
                break;

            case gameMode.twentyFiveHealth:
                modeSelectText.text = "Selected Mode: 25 Health";
                startHealth = 25;
                break;

            case gameMode.hundredHealth:
                modeSelectText.text = "Selected Mode: 100 Health";
                startHealth = 100;
                break;

            case gameMode.oneHealth:
                modeSelectText.text = "Selected Mode: 1 Health";
                startHealth = 1;
                break;
        }
    }
}
