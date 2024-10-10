using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class BattleSceneManager : MonoBehaviour
{

    [HideInInspector] public CharacterData[] playerCharData;

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
    [SerializeField] private GameObject p1ItemSelectMenuParent;
    [SerializeField] private GameObject p2ItemSelectMenuParent;

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
        p2ItemSelectMenuParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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

    private void EndRound()
    {
        isPlaying = false;
        roundCount++;
    }

    void OnRoundStart()
    {
        p1ItemSelectMenuParent.SetActive(false);
        p2ItemSelectMenuParent.SetActive(false);

        //Spawn player for each player that was defined
        for (int i = 0; i < playerCharData.Length; i++)
        {
            //Instantiate each player
            GameObject player = Instantiate(playerPrefab, playerSpawnpoints[i].position,
                Quaternion.identity, this.gameObject.transform);

            PlayerController pController = player.GetComponent<PlayerController>();
            PlayerHealthManager pHManager = player.GetComponent<PlayerHealthManager>();

            pHManager.maxHealth = startingHealth;

            pController.charData = playerCharData[i];

            activePlayers.Add(player);
        }

        isPlaying = true;
    }


    public void RemovePlayerFromActiveList(GameObject _playerToRemove)
    {
        activePlayers.Remove(_playerToRemove);
    }
}
