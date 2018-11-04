using System;
using UnityEngine;

[Serializable]
public class Connection {

    [SerializeField]
    private GameObject firstPoint;
    [SerializeField]
    private GameObject secondPoint;

    private GameObject line;

    private bool isActive;

    public Connection()
    {
        isActive = false;
    }

    public void SetActive()
    {
        isActive = true;
        line.GetComponent<LineRenderer>().material.color = Level.Instance.GetLevelColor();
    }

    public bool IsActive()
    {
        return isActive;
    }

    public GameObject GetFirstPoint()
    {
        return firstPoint;
    }

    public GameObject GetSecondPoint()
    {
        return secondPoint;
    }

    public void SetLine(GameObject line)
    {
        this.line = line;
    }
}
