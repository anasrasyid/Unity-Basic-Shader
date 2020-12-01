using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefabs = default;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    private void Awake()
    {
        float step = 2f / resolution;
        Vector3 position = Vector3.zero;
        var scale = Vector3.one * step;
        for (int i = 0; i < resolution; i++)
        {
            Transform point = Instantiate(pointPrefabs);

            position.x = ((i + 0.5f) * step - 1f);
            position.y = position.x * position.x * position.x;

            point.position = position;
            point.localScale = scale;

            point.SetParent(transform, false);
        }
    }
}
