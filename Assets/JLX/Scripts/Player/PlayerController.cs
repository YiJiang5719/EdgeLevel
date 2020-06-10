using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;

    [HideInInspector]
    public float currentZSpeed;
    //[HideInInspector]
    public float zSpeedScaler = 0f;

    Rigidbody rig;
    Animator anim;
    UIJoystick joy;

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
        currentZSpeed = zSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rig.velocity = new Vector3(joy.Horizontal * xSpeed, joy.Vertical * ySpeed, currentZSpeed * zSpeedScaler);
        //Debug.LogError("Zspeed: "  + currentZSpeed * zSpeedScaler);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ZSpeedUp();
        }
    }

    public void ZSpeedUp()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SpeedUp"))
            Debug.Log("AnimSpeedUP Trigger is Set");
            anim.SetTrigger("SpeedUp");
    }
}
