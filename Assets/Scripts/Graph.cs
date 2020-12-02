﻿using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefabs = default;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    Transform[] points;

    private void Awake()
    {
        float step = 2f / resolution;
        Vector3 position = Vector3.zero;
        var scale = Vector3.one * step;
        points = new Transform[resolution];

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefabs);

            position.x = ((i + 0.5f) * step - 1f);

            point.position = position;
            point.localScale = scale;

            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    private void Update()
    {
        float time = Time.time;
        for(int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];

            Vector3 position = point.localPosition;
            position.y = FunctionLibrary.Wave(position.x, time);

            point.localPosition = position;
        }
    }
}
