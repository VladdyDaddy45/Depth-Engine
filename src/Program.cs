using Engine.Graphics;
using Utils.CMath;
using Engine.Graphics.Shaders;
using System.Numerics;
using Silk.NET.Input;

class Entry
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
    public static Camera camera = new Camera(new Transform(),90f);
    public static Vector2 rot = new Vector2(0f,0f);
    public static bool leftdown = false;
    public static bool rightdown = false;

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

        for (int i = 0; i < Application.MainApp.input.Keyboards.Count; i++)
        {
            Application.MainApp.input.Keyboards[i].KeyDown += keydown;
            Application.MainApp.input.Keyboards[i].KeyUp += keyup;
        }
    }

    public static void Render(double delta)
    {
        program.Use();
        double T = Application.MainApp.window.Time;
        float t = (float)T;
        transform.Scale = 0.5f;
        //transform.Rotation += new Vector3(0.03f, 0.03f, 0f);
        camera.transform.Position = new Vector3(0f,0f,-2f);
        //camera.transform.Rotation += new Vector3(0.05f,0f,0.0f);

        rot.X = 0;
        rot.Y = 0;

        rot.X += leftdown? 0.05f : 0;
        rot.X += rightdown? -0.05f : 0;

        camera.transform.Rotation += new Vector3(rot.X,rot.Y,0f);

        program.Uniform("transform",transform.world);
        program.Uniform("proj",camera.proj);
        program.Uniform("view",camera.view);

        Vao.Draw();

        Transform trans2 = new Transform
        {
            Rotation = new Vector3(0f, CMath.rad(90), 0f),
            Position = new Vector3(0f, -.25f, 0f)
        };

        program.Uniform("transform",trans2.world);
        Vao2.Draw();
    }

    public static void keydown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Left)
            leftdown = true;
        if (key == Key.Right)
            rightdown = true;
    }

    public static void keyup(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Left)
            leftdown = false;
        if (key == Key.Right)
            rightdown = false;
    }
}