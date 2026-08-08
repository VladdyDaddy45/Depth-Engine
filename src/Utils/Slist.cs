namespace Utils;

public interface ISlist
{}

// A Dynamically sized list, but it's a struct to keep everything on the stack.
public struct Slist<T> : ISlist
{
    public Type type = typeof(T);
    public uint Size = 0;
    public uint Capacity = 4;
    public T[] values;
    
    public Slist()
    { 
        values = new T[Capacity];
    }

    public Slist(T[] Values)
    {
        values = new T[Capacity];
        for (int i = 0; i < Values.Length; i++)
            Add(Values[i]);
    }

    public void Add(T value)
    {
        Size++;
        ResizeCapacity();

        values[Size] = value;
    }

    private void ResizeCapacity()
    {
        if (Size > Capacity)
            Capacity *= 2;
            Clone();

        if (Size < Capacity / 2 && Capacity > 4)
            Capacity /= 2;
            Clone();
    }

    private void Clone()
    {
        T[] holder = new T[Capacity];
        for (int i = 0; i < values.Length; i++)
            holder[i] = values[i];
        
        values = holder;
    }
}