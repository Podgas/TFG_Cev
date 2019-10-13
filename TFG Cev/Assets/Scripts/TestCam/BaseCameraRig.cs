using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCameraRig : MonoBehaviour
{

    public Transform _Transform = null;

    public Camera _Camera;
    public int _Mode = 0;
    protected bool mLockMode = false;

    public Transform _Target;
    public Vector3 _TargetOffset = new Vector3(0f, 20f, 0f);

    public Vector3 targetPos;  

    public const float MIN_PITCH = -87.4f;
    public const float MAX_PITCH = 87.4f;


    protected virtual void Awake()
    {

        _Transform = transform;

        _Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _Target = GameObject.FindGameObjectWithTag("Player").transform;
        targetPos = _Target.position + (_Target.rotation * _TargetOffset);
    }

    protected virtual void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
