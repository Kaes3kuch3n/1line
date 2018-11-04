using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    [SerializeField]
    private Connection[] connections;
    [SerializeField]
    private Color[] colorSet;
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private GameObject winParticle;
    [SerializeField]
    private GameObject winScreen;

    private int activeConnections;
    public const string pointTag = "Point";
    private bool levelWon;
    private Color levelColor;

    public static Level Instance { get; private set; }
    
	private void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one level controller in this scene!");
        }

        Instance = this;

        levelWon = false;

        levelColor = GetRandomColor();
        GetComponentInParent<LineRenderer>().material.color = levelColor;

        foreach (GameObject go in GameObject.FindGameObjectsWithTag(pointTag))
        {
            go.GetComponent<SpriteRenderer>().material.color = levelColor;
        }

        SetUpConnections();
	}

    private void Update()
    {
        if (activeConnections == connections.Length && !levelWon)
        {
            Debug.Log("Level complete!");
            GetComponentInParent<LineRenderer>().enabled = false;

            CompleteLevel();

            levelWon = true;
        }
    }

    private void SetUpConnections()
    {
        foreach (Connection con in connections)
        {
            GameObject firstPoint = con.GetFirstPoint();
            GameObject secondPoint = con.GetSecondPoint();

            if (firstPoint == null || secondPoint == null)
            {
                Debug.LogError("Connections not set up correctly!");
                return;
            }

            GameObject lineObject = Instantiate(linePrefab);
            con.SetLine(lineObject);
            LineRenderer line = lineObject.GetComponent<LineRenderer>();
            line.SetPosition(0, firstPoint.transform.position + Vector3.forward);
            line.SetPosition(1, secondPoint.transform.position + Vector3.forward);
        }
    }

    public List<Connection> GetConnections(GameObject point)
    {
        List<Connection> conns = new List<Connection>();

        foreach (Connection con in connections)
        {
            GameObject firstPoint = con.GetFirstPoint();
            GameObject secondPoint = con.GetSecondPoint();

            if (point.Equals(firstPoint) || point.Equals(secondPoint))
            {
                conns.Add(con);
            }
        }

        return conns;
    }

    public void IncreaseActiveConnections()
    {
        activeConnections++;
    }

    private Color GetRandomColor()
    {
        if (colorSet.Length == 0)
        {
            Debug.Log("Empty Color Set, generating color...");
            return Random.ColorHSV(0, 1, 0.7F, 0.7F, 0.75F, 0.75F);
        }

        return colorSet[Mathf.FloorToInt(Random.Range(0, colorSet.Length + 1))];
    }

    public Color GetLevelColor()
    {
        return levelColor;
    }

    public bool NoActiveConnections()
    {
        return activeConnections == 0;
    }

    private void CompleteLevel()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(pointTag))
        {
            GameObject particle = Instantiate(winParticle);
            ParticleSystem.MainModule main = particle.GetComponent<ParticleSystem>().main;
            main.startColor = levelColor;
            particle.transform.position = go.transform.position;
        }

        winScreen.SetActive(true);
    }

    public bool IsWon()
    {
        return levelWon;
    }
}
