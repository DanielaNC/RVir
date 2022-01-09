﻿using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float defaultLength = 3.0f;

    public LineRenderer lineRenderer = null;

    private void Awake()
    {
        //lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLength();
    }

    private void UpdateLength()
    { 
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, GetEnd());
    }

    protected virtual Vector3 GetEnd()
    {
        return CalculateEnd(defaultLength);
    }

    protected Vector3 CalculateEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }
}
