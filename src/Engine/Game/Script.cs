using System.Reflection;

namespace Depth; // in the topmost namespace so scripting is easier.

public abstract class Script
{
    // DO. NOT. TOUCH.
    public static void RunAll()
    {
        Type baseType = typeof(Script);
        var assembly = baseType.Assembly;
        var scripts = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

        foreach(Type t in scripts)
        {
            Script instance = (Script)Activator.CreateInstance(t);
            var init = GetInstanceMethod(instance, "Init");
            init?.Invoke(instance,null);
        }
    }
    
    private static MethodInfo? GetInstanceMethod(Script? instance, string name)
    {
        return instance?
        .GetType()
        .GetMethod(
            name,
            BindingFlags.Instance | 
            BindingFlags.Public | 
            BindingFlags.NonPublic
        );
    }
}