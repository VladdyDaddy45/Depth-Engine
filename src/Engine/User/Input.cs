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
        KeyDownEvents = new List<Action>[Length];
        KeyUpEvents = new List<Action>[Length];

        foreach(Key key in values)
        {
            int idx = (int)key;

            if (idx == -1)
                continue;

            Buttons[idx] = false;
            KeyDownEvents[idx] = new List<Action>();
            KeyUpEvents[idx] = new List<Action>();
        }
    }

    public static bool GetKey(Key key)
    { return Buttons[(int)key]; }

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
        int idx = (int)key;
        Buttons[idx] = true;
        for (int i = 0; i < KeyDownEvents[idx].Count; i++)
            KeyDownEvents[idx][i]();
    }
    
    private static void KeyUp(IKeyboard keyboard, Key key, int code)
    {
        int idx = (int)key;
        Buttons[idx] = false;
        for (int i = 0; i < KeyUpEvents[idx].Count; i++)
            KeyUpEvents[idx][i]();
    }
}