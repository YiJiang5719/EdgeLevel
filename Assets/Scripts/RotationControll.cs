using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward * Time.deltaTime * 90.0f;
        
        transform.Rotate(Input.GetAxis("Vertical"), 0.0f, Input.GetAxis("Horizontal"));
        //Debug.Log("FLy script added to" + gameObject.name);
    }
}
