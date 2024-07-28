using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph3 : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10,100)] int resolution = 10;
    Transform[] points;
    [SerializeField] FunctionLibrary.FunctionName function;

    private void Awake()
    {
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        points = new Transform[resolution*resolution];
        for (int i = 0; i < points.Length; i++)
        {
            Transform p = Instantiate(pointPrefab);
            p.SetParent(transform, false);
            p.localScale = scale;
            points[i] = p;
        }
    }
    private void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float step = 2f / resolution;
        float time = Time.time;
        float v = 0.5f * step - 1f;
        for (int z = 0; z < resolution; z++)
        {
            v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++)
            {
                float u = (x + 0.5f) * step - 1f;
                Transform p = points[x * resolution + z];
                p.localPosition = f(u, v, time);
            }
        }
    }
}
