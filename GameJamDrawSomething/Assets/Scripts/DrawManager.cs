using System;
using Assets.Scripts;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;

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

    public void createLine(Vector3 loc1, Vector3 loc2)
    {
        line = new GameObject("Line " + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.SetVertexCount(2);
        line.SetWidth(0.08f, 0.08f);
        line.useWorldSpace = true;
        mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        line.SetPosition(0, loc1);
        line.SetPosition(1, loc2);
        line.transform.SetParent(dotParent.transform);
    }

    public void setDraw()
    {
        doDraw = !doDraw;
    }
}
