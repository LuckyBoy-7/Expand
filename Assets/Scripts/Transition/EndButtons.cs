using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lucky.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndButtons : MonoBehaviour
{
    public Button restartButton;
    public Button exitButton;
    public CanvasGroup canvasGroup;

    private void Start()
    {
        restartButton.enabled = false;
        exitButton.enabled = false;

        restartButton.onClick.AddListener(() => { SceneManager.LoadScene("GameScene"); });
        exitButton.onClick.AddListener(Application.Quit);
        this.CreateFuncTimer(() =>
        {
            canvasGroup.DOFade(1, 1).onComplete += () =>
            {
                restartButton.enabled = true;
                exitButton.enabled = true;
            };
        }, () => 2);
    }
}