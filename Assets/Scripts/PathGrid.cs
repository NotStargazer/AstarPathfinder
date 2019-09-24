using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(LineRenderer))]
public class PathGrid : MonoBehaviour
{
    public int width;
    public int height;

    LineRenderer line;
    [SerializeField] Node nodePrefab;
    bool hasStart = false;
    bool hasEnd = false;

    public Node[,] grid;

    private void Awake()
    {
        grid = new Node[width, height];
        line = GetComponent<LineRenderer>();
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
                go.xPos = x;
                go.yPos = y;
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

    public void PressButton()
    {
        StopAllCoroutines();
        StartCoroutine(DrawPath());
    }

    IEnumerator DrawPath()
    {
        line.positionCount = 0;
        if (hasEnd || hasStart)
        {
            var path = GetPathPoints(FindEnd(), FindStart());

            if (path == null)
            {
                UIController.instance.ImpossiblePath(true);
            }
            else
            {
                UIController.instance.ImpossiblePath(false);
                for (int i = 0; i < path.Length; i++)
                {
                    line.positionCount++;
                    line.SetPosition(i, path[i]);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        else
        {
            line.positionCount = 0;
        }
    }

    private Vector3[] GetPathPoints(Vector2Int endPoint, Vector2Int startPoint)
    {

        Node startNode = grid[startPoint.x, startPoint.y];
        Node endNode = grid[endPoint.x, endPoint.y];

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost <= currentNode.fCost)
                {
                    if (openSet[i].hCost < currentNode.hCost)
                        currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                return TracePath(startNode, endNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (neighbor.nodeType == Node.Type.blockage || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int movementCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (movementCost < currentNode.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = movementCost;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return TracePath(startNode, endNode);
    }

    Vector3[] TracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            if (currentNode.nodeType == Node.Type.blockage || !currentNode.parent)
            {
                return null;
            }
            path.Add(currentNode.transform.position + Vector3.one / 2);
            currentNode = currentNode.parent;
        }

        path.Add(startNode.transform.position + Vector3.one / 2);

        path.Reverse();

        return path.ToArray();
    }

    int GetDistance(Node startNode, Node targetNode)
    {
        int disX = Mathf.Abs(startNode.pos.x - targetNode.pos.x);
        int disY = Mathf.Abs(startNode.pos.y - targetNode.pos.y);

        if (disX > disY)
        {
            return 14 * disY + 10 * (disX - disY);
        }
        return 14 * disX + 10 * (disY - disX);
    }

    List<Node> GetNeighbors(Node targetNode)
    {
        List<Node> neighbors = new List<Node>();
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

        foreach (var item in moveDirections)
        {
            int x = targetNode.pos.x + item.x;
            int y = targetNode.pos.y + item.y;
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                neighbors.Add(grid[x, y]);
            }
        }

        return neighbors;
    }

    private Vector2Int FindStart()
    {
        foreach (var item in grid)
        {
            if (item.nodeType == Node.Type.start)
            {
                return item.pos;
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
                return item.pos;
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
