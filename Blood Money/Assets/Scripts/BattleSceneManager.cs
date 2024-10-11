using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class BattleSceneManager : MonoBehaviour
{

    public List<CharacterData> playerCharData = new List<CharacterData>();

    [Header("Player Spawning")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<Transform> playerSpawnpoints = new List<Transform>();

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
    [SerializeField] private float timeBetweenCountdowns = 5f;

    private List<GameObject> activePlayers = new List<GameObject>();

    int currPlayerSelecting = 0;

    [HideInInspector] public float startingHealth;

    [HideInInspector] public GameObject levelObjects;

    bool isPlaying = false;
    bool isStarting = false;
    int roundCount = 0;

    public static BattleSceneManager instance { get; private set; }

    //Set the singleton on instantiation
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

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
        if (currPlayerSelecting >= itemSelectMenuParents.Length && !isPlaying && !isStarting)
        {
            Debug.Log("Ended Buy Phase");
            OnRoundStart();
            return;
        }

        if (!isPlaying && !isStarting)
        {
            ShowMenu();
        }


        //If there is 1 or less players remaing (to account for a draw) end the game
        /*
        if (activePlayers.Count < 0 && isPlaying)
        {
            Debug.Log("Ended Round");
            EndRound();
        }
        */

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
        currPlayerSelecting = 0;
        isStarting = true;

        //Clear all active itemSelectMenus
        foreach (GameObject menu in itemSelectMenuParents)
        {
            menu.SetActive(false);
        }

        itemShopParent.SetActive(false);

        LoadLevel(levelObjects);

        //Spawn player for each player that was defined
        for (int i = 0; i < playerCharData.Count; i++)
        {
            Debug.Log("Made it to the round start");

            //TODO ADD RESPAWNPOINTS
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

        StartCoroutine(DoCountdown());
    }

    private IEnumerator DoCountdown()
    {
        roundStartText.gameObject.SetActive(true);
        roundStartText.text = "3";

        yield return new WaitForSeconds(timeBetweenCountdowns);
        roundStartText.text = "2";

        yield return new WaitForSeconds(timeBetweenCountdowns);
        roundStartText.text = "1";

        yield return new WaitForSeconds(timeBetweenCountdowns);
        roundStartText.text = "FIGHT!";

        yield return new WaitForSeconds(timeBetweenCountdowns);
        roundStartText.gameObject.SetActive(false);
        isStarting = false;
        isPlaying = true;
    }

    private void LoadLevel(GameObject _levelObjects)
    {
        GameObject levelGeo = Instantiate(_levelObjects, new Vector3(0, 0, 0), Quaternion.identity);

        //Loop through all child objects and add the ones with the Spawnpoint tag to the active list
        for (int i = 0; i < levelGeo.transform.childCount; i++)
        {
            if (levelGeo.transform.GetChild(i).tag == "Spawnpoint")
            {
                playerSpawnpoints.Add(levelGeo.transform.GetChild(i));
            }
        }
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
