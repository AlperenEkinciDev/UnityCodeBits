using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerObject : MonoBehaviour
{
    [SerializeField] Material fadeMaterial;
    [SerializeField] float sceneSequenceDuration;
    [SerializeField] float sceneSequenceBlackOffset;

    private float sceneSequenceSequenceStartTime;

    public void LoadSceneSequence(int i)
    {
        sceneSequenceSequenceStartTime = Time.time;

        StartCoroutine(LoadSequence(i));
    }

    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    IEnumerator LoadSequence(int i)
    {

        Color newColor = fadeMaterial.color;

        while (Time.time - sceneSequenceSequenceStartTime < sceneSequenceDuration)
        {
            float alpha = EaseMethods.EaseOutQuad(Mathf.Clamp((Time.time - sceneSequenceSequenceStartTime + sceneSequenceBlackOffset) / sceneSequenceDuration, 0.0f, 1.0f));

            newColor.a = alpha;
            fadeMaterial.color = newColor;

            yield return new WaitForEndOfFrame();
        }
        LoadScene(i);
        newColor.a = 0f;
        fadeMaterial.color = newColor;
    }
}
