using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text p1CharacterName;
    [SerializeField] private TMP_Text p2CharacterName;

    [SerializeField] private Image p1SplashImage;
    [SerializeField] private Image P2SplashImage;

    [SerializeField] private CharacterData[] characterData;

    [SerializeField] private CharacterData p1Data;
    [SerializeField] private CharacterData p2Data;

    public enum currPlayerSelecting
    {
        playerOneSelecting,
        playerTwoSelecting
    };

    public enum gameMode
    {
       hundredHealth,
       fiftyHealth,
       twentyFiveHealth
    }

    gameMode selectedGameMode;
    currPlayerSelecting currPlayer;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseCharacter(CharacterData _char)
    {
        if (currPlayer == currPlayerSelecting.playerOneSelecting)
        {
            p1CharacterName.text = _char.characterName;
            p1SplashImage.sprite = _char.defaultSprite;

            p1Data = _char;
            currPlayer = currPlayerSelecting.playerTwoSelecting;
            return;
        }

        if (currPlayer == currPlayerSelecting.playerTwoSelecting)
        {
            p2CharacterName.text = _char.characterName;
            P2SplashImage.sprite = _char.defaultSprite;

            p2Data = _char;
            return;
        }
    }

    public void SelectGameMode(gameMode _selectedGameMode)
    {
        selectedGameMode = _selectedGameMode;
    }
}
