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

    public Node[,] grid;

    private void Awake()
    {
        grid = new Node[width, height];
        Assert.IsNotNull(nodePrefab, "Missing Node prefab");
        DrawGrid();
    }



    private void DrawGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var go = Instantiate(nodePrefab, transform);
                go.transform.localPosition = new Vector3(x, y);
                grid[x, y] = go;
            }
        }
    }

    public void UpdateFlags()
    {
        int ends = 0;
        int starts = 0;
        foreach (var item in grid)
        {
            if (item.nodeType == Node.Type.end)
            {
                ends++;
            }
            if (item.nodeType == Node.Type.start)
            {
                starts++;
            }
        }

        hasEnd = ends == 1 ? true : false;
        hasStart = starts == 1 ? true : false;

        UIController.instance.UpdateText(hasEnd, hasStart);
    }

    public void DrawPath()
    {
        if (!hasEnd || !hasStart)
        {
            return;
        }

        foreach (var item in grid)
        {
            item.path = false;
        }

        var path = GetPathPoints();

        if (path == null)
        {
            UIController.instance.ImpossiblePath(true);
        }
        else
        {
            UIController.instance.ImpossiblePath(false);
            GetComponent<LineRenderer>().positionCount = path.Length;
            GetComponent<LineRenderer>().SetPositions(path);
        }
    }

    private Vector3[] GetPathPoints()
    {
        Vector2Int currentPoint = FindStart();
        Vector2Int endPoint = FindEnd();
        Vector2Int[] moveDirections = {
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(1, -1),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
        };
        List<Vector3> points = new List<Vector3>();
        float pathLength = 0;

        points.Add(grid[currentPoint.x, currentPoint.y].transform.position + new Vector3(0.5f, 0.5f));

        while (true)
        {
            List<Node> pointsInArea = new List<Node>();
            foreach (var item in moveDirections)
            {
                try
                {
                    if (!grid[currentPoint.x + item.x, currentPoint.y + item.y].path && grid[currentPoint.x + item.x, currentPoint.y + item.y].nodeType != Node.Type.blockage)
                    {
                        pointsInArea.Add(grid[currentPoint.x + item.x, currentPoint.y + item.y]);
                    }
                }
                catch (Exception)
                {
                }
            }

            if (pointsInArea.Count == 0)
            {
                if (grid[currentPoint.x, currentPoint.y].nodeType == Node.Type.start)
                {
                    return null;
                }
                grid[currentPoint.x, currentPoint.y].path = true;
                points.RemoveAt(points.Count - 1);
                currentPoint = new Vector2Int(Mathf.RoundToInt(points[points.Count - 1].x - transform.position.x - 0.5f), Mathf.RoundToInt(points[points.Count - 1].y - transform.position.y - 0.5f));
                if (currentPoint == FindStart())
                {
                    return null;
                }
                continue;
            }

            float currentDis = 0;
            float currentLowestWeight = Mathf.Infinity;
            Node currentNode = pointsInArea[0];

            foreach (var item in pointsInArea)
            {
                float cost = item.weight;
                float dis = Vector2Int.Distance(new Vector2Int(Mathf.RoundToInt(item.transform.localPosition.x), Mathf.RoundToInt(item.transform.localPosition.y)), endPoint);
                if ((dis + cost) < currentLowestWeight)
                {
                    currentLowestWeight = dis + cost;
                    currentNode = item;
                    currentDis = dis;
                }
            }

            points.Add(currentNode.transform.position + new Vector3(0.5f, 0.5f));
            currentPoint = new Vector2Int(Mathf.RoundToInt(currentNode.transform.localPosition.x), Mathf.RoundToInt(currentNode.transform.localPosition.y));
            currentNode.path = true;

            pathLength += currentDis;

            if (currentNode.nodeType == Node.Type.end)
            {
                break;
            }
        }
        return points.ToArray();
    }

    private Vector2Int FindStart()
    {
        foreach (var item in grid)
        {
            if (item.nodeType == Node.Type.start)
            {
                return new Vector2Int(Mathf.RoundToInt(item.transform.localPosition.x), Mathf.RoundToInt(item.transform.localPosition.y));
            }
        }

        return new Vector2Int();
    }

    private Vector2Int FindEnd()
    {
        foreach (var item in grid)
        {
            if (item.nodeType == Node.Type.end)
            {
                return new Vector2Int(Mathf.RoundToInt(item.transform.localPosition.x), Mathf.RoundToInt(item.transform.localPosition.y));
            }
        }

        return new Vector2Int();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(width, 0));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, height));
        Gizmos.DrawLine(transform.position + new Vector3(width, height), transform.position + new Vector3(width, 0));
        Gizmos.DrawLine(transform.position + new Vector3(width, height), transform.position + new Vector3(0, height));
    }
}
