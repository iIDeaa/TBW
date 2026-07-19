using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroScene : MonoBehaviour
{
    public string nextScene;
    public float delay = 3f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextScene);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}