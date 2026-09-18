using Depth.Graphics;
using Depth.Graphics.Shaders;
using Depth.Parsing;
using static Depth.Game.Input;
using Utils.CMath;
using System.Numerics;

unsafe class Entry
{
    static string[] Args = [];
    public static void Main(string[] args)
    {
        Application.Init(1200, 800);
        Application.MainApp.AddLoad(Load);
        Video.RenderCallbacks.Add(Render);

        Args = args;
        Application.Start();
    }

    public static ShaderProgram program, prog2;
    public static VertexArray Vao, Vao2;
    public static Transform transform = new Transform();
    public static Mesh teapot, plane;

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

        Depth.Script.ExecuteScripts(Args);
    }

//  -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- 
    public static void Render(double delta)
    {
        float felta = (float)delta;

        program.Use();
        double T = Application.MainApp.window.Time;


        transform.Rotation += new Vector3(1f*felta,1f*felta,1f*felta);
        transform.Position = new Vector3(1f*MathF.Sin((float)T),0f,0f);

        program.Uniform("proj",Camera.CurrentCamera.proj);
        program.Uniform("view",Camera.CurrentCamera.view);
        
        var mat3 = Transform.ToMatrix3x3(transform);

        program.Uniform("col0", mat3[0]);
        program.Uniform("col1", mat3[1]);
        program.Uniform("col2", mat3[2]);

        plane.Draw();
        teapot.Draw();
    }
    
    private static bool wire = false;
    private static void ToggleWireframe()
    {
        wire = !wire;
        Video.SetWireframe(wire);
    }
}