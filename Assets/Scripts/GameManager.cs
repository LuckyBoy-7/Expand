using System.Collections;
using System.Collections.Generic;
using Lucky.Interactive;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeScale = 5;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScale;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
