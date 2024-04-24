using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class Player : MonoBehaviour
{
    public TMP_Text  winText;
    public GameObject winCanvas;
    public ObjectSpawner objectSpawner;
    public DateTime? endTime = null;

    private void Awake()
    {
        winCanvas.SetActive(false);
    }
    
    public void EndGame()
    {
        Debug.Log("You Lose!");
        winCanvas.SetActive(true);
        winText.text = "You Lose! Total Enemies = " + objectSpawner.m_SpawnCounter;
        if (endTime == null)
        {
            endTime = DateTime.Now;
        }
        winText.text += "\nTime: " + (endTime.Value.Subtract(objectSpawner.time)).ToString();
        Time.timeScale = 0;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
