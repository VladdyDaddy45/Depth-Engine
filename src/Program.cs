using Engine.Graphics;
using Engine.Graphics.Shaders;
using Engine.Parsers;
using static Engine.User.Input;
using Utils.CMath;
using System.Numerics;
using static System.Console;

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
    public static Transform* camtrans;
    public static bool leftdown = false;
    public static bool rightdown = false;
    private static float movespeed = 5f;
    private static float sensitivity = 0.4f;

    public static void Load()
    {   
        Vao = ModelParser.ParseObj("teapot.obj");
        Vao2 = ModelParser.ParseObj("plane.obj");

        Shader vert = new Shader("vertex/projection.vert");
        Shader frag = new Shader("fragment/simple.frag");
        program = new ShaderProgram([vert, frag]);

        AddDownCallback(Key.K, ToggleWireframe);
        AddMouseMoveCallback(CameraMovement);

        camera.transform.Rotation = new Vector3(0, CMath.rad(-90), 0);
    }

    // -- RENDERING!!!

    public static void Render(double delta)
    {
        f32 felta = (f32)delta;

        program.Use();
        double T = Application.MainApp.window.Time;
        transform.Scale = new Vector3(0.25f,0.25f,0.25f);
        transform.Rotation += new Vector3(0.03f, 0.03f, 0f);
        transform.Position = new Vector3(0f,(float)Math.Sin(T),0f);

        if (GetKey(Key.W)) camera.transform.Position +=  camera.transform.Forward * movespeed * felta;
        if (GetKey(Key.S)) camera.transform.Position += -camera.transform.Forward * movespeed * felta;
        if (GetKey(Key.A)) camera.transform.Position +=  camera.transform.Right   * movespeed * felta;
        if (GetKey(Key.D)) camera.transform.Position += -camera.transform.Right   * movespeed * felta;
        if (GetKey(Key.E)) camera.transform.Position +=  camera.transform.Up      * movespeed * felta;
        if (GetKey(Key.Q)) camera.transform.Position += -camera.transform.Up      * movespeed * felta;

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