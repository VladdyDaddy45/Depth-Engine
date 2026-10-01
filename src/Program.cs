using Depth.Graphics;
using Depth.Graphics.Shaders;
using Depth.Parsing;
using static Depth.Game.Input;
using Utils.CMath;
using System.Numerics;

unsafe class Entry
{
    static string[] Args = [];
    static ShaderProgram program;
    static Transform transform = new Transform();
    static Mesh thing;

    static readonly bool FORCE_VR = true;

    public static void Main(string[] args)
    {
        Application.Init(1200, 800);
        Application.MainApp.AddLoad(Load);
        Video.RenderCallbacks.Add(Render);

        Args = args;
        Application.Start();
    }

    static void Load()
    {
        Obj.Load("assets/models/cube.obj");
        
        Random random = new Random();
        thing = Obj.meshes[0];

        for (int i = 0; i < 10000; i++)
        {
            Transform trans = new Transform
            {
                Position = new Vector3(
                    (random.NextSingle() - .5f) * 100,
                    (random.NextSingle() - .5f) * 100,
                    (random.NextSingle() - .5f) * 100
                ),/*
                Scale = new Vector3(
                    random.NextSingle(),
                    random.NextSingle(),
                    random.NextSingle()
                )*/
            };

            thing.NewInstance(trans);
        }

        thing.ProcessBuffer();

        Shader vert = new Shader("vertex/projection.vs");
        Shader frag = new Shader("fragment/simple.frag");
        program = new ShaderProgram([vert, frag]);

        AddDownCallback(Key.K, ToggleWireframe);

        Depth.Script.ExecuteScripts(Args);
    }

//  -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- 
    static void Render(double delta)
    {
        float felta = (float)delta;

        program.Use();
        double T = Application.MainApp.window.Time;
        float tsin = MathF.Sin((float)T);

        //transform.Rotation += new Vector3(felta, felta, felta);
        transform.Position = new Vector3(tsin,0f,0f);
        
        var mat3 = Transform.ToMatrix3x3(transform);

        program.Uniform("proj",Camera.CurrentCamera.proj);
        program.Uniform("view",Camera.CurrentCamera.view);

        program.Uniform("LightDirection", new Vector3(tsin, 1, 0));
        program.Uniform("AmbientLight", .01f);

        program.Uniform("col0", mat3[0]);
        program.Uniform("col1", mat3[1]);
        program.Uniform("col2", mat3[2]);

        thing.Draw();
    }
    
    static bool wire = false;
    static void ToggleWireframe()
    {
        wire = !wire;
        Video.SetWireframe(wire);
    }
}