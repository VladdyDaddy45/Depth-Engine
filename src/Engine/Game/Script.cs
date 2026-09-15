using System.Reflection;

namespace Depth; // in the topmost namespace so scripting is easier.

struct Callback
{
    public Script? script;
    public MethodInfo? methodInfo;
};

public abstract class Script
{
    private static List<Callback> renderCallbacks = new List<Callback>();

    // DO. NOT. TOUCH.
    public static void ExecuteScripts()
    {
        Type baseType = typeof(Script);
        Assembly assembly = baseType.Assembly;
        var scripts = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

        foreach(Type t in scripts)
        {
            Script? script = (Script?)Activator.CreateInstance(t);
            GetInstanceMethod(script, "Init")?
                .Invoke(script,null);
            
            renderCallbacks.Add(new Callback {
                methodInfo = GetInstanceMethod(script, "Render"),
                script = script
            });
        }

        Graphics.Video.RenderCallbacks.Add(OnRender);
    }

    static void OnRender(double delta)
    {
        foreach (var callback in renderCallbacks)
            callback.methodInfo?.Invoke(callback.script,[delta]);
        
    }

    static MethodInfo? GetInstanceMethod(Script? script, string name)
    {
        return script?
            .GetType().GetMethod(
                name,
                BindingFlags.Instance | 
                BindingFlags.Public | 
                BindingFlags.NonPublic
            );
    }
}