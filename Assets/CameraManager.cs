using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager singleton;
    [SerializeField] Transform[] checkpoints;
    public int currentCheckPoint;
    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, checkpoints[currentCheckPoint].position, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, checkpoints[currentCheckPoint].rotation, Time.deltaTime);
    }
}
