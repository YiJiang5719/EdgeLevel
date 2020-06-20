using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string SceneName;
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.isTrigger)
        {
            Debug.Log("SceneLoad!");
            SceneManager.LoadScene(SceneName);
        }
    }
}
