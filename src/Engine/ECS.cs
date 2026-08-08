using Utils;

namespace Engine.ECS;

struct Position
{
    public uint Entity;
    public uint Component;
}

public unsafe class World
{
    private List<IComponent> components;

    public World()
    {
        components = new List<IComponent>();

    }

    public void AddComponent<T>()
    {
        components.Add(new Component<T>());
    }
}


public abstract class System
{
    public World world;

    public void Init()
    {}

    public void Tick()
    {}

    public void Render()
    {}
}

// Used for under the hood purposes. don't use this.
interface IComponent {}

public struct Component<T> : IComponent
{
    public Type type;
    public Slist<T> data;

    public Component()
    {
        type = typeof(T);
        data = new Slist<T>();
    }
}