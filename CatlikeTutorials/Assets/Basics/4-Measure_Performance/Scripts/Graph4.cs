using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph4 : MonoBehaviour
{
    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(10,100)] int resolution = 10;
    Transform[] points;
    [SerializeField] FunctionLibrary.FunctionName function;
    public enum TransitionMode { Cycle, Random }
    [SerializeField] TransitionMode transitionMode;
    [SerializeField, Min(0f)] float transitionDuration = 1f;
    bool transitioning = false;
    FunctionLibrary.FunctionName transitionFunction;

    [SerializeField, Min(0f)] float functionDuration = 1f;
    float duration = 0f;

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
        duration += Time.deltaTime;
        if (transitioning && duration >= transitionDuration)
        {
            duration -= transitionDuration;
            transitioning = false;
        }
        else if (duration >= functionDuration) { 
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = function;
            PickNextFunction();
        }
        if (transitioning) { UpdateGraphTransitioning(); }
        else { UpdateGraph(); }
    }

    void PickNextFunction()
    {
        if (transitionMode == TransitionMode.Cycle) {
            function = FunctionLibrary.GetNextFunctionName(function);
        }
        else
        {
            function = FunctionLibrary.GetRandomFunctionNameOtherThan(function);
        }
    }

    private void UpdateGraph()
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

    private void UpdateGraphTransitioning()
    {
        FunctionLibrary.Function from = FunctionLibrary.GetFunction(transitionFunction);
        FunctionLibrary.Function to = FunctionLibrary.GetFunction(function);
        float progress = duration / transitionDuration;
        
        float time = Time.time;
        float step = 2f / resolution;
        
        float v = 0.5f * step - 1f;
        for (int z = 0; z < resolution; z++)
        {
            v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++)
            {
                float u = (x + 0.5f) * step - 1f;
                Transform p = points[x * resolution + z];
                p.localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
            }
        }
    }
}
