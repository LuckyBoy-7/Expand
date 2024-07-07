using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class TerritoryArea : MonoBehaviour
{
    public TMP_Text text;
    public float ratio;

    void Update()
    {
        Vector2 min = Vector2.positiveInfinity;
        Vector2 max = Vector2.negativeInfinity;
        foreach (var b in BuildingsManager.instance.buildings)
        {
            Vector2 vec = b.transform.position;
            min = new Vector2(Mathf.Min(min.x, vec.x), Mathf.Min(min.y, vec.y));
            max = new Vector2(Mathf.Max(max.x, vec.x), Mathf.Max(max.y, vec.y));
        }

        text.text = $"Area: {(int)((max.x - min.x) * ratio * (max.y - min.y))}km^2";
    }
}