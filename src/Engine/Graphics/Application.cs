using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

using Engine.User;
using Engine.Graphics.Shaders;

namespace Engine.Graphics;

public class Application
{   
    [NotNull]
    public static Application? MainApp;
    
    public int width = 800;
    public int height = 600;

    [NotNull]
    public IWindow window;
    [NotNull]
    public WindowOptions options;
    [NotNull]
    public IInputContext input;

    public static void Init(int Width, int Height)
    {
        MainApp = new Application(Width, Height);
        
    }

    public static void Start()
    {
        MainApp.window.Run();
        MainApp.window.Dispose();
    }

    private Application(int Width, int Height)
    {
        width = Width;
        height = Height;

        options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(width, height),
            Title = "Engine Testing"
        };

        window = Window.Create(options);

        window.Load += Load;
        window.Update += Update;
        window.Render += Render;
        window.FramebufferResize += FramebufferResize;
        window.Closing += Close;
    }

    private void Load()
    {
        input = window.CreateInput();

        Video.Init(MainApp);
        Input.Init();

        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;
    }

    private void Update(double deltaTime)
    {}

    private void Render(double deltaTime)
    {}

    private void Close()
    {
        ShaderProgram.Cleanup();
    }

    private void FramebufferResize(Vector2D<int> newSize)
    {
        width = newSize.X;
        height = newSize.Y;
        Video.gl.Viewport(newSize);
    }

    private void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            window.Close();
    }

    public IInputContext getInputContext()
    { return input; }


    public void AddLoad(Action method)
    { window.Load += method; }

    public void AddUpdate(Action<double> method)
    { window.Update += method; }

    public void AddRender(Action<double> method)
    { window.Render += method; }

    public void AddClosing(Action method)
    { window.Closing += method; }

    ~Application()
    {
        Close();
    }
}