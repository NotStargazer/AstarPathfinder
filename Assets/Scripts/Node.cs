using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Node : MonoBehaviour
{
    private SpriteRenderer renderer;
    public int weight;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(renderer, "Missing SpriteRender component");
        UpdateNode();
    }

    public enum Type
    {
        blockage,
        water,
        path,
        bridge,
        start,
        end
    }

    public Type nodeType;

    private void OnMouseOver()
    {
        Debug.Log("Mouse is over object");
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
            case Type.water:
                renderer.color = Color.blue;
                weight = 3;
                break;
            case Type.bridge:
                renderer.color = new Color32(152, 118, 84, 255);
                weight = 2;
                break;
            case Type.start:
                renderer.color = Color.green;
                weight = 1;
                break;
            case Type.end:
                renderer.color = Color.red;
                weight = 0;
                break;
        }
    }
}
