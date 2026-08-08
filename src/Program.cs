using Engine.Graphics;
using Engine.Graphics.Shaders;
using Engine.User;
using Utils.CMath;
using System.Numerics;
using static System.Console;

unsafe class Entry
{
    public static void Main(string[] args)
    {
        Application.Init(1200, 800);
        Application.MainApp.AddLoad(Load);
        Video.RenderCallbacks.Add(Render);
        Application.Start();
    }

    public static ShaderProgram program, prog2;
    public static VertexArray Vao, Vao2;
    public static Transform transform = new Transform();
    public static Camera camera = new Camera(new Transform(),60f);
    public static Transform* camtrans;
    public static bool leftdown = false;
    public static bool rightdown = false;
    private static float movespeed = 5f;
    private static float sensitivity = 0.4f;

    public static void Load()
    {   
        /*
        // Quad vertices
        float[] verts =
        [//  x      y     z         r     g     b
            -1.0f,  1.0f, 0.0f,     1.0f, 0.0f, 0.0f, // top left
             1.0f,  1.0f, 0.0f,     0.0f, 1.0f, 0.0f, // top right
            -1.0f, -1.0f, 0.0f,     0.0f, 0.0f, 1.0f, // bottom left
             1.0f, -1.0f, 0.0f,     1.0f, 1.0f, 0.0f, // bottom right
        ];

        uint[] indices =
        [
            0, 1, 2,
            1, 2, 3
        ];
        */

        float[] verts = { // cube
            // front
            -1.0f, -1.0f,  1.0f,    1f, 0f, 0f,
             1.0f, -1.0f,  1.0f,    0f, 1f, 0f,
             1.0f,  1.0f,  1.0f,    0f, 0f, 1f,
            -1.0f,  1.0f,  1.0f,    1f, 1f, 0f,
            // back
            -1.0f, -1.0f, -1.0f,    1f, 0f, 0f,
             1.0f, -1.0f, -1.0f,    0f, 1f, 0f,
             1.0f,  1.0f, -1.0f,    0f, 0f, 1f,
            -1.0f,  1.0f, -1.0f,    1f, 1f, 0f,
        };

        uint[] indices =
        {
            // front
            0, 1, 2,
            2, 3, 0,
            // right
            1, 5, 6,
            6, 2, 1,
            // back
            7, 6, 5,
            5, 4, 7,
            // left
            4, 0, 3,
            3, 7, 4,
            // bottom
            4, 5, 1,
            1, 0, 4,
            // top
            3, 2, 6,
            6, 7, 3
        };

        Vao = new VertexArray(verts, 6);
        Vao.SetIndices(indices);
        Vao.SetAttribute(0, 3, VertAttribType.Float);
        Vao.SetAttribute(1, 3, VertAttribType.Float);
        
        Mesh3D mesh = new Mesh3D(Vao);
        var inst = mesh.NewInstance();

        mesh.Draw();

        float[] tri =
        {
            -1.0f,  1.0f, 0.0f,     1.0f, 0.0f, 0.0f, // top left
             1.0f,  1.0f, 0.0f,     0.0f, 1.0f, 0.0f, // top right
            -1.0f, -1.0f, 0.0f,     0.0f, 0.0f, 1.0f, // bottom left
             1.0f, -1.0f, 0.0f,     1.0f, 1.0f, 0.0f, // bottom right
        };

        uint[] triInds =
        {
            0, 1, 2,
            1, 2, 3
        };

        Vao2 = new VertexArray(tri,6);
        Vao2.SetIndices(triInds);
        Vao2.SetAttribute(0, 3, VertAttribType.Float);
        Vao2.SetAttribute(1, 3, VertAttribType.Float);

        Shader vert = new Shader("vertex/projection.vert");
        Shader frag = new Shader("fragment/simple.frag");
        program = new ShaderProgram([vert, frag]);

        Input.AddDownCallback(Key.K, ToggleWireframe);
        Input.AddMouseMoveCallback(CameraMovement);

        fixed ( Transform* t = &camera.transform )
            camtrans = t;
        camera.transform.Rotation = new Vector3(0, CMath.rad(270), 0);
    }

    public static void Render(double delta)
    {
        float felta = (float)delta;

        program.Use();
        double T = Application.MainApp.window.Time;
        transform.Scale = new Vector3(0.25f,0.25f,0.25f);
        transform.Rotation += new Vector3(0.03f, 0.03f, 0f);

        float move = 0;


        Vector2 rot = new Vector2();

        rot.X += Input.GetKey(Key.Left)? 0.02f : 0;
        rot.X += Input.GetKey(Key.Right)? -0.02f : 0;

        bool w, a, s, d, q, e;
        w = Input.GetKey(Key.W);
        s = Input.GetKey(Key.S);
        a = Input.GetKey(Key.A);
        d = Input.GetKey(Key.D);
        q = Input.GetKey(Key.Q);
        e = Input.GetKey(Key.E);

        if (w) camtrans->Position +=  camtrans->Forward * movespeed * felta;
        if (s) camtrans->Position += -camtrans->Forward * movespeed * felta;
        if (a) camtrans->Position +=  camtrans->Right   * movespeed * felta;
        if (d) camtrans->Position += -camtrans->Right   * movespeed * felta;
        if (e) camtrans->Position +=  camtrans->Up      * movespeed * felta;
        if (q) camtrans->Position += -camtrans->Up      * movespeed * felta;

        program.Uniform("transform",transform.World);
        program.Uniform("proj",camera.proj);
        program.Uniform("view",camera.view);

        Vao.Draw();

        Transform trans2 = new Transform
        {
            Rotation = new Vector3(0f, CMath.rad(90), 0f),
            Position = new Vector3(0f, -.25f, 0f)
        };

        program.Uniform("transform",trans2.World);
        Vao2.Draw();
    }
    
    private static bool wire = false;
    private static void ToggleWireframe()
    {
        wire = !wire;
        Video.SetWireframe(wire);
    }

    private static void CameraMovement(Vector2 mpos)
    {
        camera.transform.Rotation += new Vector3(
           -CMath.rad(Input.MouseDelta.X/3) * sensitivity,
            CMath.rad(Input.MouseDelta.Y/3) * sensitivity,
            0f
        );
    }
}