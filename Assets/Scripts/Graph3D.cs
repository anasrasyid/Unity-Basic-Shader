using UnityEngine;

public class Graph3D : MonoBehaviour
{
    [SerializeField]
    Transform point3DPrefabs = default;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    Transform[] points;

    [SerializeField]
    FunctionLibrary.FunctionName function = default;

    public enum TransitionMode { Cycle, Random}
    [SerializeField]
    TransitionMode transitionMode = TransitionMode.Cycle;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    float duration;
    bool transioning;
    FunctionLibrary.FunctionName transitionFunction;

    private void Awake()
    {
        // initialize some variable to help create points
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        points = new Transform[resolution * resolution];

        // Create Points and keep in array
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(point3DPrefabs);
            
            point.localScale = scale;

            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    private void Update()
    {
        duration += Time.deltaTime;
        
        // Function Change based on condition
        if (transioning && duration >= transitionDuration)
        {
            duration -= functionDuration;
            transioning = false;
        }
        else if(duration >= functionDuration)
        {
            duration -= functionDuration;
            transioning = true;
            transitionFunction = function;
            PickNextFunction();
        }

        // Update Function
        if (transioning)
            UpdateFunctionTransition();
        else
            UpdateFunction();
    }

    void PickNextFunction()
    {
        function = FunctionLibrary.GetNextFunctionName(function);
        if(transitionMode == TransitionMode.Random)
            function = FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }

    private void UpdateFunction()
    {
        // the function and current time
        FunctionLibrary.Function3D f = FunctionLibrary.GetFunction3D(function);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;

        // Animating Points
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if(x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            
            points[i].localPosition = f(u, v, time);
        }
    }

    private void UpdateFunctionTransition()
    {
        // get from and to function 
        FunctionLibrary.Function3D from = FunctionLibrary.GetFunction3D(transitionFunction);
        FunctionLibrary.Function3D to = FunctionLibrary.GetFunction3D(function);

        // Get Progress
        float progress = duration / transitionDuration;

        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;

        // Animating Points
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;

            points[i].localPosition = FunctionLibrary.Morph(
                u, v, time, from, to, progress);
        }
    }
}
