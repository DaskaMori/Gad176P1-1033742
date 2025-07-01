using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    public Transform center;
    public float radius = 1f;
    public float spinSpeed = 10f;
    private float angle;

    private void Update()
    {
        angle += spinSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        transform.position = center.position + new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
