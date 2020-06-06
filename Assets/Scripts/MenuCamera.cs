using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 desirePosition;
    private Quaternion desireRotation;

    public Transform shopWayPoint;
    public Transform levelWayPoint;

    private void Start()
    {
        startPosition = desirePosition = transform.localPosition;
        startRotation = desireRotation = transform.localRotation;
    }

    private void Update()
    {

        float x = Manager.Instance.GetPlayerInput().x;


        transform.localPosition = Vector3.Lerp(transform.localPosition, desirePosition + new Vector3(0,x,0) * 0.01f, 0.1f);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, desireRotation, 0.1f);
    }

    public void BackToMainMenu()
    {
        desirePosition = startPosition;
        desireRotation = startRotation;
    }

    public void MoveToShop()
    {
        desirePosition = shopWayPoint.localPosition;
        desireRotation = shopWayPoint.localRotation;

    }


    public void MoveToLevel()
    {
        desirePosition = levelWayPoint.localPosition;
        desireRotation = levelWayPoint.localRotation;
    }
}
