using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class leveldata
{
    public string levelName;

    [Header("leveltimer")]
    public int minute;
    public int seconds;
    public float scale;
    public GameObject[] TotalObjects;
}
