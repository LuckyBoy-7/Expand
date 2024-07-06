using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lucky.Extensions;
using Lucky.Managers;
using UnityEngine;

public class LineManager : Singleton<LineManager>
{
    public float lineWidth = 10f;
    public float remainDuration = 0.2f;
    public float fadeOutDuration = 0.2f;

    public void ShowOneWayLine(Vector3 from, Vector3 to, float duration)
    {
        var linePrefab = Resources.Load<LineRenderer>("Components/LineRenderer");
        LineRenderer line = Instantiate(linePrefab);
        line.enabled = false;
        line.startWidth = line.endWidth = lineWidth;
        line.positionCount = 2;
        line.enabled = true;
        line.SetPosition(0, from);
        line.SetPosition(1, from);

        var seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => line.GetPosition(1), value => line.SetPosition(1, value), to, duration));
        seq.AppendInterval(remainDuration);
        seq.Append(DOTween.To(() => line.startColor, value => line.startColor = line.endColor = value, line.startColor.WithA(0), duration)).onComplete +=
            () => Destroy(line.gameObject);
    }
}