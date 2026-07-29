using System.Diagnostics.CodeAnalysis;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;

using Engine.Graphics;
using Engine.User;

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
    public IInputContext input;

    public static void Init(int Width, int Height)
    {
        MainApp = new Application(Width, Height);

    }

    public static void Start()
    {
        MainApp.window.Run();
    }

    private Application(int Width, int Height)
    {
        width = Width;
        height = Height;

        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(width, height),
            Title = "Engine Testing"
        };

        window = Window.Create(options);

        window.Load += Load;
        window.Update += Update;
        window.Render += Render;
        window.Closing += Close;
    }

    private void Load()
    {
        input = window.CreateInput();

        Video.Init(MainApp);

        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;
    }

    private void Update(double deltaTime)
    {}

    private void Render(double deltaTime)
    {}

    private void Close()
    {}

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
}