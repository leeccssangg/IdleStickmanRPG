using System;
using UnityEngine;

public class GameBaseManagers : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}
