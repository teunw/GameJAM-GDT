using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;

public class DrawManager : MonoBehaviour
{
    public static DrawManager instance;

    public Canvas Canvas;
    public GameObject dotParent;
    public RectTransform panel;
    //Copy Pasta'd Code
    private LineRenderer line;
    private Vector3 mousePos;
    [SerializeField]
    public Material material;
    private int currLines = 0;

    public bool doDraw;

    // Use this for initialization
        private Socket socket;

    // Use this for initialization
    void Start()
    {
        socket = IO.Socket("ws://teunwillems.nl:3000/");

        socket.On(Socket.EVENT_ERROR, () =>
        {
            Debug.Log("Error");
        });
        socket.On(Socket.EVENT_CONNECT, () =>
        {
            Debug.Log("Connected");
            socket.Emit("UnityClient", "asdf");
        });
        socket.On("location", (data) =>
        {
            Debug.Log(data);
            Location loc = JsonUtility.FromJson<Location>(data.ToString());
            Debug.Log(data.ToString());
        });
    }

    void OnDestroy()
    {
        socket.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!Input.GetMouseButton(0) || !doDraw)
        {
            line = null;
            return;
        }
        if (doDraw)
        {
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            Vector2 mpv2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 min = Vector2.zero;
            Vector2 max = panel.rect.max * 2;
            
            //Right & Top
            max = new Vector2(max.x + panel.offsetMin.x, max.y + panel.offsetMin.y);
            //Left & Bottom
            min = new Vector2(min.x + panel.offsetMin.x, min.y + panel.offsetMin.y);
            max *= Canvas.scaleFactor;
            min *= Canvas.scaleFactor;
            //min = new Vector2(min.x + panel.rect.left, min.y + panel.rect.top);
            if (!(min.x <= mpv2.x && max.x > mpv2.x && min.y <= mpv2.y && max.y > mpv2.y))
            {
                line = null;
                return;
            }
        }

        //Create line
        if (line == null)
        {
            createLine();
            return;
        }
        else
        {
            
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
        line.SetPosition(0, mousePos);
        line.SetPosition(1, mousePos);
        line.transform.SetParent(dotParent.transform);
        line.transform.position += new Vector3(0, 0, -0.01f);
    }

    public void setDraw()
    {
        doDraw = !doDraw;
    }

    public void clearDrawings()
    {
        foreach (Transform l in dotParent.transform)
        {
            Destroy(l.gameObject);
        }
        currLines = 0;
    }

    public void setColor(Material color)
    {
        material = color;
    }
}
