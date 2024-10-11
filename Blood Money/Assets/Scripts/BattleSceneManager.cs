using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class BattleSceneManager : MonoBehaviour
{

    [HideInInspector] public List<CharacterData> playerCharData = new List<CharacterData>();

    [Header("Player Spawning")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] playerSpawnpoints;

    [Header("UI Stuff")]
    [SerializeField] private TMP_Text roundStartText;
    [SerializeField] private TMP_Text p1CharacterName;
    [SerializeField] private TMP_Text p2CharacterName;
    [SerializeField] private TMP_Text roundCountText;
    [SerializeField] private Slider[] playerHealthSliders;

    [Header("Player Shop Objects")]
    [SerializeField] private GameObject[] itemSelectMenuParents;
    [SerializeField] private GameObject itemShopParent;

    [Header("Round Start")]
    [SerializeField] private float timeBetweenCountdowns = .5f;

    private List<GameObject> activePlayers = new List<GameObject>();

    int currPlayerSelecting = 0;

    [HideInInspector] public float startingHealth;

    bool isPlaying = false;

    int roundCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject menu in itemSelectMenuParents)
        {
            menu.SetActive(false);
        }

        ShowMenu();
    }



    // Update is called once per frame
    void Update()
    {
        if (currPlayerSelecting >= itemSelectMenuParents.Length && !isPlaying)
        {
            Debug.Log("Ended Buy Phase");
            isPlaying = true;
            OnRoundStart();
        }

        if (!isPlaying)
        {
            ShowMenu();
        }

        if (activePlayers.Count == 0 && isPlaying)
        {
            EndRound();
        }

        if (isPlaying)
        {
            //Update Health Value
            for (int i = 0; i < activePlayers.Count; i++)
            {
                PlayerHealthManager hManager = activePlayers[i].GetComponent<PlayerHealthManager>();
                playerHealthSliders[i].value = hManager.currHealth;
            }
        }
    }

    private void ShowMenu()
    {
        itemShopParent.SetActive(true);
        foreach (GameObject menu in itemSelectMenuParents)
        {
            menu.SetActive(false);
        }

        itemSelectMenuParents[currPlayerSelecting].SetActive(true);

    }

    private void EndRound()
    {
        isPlaying = false;
        roundCount++;
    }

    void OnRoundStart()
    {
        //Clear all active itemSelectMenus
        foreach (GameObject menu in itemSelectMenuParents)
        {
            menu.SetActive(false);
        }

        //Spawn player for each player that was defined
        for (int i = 0; i < playerCharData.Count; i++)
        {
            Debug.Log("Made it to the round start");
            break;

            //Instantiate each player
            GameObject player = Instantiate(playerPrefab, playerSpawnpoints[i].position,
                Quaternion.identity, this.gameObject.transform);

            //Get references to the player controller,player input manager, and the health manager
            PlayerController pController = player.GetComponent<PlayerController>();
            PlayerHealthManager pHManager = player.GetComponent<PlayerHealthManager>();
            PlayerInputController pInputController = player.GetComponent<PlayerInputController>();

            //Set data to each instance of the player
            pHManager.maxHealth = startingHealth;

            pController.charData = playerCharData[i];
            pInputController.playerInd = i;

            //Add each player to a list containing all active players
            activePlayers.Add(player);
        }

        isPlaying = true;
    }

    public void ChangeSelectedPlayer()
    {
        currPlayerSelecting++;

        foreach (GameObject menu in itemSelectMenuParents)
        {
            menu.SetActive(false);
        }
    }

    public void RemovePlayerFromActiveList(GameObject _playerToRemove)
    {
        activePlayers.Remove(_playerToRemove);
    }
}
