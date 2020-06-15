using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed = 50;
    public Vector3 startPos = new Vector3(0, 0, 10);

    PlayerController pc;
    Rigidbody rig;


    private void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
        rig = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        transform.position = pc.transform.position + startPos;
    }

    private void FixedUpdate()
    {
        rig.velocity = (pc.transform.position - transform.position).normalized * speed;
    }
}
