using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {

    [SerializeField]
    private float fadeTime = 1f;
    [SerializeField]
    private AnimationCurve fadeCurve;
    [SerializeField]
    private int nextLevel;
    [SerializeField]
    private Image img;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    private void FadeTo(int sceneIndex)
    {
        StartCoroutine(FadeOut(sceneIndex));
    }

    public void LoadNextLevel()
    {
        img.GetComponentInParent<Canvas>().sortingOrder = 1;

        if (nextLevel < 10)
        {
            FadeTo("Level0" + nextLevel);
        }
        else
        {
            FadeTo("Level" + nextLevel);
        }
    }

    public void GoToMenu()
    {
        img.GetComponentInParent<Canvas>().sortingOrder = 1;
        FadeTo("MainMenu");
    }

    public void ReloadLevel()
    {
        FadeTo(SceneManager.GetActiveScene().name);
    }

    private IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime / fadeTime;
            float alpha = fadeCurve.Evaluate(t);
            img.color = new Color(0.141176f, 0.141176f, 0.141176f, alpha);
            yield return 0;
        }
    }

    private IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeTime;
            float alpha = fadeCurve.Evaluate(t);
            img.color = new Color(0.141176f, 0.141176f, 0.141176f, alpha);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }

    private IEnumerator FadeOut(int sceneIndex)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeTime;
            float alpha = fadeCurve.Evaluate(t);
            img.color = new Color(0.141176f, 0.141176f, 0.141176f, alpha);
            yield return 0;
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
