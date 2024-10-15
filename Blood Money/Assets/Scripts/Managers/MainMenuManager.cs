using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CharacterSelectMenuManager;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject howToCanvas;
    [SerializeField] private GameObject controlsCanvas;

    [SerializeField] private GameObject[] menus;

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

    public void ChangeToHowTo()
    {
        //Debug.Log("this is the transfer to settings page");

        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }

        howToCanvas.SetActive(true);
    }

    public void ChangeToControls()
    {

        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }

        controlsCanvas.SetActive(true);
    }

    public void BackToMainMenu()
    {

        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }

        mainCanvas.SetActive(true);
    }
}

