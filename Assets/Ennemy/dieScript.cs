using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieScript : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(killTheGame), 2.1f);
    }
    private void killTheGame()
    {
        Application.Quit();
    }
}
