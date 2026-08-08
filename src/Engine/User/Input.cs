global using Key = Silk.NET.Input.Key;

namespace Engine.User;

using System.Numerics;
using Engine.Graphics;
using Silk.NET.Input;

public class Input
{
    private static IInputContext context = Application.MainApp.input;

    private static List<Action<Vector2>> MouseMoveEvents = new List<Action<Vector2>>();
    private static List<Action>[] KeyDownEvents = [];
    private static List<Action>[] KeyUpEvents = [];
    private static bool[] Buttons = [];
    

    private static Vector2 MousePosition = new Vector2(0f, 0f);
    public static Vector2 MouseDelta = new Vector2(0f, 0f);
    

    public static void Init()
    {
        for (int i = 0; i < context.Keyboards.Count; i++)
        {
            context.Keyboards[i].KeyDown += KeyDown;
            context.Keyboards[i].KeyUp += KeyUp;
        }

        for (int i = 0; i < context.Mice.Count; i++)
        {
            context.Mice[i].Cursor.CursorMode = CursorMode.Raw;
            context.Mice[i].MouseMove += MouseMove;
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

    // Addition methods
    public static void AddDownCallback(Key key, Action cb)
    { KeyDownEvents[(int)key].Add(cb); }

    public static void AddUpCallback(Key key, Action cb)
    { KeyUpEvents[(int)key].Add(cb); }

    public static void AddMouseMoveCallback(Action<Vector2> cb)
    { MouseMoveEvents.Add(cb); }

    // Removal methods
    public static void RemoveDownCallback(Key key, Action cb)
    { KeyDownEvents[(int)key].Remove(cb); }

    public static void RemoveUpCallback(Key key, Action cb)
    { KeyUpEvents[(int)key].Remove(cb); }

    public static void RemoveMouseMoveCallback(Action<Vector2> cb)
    { MouseMoveEvents.Remove(cb); }
    

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

    private static void MouseMove(IMouse mouse, Vector2 position)
    {
        MouseDelta = position - MousePosition;
        MousePosition = position;

        for (int i = 0; i < MouseMoveEvents.Count; i++)
            MouseMoveEvents[i](position);
    }
}