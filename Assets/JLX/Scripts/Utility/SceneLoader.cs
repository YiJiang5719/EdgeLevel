using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string name;
    public void LoadSceneAsync()
    {
        SoundManager.instance.MirrorBreak();
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(2f);
        var a = SceneManager.LoadSceneAsync(name);
        while (!a.isDone)
            yield return null;
    }
}
