using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lucky.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup.DOFade(1, 2);
    }
}