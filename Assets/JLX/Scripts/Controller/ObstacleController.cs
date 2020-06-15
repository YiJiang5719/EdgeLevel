using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleController : MonoBehaviour
{
    public GameObject nextObstacle;
    public GameObject TrailPrefab;

    [Range(0.5f, 2f)]
    public float trailLastingTime;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogError(other.tag);
        if (other.CompareTag("Player"))
        {
            Raycast();
            other.GetComponent<PlayerController>().ZSpeedUp();
            Destroy(gameObject);
        }
    }

    void Raycast()
    {
        //Debug.LogError("Raycast");
        var trail = Instantiate(TrailPrefab, transform.position, transform.rotation);
        Vector3 targetPos = nextObstacle.transform.position;
        trail.transform.DOMove(targetPos, trailLastingTime).Play();
    }
}
