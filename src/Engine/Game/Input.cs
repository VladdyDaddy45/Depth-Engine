namespace Engine.Game;

using System.Diagnostics.CodeAnalysis;
using Engine.Graphics;
using Silk.NET.Input;

public class Input
{

    private static IInputContext context = Application.MainApp.input;
    private List<Input> objects = new List<Input>();

    public Input(Key key)
    {
        
    }

    public static void Init()
    {
        for (int i = 0; i < context.Keyboards.Count; i++)
        {
            context.Keyboards[i].KeyDown += KeyDown;
            context.Keyboards[i].KeyUp += KeyUp;
        }
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int code)
    {
        Console.WriteLine(code);
    }
    
    private static void KeyUp(IKeyboard keyboard, Key key, int Code)
    {
        
    }
}