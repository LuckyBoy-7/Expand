using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lucky.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScroller : MonoBehaviour
{
    public float scrollSpeed = 100;
    public float enterDuration = 10f;
    public CanvasGroup canvasGroup;

    private void Start()
    {
        this.CreateFuncTimer(() => { canvasGroup.DOFade(0, 2).onComplete += () => SceneManager.LoadScene("GameScene"); }, () => enterDuration);
    }

    private void Update()
    {
        transform.position += Vector3.up * (scrollSpeed * Time.deltaTime);
    }
}