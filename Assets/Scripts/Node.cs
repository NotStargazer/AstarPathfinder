using UnityEngine;
using UnityEngine.Assertions;

public class Node : MonoBehaviour
{
    private SpriteRenderer renderer;
    public int weight;
    public bool path;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(renderer, "Missing SpriteRender component");
        UpdateNode();
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
            nodeType = nodeType.Next();
            UpdateNode();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            nodeType = nodeType.Previous();
            UpdateNode();
        }
    }

    private void UpdateNode()
    {
        switch (nodeType)
        {
            case Type.path:
                renderer.color = Color.gray;
                weight = 1;
                break;
            case Type.blockage:
                renderer.color = Color.black;
                weight = 100;
                break;
            case Type.start:
                renderer.color = Color.green;
                weight = 100;
                break;
            case Type.end:
                renderer.color = Color.red;
                weight = 0;
                break;
        }
        transform.parent.GetComponent<PathGrid>().UpdateFlags();
    }
}
