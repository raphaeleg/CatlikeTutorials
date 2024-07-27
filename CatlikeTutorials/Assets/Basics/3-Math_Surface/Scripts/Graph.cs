using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10,100)] int resolution = 10;
    Transform[] points;

    private void Awake()
    {
        float step = 2f / resolution;
        Vector3 position = Vector3.zero;
        var scale = Vector3.one * step;
        points = new Transform[resolution];
        for (int i = 0; i < resolution; i++)
        {
            Transform p = Instantiate(pointPrefab);
            p.SetParent(transform, false);
            position.x = (i+0.5f)*step-1f;
            p.localPosition = position;
            p.localScale = scale;
            points[i] = p;
        }
    }
    private void Update()
    {
        float time = Time.time;
        for (int i = 0; i < resolution; i++)
        {
            Transform p = points[i];
            var position = p.localPosition;
            //position.y = position.x * position.x * position.x;
            position.y = Mathf.Sin(Mathf.PI * (position.x +time));
            p.localPosition = position;
        }
    }
}
