using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] TextMeshProUGUI start;
    [SerializeField] TextMeshProUGUI end;
    [SerializeField] TextMeshProUGUI impossible;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateText(bool endOn, bool startOn)
    {
        end.faceColor = endOn ? Color.green : Color.white;
        start.faceColor = startOn ? Color.green : Color.white;
    }

    public void ImpossiblePath(bool path)
    {
        impossible.color = path ? Color.red : new Color(0, 0, 0, 0);
    }
}
