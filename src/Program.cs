using Depth.Graphics;
using Depth.Graphics.Shaders;
using Depth.Parsing;
using static Depth.Game.Input;
using Utils.CMath;
using System.Numerics;
using GL = Silk.NET.OpenGL.GL;
using GLEnum = Silk.NET.OpenGL.GLEnum;
using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;
using Depth;

class Entry
{
    static GL gl = Video.gl;
    static string[] Args = [];
    static ShaderProgram program;
    static Transform transform = new Transform();
    static Mesh thing;
    static Scene scene = new Scene();
    static Texture texture;

    //static readonly bool FORCE_VR = true;

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
        thing = Parsing.Importer.LoadMesh("assets/models/sphere.obj");
        scene.AddMesh(thing);

        Shader vert = new Shader("vertex/projection.vs");
        Shader frag = new Shader("fragment/simple.frag");
        program = new ShaderProgram([vert, frag]);
        texture = new Texture("assets/textures/mustard.png", "uTexture",TexUnit.Texture0);

        AddDownCallback(Key.K, ToggleWireframe);

        gl = Video.gl;

        Script.ExecuteScripts(Args);
    }

//  -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- RENDERING -- 
    static void Render(double delta)
    {
        float felta = (float)delta;

        program.Use();
        double T = Application.MainApp.window.Time;
        float tsin = MathF.Sin((float)T);

        transform.Position = new Vector3(tsin,0f,0f);

        texture.PassToShader(program);
        program.Uniform("proj",Camera.CurrentCamera.proj);
        program.Uniform("view",Camera.CurrentCamera.view);

        //program.Uniform("LightDirection", new Vector3(tsin, 1, 0));
        //program.Uniform("AmbientLight", .01f);

        scene.Draw();
    }
    
    static bool wire = false;
    static void ToggleWireframe()
    {
        wire = !wire;
        Video.SetWireframe(wire);
    }
}