namespace Engine.Graphics;

using System.Diagnostics.CodeAnalysis;
using System.Drawing;

using Silk.NET.OpenGL;
using Silk.NET.Vulkan;

public class Video
{
    [NotNull]
    public static GL gl;
    public static List<Action<double>> RenderCallbacks = new List<Action<double>>();
    
    public static void Init(Application app)
    {
        gl = app.window.CreateOpenGL();
        gl.ClearColor(Color.CornflowerBlue);
        gl.Enable(GLEnum.DepthTest);
        gl.DepthFunc(GLEnum.Less);

        app.AddRender(Render);
    }

    public static void Render(double delta)
    {
        gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        foreach(Action<double> callback in RenderCallbacks)
        { callback(delta); }
    }

    public static void SetWireframe(bool Bool)
    {
        gl.PolygonMode(GLEnum.FrontAndBack, Bool? GLEnum.Line : GLEnum.Fill);
    }
}