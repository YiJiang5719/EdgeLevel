using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackHoleInsideScene : MonoBehaviour
{
    public float bossAppearTime = 2f;
    public float winDistance = 100f;

    Timer bossAppearTimer = new Timer();
    BossController bc;
    PlayerController pc;

    // Start is called before the first frame update
    void Awake()
    {
        bc = FindObjectOfType<BossController>();
        pc = FindObjectOfType<PlayerController>();

        bossAppearTimer.OnTimerStart += HideBoss;
        bossAppearTimer.OnTimerEnd += ShowBoss;

        bossAppearTimer.StartTimer(bossAppearTime, false);
    }

    // Update is called once per frame
    void Update()
    {
        bossAppearTimer.Update();
        if (IsWin())
            AfterWin();
    }

    public void ShowBoss()
    {
        //Debug.Log("ShowBoss");
        bc.gameObject.SetActive(true);
    }
    public void HideBoss()
    {
        //Debug.Log("HideBoss");
        bc.gameObject.SetActive(false);
    }

    public bool IsWin()
    {
        var dis = (pc.transform.position - bc.transform.position).magnitude;
        return dis > winDistance;
    }

    public void AfterWin()
    {
        SceneManager.LoadScene("FinalWorld");
    }
}
