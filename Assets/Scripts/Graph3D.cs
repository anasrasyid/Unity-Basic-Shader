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

    private void Awake()
    {
        // initialize some variable to help create points
        float step = 2f / resolution;
        Vector3 position = Vector3.zero;
        var scale = Vector3.one * step;
        points = new Transform[resolution * resolution];

        // Create Points and keep in array
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
            }
            Transform point = Instantiate(point3DPrefabs);

            position.x = ((x + 0.5f) * step - 1f);
            position.z = ((z + 0.5f) * step - 1f);

            point.position = position;
            point.localScale = scale;

            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    private void Update()
    {
        // the function and current time
        FunctionLibrary.Function3D f = FunctionLibrary.GetFunction3D(function);
        float time = Time.time;

        // Animating Points
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];

            Vector3 position = point.localPosition;
            position.y = f(position.x, position.z, time);

            point.localPosition = position;
        }
    }
}
