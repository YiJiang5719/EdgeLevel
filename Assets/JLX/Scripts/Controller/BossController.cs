using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    enum State { Idle, Rush }
    State _state;
    State state
    {
        get
        {
            return _state;
        }

        set
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Rush:
                    break;
                default:
                    break;
            }
            _state = value;
            switch (_state)
            {
                case State.Idle:
                    skillTimer.StartTimer(skillCDTime);
                    rig.velocity = Vector3.forward * normalSpeed;
                    break;
                case State.Rush:
                    GetTargetPosition();
                    Vector3 dis = (targetPosition - transform.position);
                    rig.velocity = dis.normalized * rushSpeed;
                    rushTimer.StartTimer(dis.magnitude / rushSpeed);
                    break;
                default:
                    break;
            }
        }
    }

    public float normalSpeed = 20;
    public float rushSpeed = 50;
    public float skillCDTime = 5f;
    public Vector3 startPosOffset = new Vector3(0, 0, 10);
    public Vector3 zRushOffset = new Vector3(0, 0, 10);

    PlayerController pc;
    Rigidbody rig;
    Timer skillTimer = new Timer();
    Timer rushTimer = new Timer();
    Vector3 targetPosition;
    BlackHoleInsideScene manager;

    private void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
        manager = FindObjectOfType<BlackHoleInsideScene>();
        rig = GetComponent<Rigidbody>();

        skillTimer.OnTimerEnd += () => state = State.Rush;
        rushTimer.OnTimerEnd += () => state = State.Idle;
    }

    private void OnEnable()
    {
        transform.position = pc.transform.position + startPosOffset;
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                skillTimer.Update();
                break;
            case State.Rush:
                rushTimer.Update();
                break;
            default:
                break;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (state)
        {
            case State.Rush:
                if (collision.gameObject.CompareTag("Player"))
                {
                    Debug.Log("OnCollisionEnter");
                    manager.AddCounter();
                    pc.BossHit(collision.contacts[0].point);
                    state = State.Idle;
                }
                break;
        }
    }

    void GetTargetPosition()
    {
        targetPosition = pc.transform.position + zRushOffset;
    }

    bool IsEqual(Vector3 a, Vector3 b)
    {
        return (a - b).sqrMagnitude <= 0.01;
    }
}
