using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { Normal, Break }
    private State _state;
    private State state
    {
        get
        {
            return _state;
        }
        set
        {
            switch (_state)
            {
                case State.Normal:
                    //rig.velocity = Vector3.zero;
                    break;
                case State.Break:
                    break;
                default:
                    break;
            }
            _state = value;
            switch (_state)
            {
                case State.Normal:
                    break;
                case State.Break:
                    anim.SetTrigger("Break");
                    break;
                default:
                    break;
            }
        }
    }


    public GameObject entity;
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;
    public float bossBreakRecoverTime;
    public float windBreakRecoverTime;
    public float explosionScaler = 10000f;

    public float speedUpScaler = 0f;
    public float windScaler = 0f;
    public float finalSpeed = 0f;

    Rigidbody rig;
    Animator anim;
    UIJoystick joy;
    Timer breakRecoverTimer = new Timer();

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        joy = FindObjectOfType<UIJoystick>();

        breakRecoverTimer.OnTimerEnd += () => { state = State.Normal; };
    }

    // Start is called before the first frame update
    void Start()
    {
        rig.velocity = new Vector3(0, 0, zSpeed);
    }

    private void Update()
    {
        breakRecoverTimer.Update();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (_state)
        {
            case State.Normal:
                entity.transform.rotation = Quaternion.Euler(0, 0, joy.Horizontal * 45);
                finalSpeed = zSpeed * speedUpScaler * windScaler;
                rig.velocity = new Vector3(joy.Horizontal * xSpeed, joy.Vertical * ySpeed, finalSpeed);
                break;
        }
    }

    public void ZSpeedUp()
    {
        switch (_state)
        {
            case State.Normal:
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SpeedUp"))
                {
                    //Debug.Log("AnimSpeedUP Trigger is Set");
                    anim.SetTrigger("SpeedUp");
                }
                break;
        }
    }



    public void WindHit(Vector3 point)
    {
        switch (_state)
        {
            case State.Normal:
                state = State.Break;
                breakRecoverTimer.Reset();
                breakRecoverTimer.StartTimer(windBreakRecoverTime, false);
                rig.AddExplosionForce(explosionScaler, point, 10000);
                break;
        }
    }

    public void BossHit(Vector3 point)
    {
        switch (_state)
        {
            case State.Normal:
                state = State.Break;
                breakRecoverTimer.Reset();
                breakRecoverTimer.StartTimer(bossBreakRecoverTime, false);
                FindObjectOfType<CameraShake>().Shake();
                rig.AddExplosionForce(explosionScaler, point, 10000);
                break;
        }
    }
}
