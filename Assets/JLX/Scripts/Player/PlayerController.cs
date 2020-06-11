using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject entity;
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;

    float currentZSpeed;
    public float speedUpScaler = 0f;
    public float windScaler = 0f;

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
        entity.transform.rotation = Quaternion.Euler(0, 0, joy.Horizontal * 45);
        rig.velocity = new Vector3(joy.Horizontal * xSpeed, joy.Vertical * ySpeed, currentZSpeed * speedUpScaler * windScaler);
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
        {
            //Debug.Log("AnimSpeedUP Trigger is Set");
            anim.SetTrigger("SpeedUp");
        }
    }
}
