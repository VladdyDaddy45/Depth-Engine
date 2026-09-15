global using BufferUsageARB = Silk.NET.OpenGL.BufferUsageARB;
global using BufferTargetARB = Silk.NET.OpenGL.BufferTargetARB;

namespace Depth.Graphics;
using Silk.NET.OpenGL;


public class BufferObject<T>
{
    private static GL gl = Video.gl;

    private uint buffer;
    private T[] data;
    private BufferTargetARB target;

    public unsafe BufferObject(T[] Data, BufferTargetARB Target)
    {
        buffer = gl.GenBuffer();
        data = Data;
        target = Target;

        gl.BindBuffer(target, buffer);

        fixed (T* buf = data)
        {
            gl.BufferData(
                target, 
                (nuint) (data.Length * sizeof(T)),
                buf,
                BufferUsageARB.DynamicDraw
            );
        }
    }

    public void Bind()
    {
        gl.BindBuffer(target, buffer);
    }

    public void Unbind()
    {
        gl.BindBuffer(target,0);
    }
}