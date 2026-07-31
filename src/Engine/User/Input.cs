global using Key = Silk.NET.Input.Key;

namespace Engine.User;

using System.Diagnostics.CodeAnalysis;
using Engine.Graphics;
using Silk.NET.Input;

public class Input
{
    private static IInputContext context = Application.MainApp.input;
    private static List<Action>[] KeyDownEvents = {};
    private static List<Action>[] KeyUpEvents = {};
    private static bool[] Buttons = {};
    

    public static void Init()
    {
        for (int i = 0; i < context.Keyboards.Count; i++)
        {
            context.Keyboards[i].KeyDown += KeyDown;
            context.Keyboards[i].KeyUp += KeyUp;
        }

        Key[] values = Enum.GetValues<Key>();
        int Length = 0;

        foreach (Key key in values)
        {
            if ((int)key > Length)
                Length = (int)key;
        }

        Length++;
        Buttons = new bool[Length];

        foreach(Key key in values)
        {
            if ((int)key == -1)
                continue;

            Buttons[(int)key] = false;
        }
        

        KeyDownEvents = new List<Action>[Length];
        KeyUpEvents = new List<Action>[Length];
    }

    public static bool GetKey(Key key)
    {
        return Buttons[(int)key];
    }

    public static void AddDownCallback(Key key, Action cb)
    { KeyDownEvents[(int)key].Add(cb); }

    public static void AddUpCallback(Key key, Action cb)
    { KeyUpEvents[(int)key].Add(cb); }

    public static void RemoveDownCallback(Key key, Action cb)
    { KeyDownEvents[(int)key].Remove(cb); }

    public static void RemoveUpCallback(Key key, Action cb)
    { KeyUpEvents[(int)key].Remove(cb); }
    

    private static void KeyDown(IKeyboard keyboard, Key key, int code)
    {
        Buttons[(int)key] = true;
        //foreach(Action cb in KeyDownEvents[(int)key])
        //    cb();
    }
    
    private static void KeyUp(IKeyboard keyboard, Key key, int code)
    {
        Buttons[(int)key] = false;
        //foreach(Action cb in KeyUpEvents[(int)key])
        //    cb();
    }
}