using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    public Button restartButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(() => { SceneManager.LoadScene("GameScene"); });
    }
}