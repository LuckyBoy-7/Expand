using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Lucky.Extensions;
using Lucky.Managers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LineManager : Singleton<LineManager>
{
    public Transform lineContainer;
    public float lineWidth = 10f;
    public float remainDuration = 0.2f;
    public float fadeOutDuration = 0.2f;


    public void ShowOneWayLine(Vector3 from, Vector3 to, float duration)
    {
        var line = CreateLine();
        line.SetPosition(0, from);
        line.SetPosition(1, from);

        var seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => line.GetPosition(1), value => line.SetPosition(1, value), to, duration));
        seq.AppendInterval(remainDuration);
        seq.Append(DOTween.To(() => line.startColor, value => line.startColor = line.endColor = value, line.startColor.WithA(0), fadeOutDuration)).onComplete +=
            () => Destroy(line.gameObject);
    }

    public LineRenderer CreateLine()
    {
        var linePrefab = Resources.Load<LineRenderer>("Components/LineRenderer");
        LineRenderer line = Instantiate(linePrefab, lineContainer);
        line.startWidth = line.endWidth = lineWidth;
        line.positionCount = 2;
        return line;
    }
}