using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Animator circleTransition;

    public void LoadGame()
    {
        circleTransition.SetTrigger("Load");
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
