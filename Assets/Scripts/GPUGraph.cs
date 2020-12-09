using UnityEngine;

public class GPUGraph : MonoBehaviour
{

    [SerializeField, Range(10, 200)]
    int resolution = 10;

    [SerializeField]
    ComputeShader computeShader = default;

    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");

    [SerializeField]
    FunctionLibrary.FunctionName function = default;

    public enum TransitionMode { Cycle, Random }
    [SerializeField]
    TransitionMode transitionMode = TransitionMode.Cycle;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    float duration;
    bool transioning;
    FunctionLibrary.FunctionName transitionFunction;

    ComputeBuffer positionBuffer;

    private void OnEnable()
    {
        positionBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
    }

    private void OnDisable()
    {
        positionBuffer.Release();
        positionBuffer = null;
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
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;
            transioning = true;
            transitionFunction = function;
            PickNextFunction();
        }

        UpdateFunctionGPU();
    }

    void UpdateFunctionGPU()
    {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);

        computeShader.SetBuffer(0, positionsId, positionBuffer);
        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);
    }

    void PickNextFunction()
    {
        function = FunctionLibrary.GetNextFunctionName(function);
        if (transitionMode == TransitionMode.Random)
            function = FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }
}
