using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorHub : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    [SerializeField]
    public Vector3 offset;

    public void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.eulerAngles = Camera.main.transform.eulerAngles;
    }
}
