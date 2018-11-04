using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour {

    private GameObject currentPoint;
    private List<Connection> currentPointConnections;
    private LineRenderer linePreview;
    [SerializeField]
    private GameObject activePointParticle;
    [SerializeField]
    private GameObject activatedPointParticle;

    private bool dragging;
    
	void Start ()
    {
        currentPoint = null;
        linePreview = GetComponentInParent<LineRenderer>();

    }

    void Update ()
    {
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            activePointParticle.SetActive(false);

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.tag == Level.pointTag)
                {
                    if (currentPoint == null)
                    {
                        currentPoint = hit.transform.gameObject;
                        currentPointConnections = Level.Instance.GetConnections(currentPoint);
                        dragging = true;
                    }
                    else
                    {
                        if (hit.transform.gameObject.Equals(currentPoint))
                        {
                            dragging = true;
                        }
                    }
                }
            }
        }
        
        if (Input.GetMouseButton(0) && dragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            linePreview.SetPosition(0, currentPoint.transform.position + Vector3.forward / 2);
            linePreview.SetPosition(1, new Vector3(mousePosition.x, mousePosition.y, 0.5F));

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.tag == Level.pointTag && hit.transform.gameObject != currentPoint)
                {
                    GameObject hitObject = hit.transform.gameObject;
                    foreach (Connection con in currentPointConnections)
                    {
                        if (con.IsActive())
                            continue;

                        if (con.GetFirstPoint().Equals(hitObject) || con.GetSecondPoint().Equals(hitObject))
                        {
                            con.SetActive();
                            Level.Instance.IncreaseActiveConnections();
                            currentPoint = hitObject;
                            currentPointConnections = Level.Instance.GetConnections(currentPoint);

                            if (!Level.Instance.IsWon())
                                SummonParticle(currentPoint.transform.position);
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Level.Instance.NoActiveConnections())
            {
                currentPoint = null;
            }
            else if (!Level.Instance.IsWon())
            {
                activePointParticle.transform.position = currentPoint.transform.position;
                ParticleSystem.MainModule main = activePointParticle.GetComponent<ParticleSystem>().main;
                main.startColor = Level.Instance.GetLevelColor();
                activePointParticle.SetActive(true);
            }

            linePreview.SetPosition(0, Vector3.zero);
            linePreview.SetPosition(1, Vector3.zero);
            dragging = false;
        }
    }

    private void SummonParticle(Vector3 position)
    {
        GameObject particle = Instantiate(activatedPointParticle);
        ParticleSystem.MainModule main = particle.GetComponent<ParticleSystem>().main;
        main.startColor = Level.Instance.GetLevelColor();
        particle.transform.position = position;
        StartCoroutine(DestroyParticle(particle));
    }

    private IEnumerator DestroyParticle(GameObject particle)
    {
        yield return new WaitForSeconds(2);
        Destroy(particle);
    }
}
