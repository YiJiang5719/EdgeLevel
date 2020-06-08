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
    void Update()
    {
        rig.velocity = new Vector3(joy.Horizontal * xSpeed, joy.Vertical * ySpeed, currentZSpeed);
    }

    public void ZSpeedUp()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SpeedUp"))
            anim.SetTrigger("SpeedUp");
    }
}
