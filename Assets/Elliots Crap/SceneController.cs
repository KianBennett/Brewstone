using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject black;
    private Animator screenFade;
    void Start()
    {
        black.SetActive(true);
        screenFade = black.gameObject.GetComponent<Animator>();
    }
    public void LoadScene(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }
    IEnumerator FadeOut(string scene)
    {
        screenFade.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene(scene);
    }

    public void Quit() {
        Application.Quit();
    }
}
