using UnityEngine;
using UnityEngine.Assertions;

public class Node : MonoBehaviour
{
    private SpriteRenderer renderer;
    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost { get => hCost + gCost; }

    public int xPos { private get; set; }
    public int yPos { private get; set; }
    public Vector2Int pos { get => new Vector2Int(xPos, yPos); }

    public Node parent;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(renderer, "Missing SpriteRender component");
        UpdateType();
    }

    public enum Type
    {
        blockage,
        path,
        start,
        end
    }

    public Type nodeType;

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (nodeType == Type.path)
            {
                nodeType = Type.blockage;
            }
            else
            {
                nodeType = Type.path;
            }
            UpdateType();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (nodeType == Type.start)
            {
                nodeType = Type.end;
            }
            else
            {
                nodeType = Type.start;
            }
            UpdateType();
            UpdateType();
        }
    }

    private void OnMouseEnter()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (nodeType == Type.path)
            {
                nodeType = Type.blockage;
            }
            else
            {
                nodeType = Type.path;
            }
            UpdateType();
        }
    }

    private void UpdateType()
    {
        switch (nodeType)
        {
            case Type.path:
                renderer.color = Color.gray;
                break;
            case Type.blockage:
                renderer.color = Color.black;
                break;
            case Type.start:
                renderer.color = Color.green;
                break;
            case Type.end:
                renderer.color = Color.red;
                break;
        }
        transform.parent.GetComponent<PathGrid>().UpdateFlags();
    }
}
