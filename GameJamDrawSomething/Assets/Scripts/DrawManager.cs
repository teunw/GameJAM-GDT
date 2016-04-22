using UnityEngine;
using System.Collections;

public class DrawManager : MonoBehaviour
{
    public GameObject dotParent;

    //Copy Pasta'd Code
    private LineRenderer line;
    private Vector3 mousePos;
    [SerializeField]
    public Material material;
    private int currLines = 0;

    public bool doDraw;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!Input.GetMouseButton(0) || !doDraw) // or screen touched
        {
            line = null;
            return;
        }

        //Create line
        if (line == null)
        {
            createLine();
            return;
        }
        else
        {
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            line.SetPosition(1, mousePos);
            line = null;
            currLines++;

            createLine();
        }

    }

    public void createLine()
    {
        line = new GameObject("Line " + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.SetVertexCount(2);
        line.SetWidth(0.08f, 0.08f);
        line.useWorldSpace = true;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        line.SetPosition(0, mousePos);
        line.SetPosition(1, mousePos);
        line.transform.SetParent(dotParent.transform);
    }

    public void setDraw()
    {
        doDraw = !doDraw;
    }
}
