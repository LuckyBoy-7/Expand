using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    public Button slowButton;
    public Button fastButton;

    private void Start()
    {
        slowButton.onClick.AddListener(() => { Time.timeScale = Mathf.Max(0, Time.timeScale - 1); });
        fastButton.onClick.AddListener(() => { Time.timeScale += 1; });
    }

    private void Update()
    {
    }
}