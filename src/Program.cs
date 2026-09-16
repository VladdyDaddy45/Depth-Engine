using Depth.Graphics;
using Depth.Graphics.Shaders;
using Depth.Parsing;
using static Depth.Game.Input;
using Utils.CMath;
using System.Numerics;

using f32 = float;

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
    public static Mesh teapot, plane;
    public static Transform* camtrans;
    public static bool leftdown = false;
    public static bool rightdown = false;
    public static float movespeed = 5f;
    public static float sensitivity = 0.4f;

    public static void Load()
    {
        Vao = ModelParser.ParseObj("teapot.obj");
        Vao2 = ModelParser.ParseObj("plane.obj");


        Random random = new Random();
        teapot = new Mesh(Vao);
        for (int i = 0; i < 10000; i++)
        {
            Transform trans = new Transform
            {
                Position = new Vector3(
                    (random.NextSingle() - .5f) * 100f,
                    (random.NextSingle() - .5f) * 100f,
                    (random.NextSingle() - .5f) * 100f
                )
            };

            teapot.NewInstance(trans);
        }
        plane = new Mesh(Vao2);
        plane.NewInstance(new Transform());
        plane.ProcessBuffer();
        teapot.ProcessBuffer();

        Shader vert = new Shader("vertex/projection.vs");
        Shader frag = new Shader("fragment/simple.frag");
        program = new ShaderProgram([vert, frag]);

        AddDownCallback(Key.K, ToggleWireframe);
        AddMouseMoveCallback(CameraMovement);

        camera.transform.Rotation = new Vector3(0, CMath.rad(-90), 0);

        Depth.Script.ExecuteScripts();
    }

//  -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- 
    public static void Render(double delta)
    {
        f32 felta = (f32)delta;

        program.Use();
        double T = Application.MainApp.window.Time;


        transform.Rotation += new Vector3(1f*felta,1f*felta,1f*felta);
        transform.Position = new Vector3(1f*MathF.Sin((float)T),0f,0f);


        movespeed = GetKey(Key.ShiftLeft)? 30f : 5f;
        if (GetKey(Key.W)) camera.transform.Position +=  camera.transform.Forward * movespeed * felta;
        if (GetKey(Key.S)) camera.transform.Position += -camera.transform.Forward * movespeed * felta;
        if (GetKey(Key.A)) camera.transform.Position +=  camera.transform.Right   * movespeed * felta;
        if (GetKey(Key.D)) camera.transform.Position += -camera.transform.Right   * movespeed * felta;
        if (GetKey(Key.E)) camera.transform.Position +=  camera.transform.Up      * movespeed * felta;
        if (GetKey(Key.Q)) camera.transform.Position += -camera.transform.Up      * movespeed * felta;

        //program.Uniform("transform",transform.World);
        program.Uniform("proj",camera.proj);
        program.Uniform("view",camera.view);
        
        Vector3 col0 = new Vector3(transform.World.M11, transform.World.M21, transform.World.M31);
        Vector3 col1 = new Vector3(transform.World.M12, transform.World.M22, transform.World.M32);
        Vector3 col2 = new Vector3(transform.World.M13, transform.World.M23, transform.World.M33);

        program.Uniform("col0", col0);
        program.Uniform("col1", col1);
        program.Uniform("col2", col2);

        //WriteLine(transform.World.ToString());

        plane.Draw();
        teapot.Draw();
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
            -CMath.rad(MouseDelta.X/3) * sensitivity,
             CMath.rad(MouseDelta.Y/3) * sensitivity,
             0f
        );

        float Y = camera.transform.Rotation.Y;
        float X = camera.transform.Rotation.X;
        float Z = camera.transform.Rotation.Z;
        if (Y > CMath.rad(90) || Y < CMath.rad(-90))
            camera.transform.Rotation = new Vector3(
                X,
                (float)Math.Clamp(Y, CMath.rad(-90),CMath.rad(90)),
                Z
            );
    }
}