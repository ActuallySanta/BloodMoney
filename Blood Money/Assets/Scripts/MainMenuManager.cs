using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CharacterSelectMenuManager;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainCan;
    public GameObject HowToCan;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeToSettings()
    {
        //Debug.Log("this is the transfer to settings page");
        MainCan.SetActive(false);
        HowToCan.SetActive(true);
    }
}

