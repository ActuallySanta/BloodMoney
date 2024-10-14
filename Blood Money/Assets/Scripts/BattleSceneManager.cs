using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSceneManager : MonoBehaviour
{

    public List<CharacterData> playerCharData = new List<CharacterData>();
    public List<GunData> playerGunData = new List<GunData>();
    public float[] playerStartingHealth = new float[2];

    [Header("Player Spawning")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<Transform> playerSpawnpoints = new List<Transform>();

    [Header("UI Stuff")]
    [SerializeField] private TMP_Text roundStartText;
    [SerializeField] private TMP_Text p1CharacterName;
    [SerializeField] private TMP_Text p2CharacterName;
    [SerializeField] private TMP_Text roundCountText;
    [SerializeField] private Slider[] playerHealthSliders;
    [SerializeField] private TMP_Text p1SelectedWeaponText;
    [SerializeField] private TMP_Text p2SelectedWeaponText;
    [SerializeField] private TMP_Text p1CurrentHealthText;
    [SerializeField] private TMP_Text p2CurrentHealthText;

    [Header("Player Shop Objects")]
    [SerializeField] private GameObject[] itemSelectMenuParents;
    [SerializeField] private GameObject itemShopParent;
    [SerializeField] private GunData defaultGunData;

    [Header("Display Timers")]
    [SerializeField] private float timeBetweenCountdowns = 5f;
    [SerializeField] private float winScreenDisplayTime = 5f;

    private List<GameObject> activePlayers = new List<GameObject>();

    int currPlayerSelecting = 0;
    
    public float startingHealth;

    [HideInInspector] public GameObject levelObjects;

    bool isPlaying = false;
    public bool isStarting = false;
    int roundCount = 0;

    public static BattleSceneManager instance { get; private set; }

    //Set the singleton on instantiation
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
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

        for (int i = 0; i < playerCharData.Count; i++)
        {
            playerStartingHealth[i] = startingHealth;
        }

        p1CharacterName.gameObject.SetActive(false);
        p2CharacterName.gameObject.SetActive(false);

        p1CurrentHealthText.text = "Current Health: " + playerStartingHealth[0];
        p2CurrentHealthText.text = "Current Health: " + playerStartingHealth[1];

        p1SelectedWeaponText.text = "Selected: " + playerGunData[0].name;
        p2SelectedWeaponText.text = "Selected: " + playerGunData[1].name;

        ShowMenu();
    }



    // Update is called once per frame
    void Update()
    {
        roundCountText.text = "ROUND: " + roundCount;

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


        //If there is 1 or less players remaining (to account for a draw) end the game
        
        if (activePlayers.Count <= 1 && isPlaying)
        {
            Debug.Log("Ended Round");
            StartCoroutine(EndRound());
        }
        

        if (isPlaying)
        {
            //Update Health Value
            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (activePlayers[i] == null) return;

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

    private IEnumerator EndRound()
    {
        isPlaying = false;
        isStarting = true;

        foreach (GameObject player in activePlayers)
        {
            Destroy(player);
        }

        roundStartText.gameObject.SetActive(true);
        roundStartText.text = activePlayers[0].GetComponent<PlayerController>().charData.name + " WINS!";

        yield return new WaitForSeconds(winScreenDisplayTime);
        isStarting = false;
        roundStartText.gameObject.SetActive(false);
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

            //Instantiate each player
            GameObject player = Instantiate(playerPrefab, playerSpawnpoints[i].position,
                Quaternion.identity, this.gameObject.transform);

            //Get references to the player controller,player input manager, and the health manager
            PlayerController pController = player.GetComponent<PlayerController>();
            PlayerHealthManager pHManager = player.GetComponent<PlayerHealthManager>();
            PlayerInputController pInputController = player.GetComponent<PlayerInputController>();
            WeaponController pWeapon = player.GetComponent<WeaponController>();

            //Set data to each instance of the player
            pHManager.maxHealth = playerStartingHealth[i];
            pWeapon.data = playerGunData[i];
            pController.charData = playerCharData[i];
            pInputController.playerInd = i;

            //Add each player to a list containing all active players
            activePlayers.Add(player);
        }

        p1CharacterName.gameObject.SetActive(true);
        p2CharacterName.gameObject.SetActive(true);

        p1CharacterName.text = playerCharData[0].name.ToUpper();
        p2CharacterName.text = playerCharData[1].name.ToUpper();

        for (int i = 0; i < playerHealthSliders.Length; i++)
        {
            playerHealthSliders[i].maxValue = startingHealth;
            playerHealthSliders[i].value = playerStartingHealth[i];
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

    /// <summary>
    /// The method called by the UI Button to select which gun the player is currently buying
    /// </summary>
    /// <param name="_data">The gun the player is selecting</param>
    public void PlayerBuyGun(GunData _data)
    {
        

        if (currPlayerSelecting == 0)
        {
            playerGunData.Insert(0, _data);

            if (playerStartingHealth[0] != startingHealth)
            {
                playerStartingHealth[0] = startingHealth;
            }

            p1SelectedWeaponText.text = "Selected: " + _data.name;

            playerStartingHealth[0] -= _data.healthCost;
            p1CurrentHealthText.text = "Current Health: " + playerStartingHealth[0];
        }
        else if (currPlayerSelecting == 1)
        {
            playerGunData.Insert(1, _data);

            if (playerStartingHealth[1] != startingHealth)
            {
                playerStartingHealth[1] = startingHealth;
            }

            playerStartingHealth[1] -= _data.healthCost;

            p2SelectedWeaponText.text = "Selected: " + _data.name;
            p2CurrentHealthText.text = "Current Health: " + playerStartingHealth[1];
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Destroy(this.gameObject);
    }
}
