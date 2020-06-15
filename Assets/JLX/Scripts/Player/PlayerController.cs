using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject entity;
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;
    public float controlRecoverTime;

    float speedUpScaler = 0f;
    float windScaler = 0f;
    float finalSpeed = 0f;
    bool isControl = true;

    Rigidbody rig;
    Animator anim;
    UIJoystick joy;
    Timer controlRecoverTimer;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        joy = FindObjectOfType<UIJoystick>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rig.velocity = new Vector3(0, 0, zSpeed);
        controlRecoverTimer = new Timer();
        controlRecoverTimer.OnTimerEnd += () => { /*Debug.LogError("TimerEnd"); */isControl = true; };
    }

    private void Update()
    {
        controlRecoverTimer.Update();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isControl)
            return;
        entity.transform.rotation = Quaternion.Euler(0, 0, joy.Horizontal * 45);
        finalSpeed = zSpeed * speedUpScaler * windScaler;
        rig.velocity = new Vector3(joy.Horizontal * xSpeed, joy.Vertical * ySpeed, finalSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isControl)
            return;
    }

    public void ZSpeedUp()
    {
        if (!isControl)
            return;
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SpeedUp"))
        {
            //Debug.Log("AnimSpeedUP Trigger is Set");
            anim.SetTrigger("SpeedUp");
        }
    }

    public float explosionScaler = 10000f;
    public void Hit(Vector3 point)
    {
        isControl = false;
        controlRecoverTimer.Reset();
        controlRecoverTimer.StartTimer(controlRecoverTime, false);
        rig.velocity = Vector3.zero;
        //Debug.Log("AddExplosionForce");
        rig.AddExplosionForce(explosionScaler, point, 10000);
    }
}
