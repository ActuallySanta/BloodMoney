using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void doExitGame()
    {
        Debug.Log("losies this quit");
        Application.Quit();
    }
}
