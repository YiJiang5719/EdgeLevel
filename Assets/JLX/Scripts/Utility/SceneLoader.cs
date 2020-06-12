using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string name;
    public void LoadSceneAsync()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        var a = SceneManager.LoadSceneAsync(name);
        a.allowSceneActivation = false;
        yield return a.isDone;
        yield return new WaitForSeconds(1);
        a.allowSceneActivation = true;
    }
}
