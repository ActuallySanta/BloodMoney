using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject levelObjects;
    [SerializeField] private TMP_Text currentLevelSelectionText;
    [SerializeField] private CharacterSelectMenuManager selectMenuManager;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ChooseLevel(levelObjects);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseLevel(GameObject _selectedLevel)
    {
        levelObjects = _selectedLevel;
        currentLevelSelectionText.text = "Current Level: " + levelObjects.name;
    }

    public void FinishLevelSelection()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        selectMenuManager = Instantiate(selectMenuManager, Vector2.zero, Quaternion.identity);

        selectMenuManager.levelObject = levelObjects;

        Destroy(this.gameObject);
    }
}
