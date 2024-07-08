using System.Collections;
using System.Collections.Generic;
using Lucky.Interactive;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeScale = 5;

    void Start()
    {
        Time.timeScale = timeScale;
    }
}