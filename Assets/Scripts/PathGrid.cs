using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PathGrid : MonoBehaviour
{
    public int width;
    public int height;

    [SerializeField] Node nodePrefab;
    bool hasStart = false;
    bool hasEnd = false;

    private void Awake()
    {
        Assert.IsNotNull(nodePrefab, "Missing Node prefab");
        DrawGrid();
    }

    public Node[,] grid;

    private void DrawGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var go = Instantiate(nodePrefab, transform);
                go.transform.localPosition = new Vector3(x, y);
            }
        }
    }

    private void Update()
    {
        
    }
}
