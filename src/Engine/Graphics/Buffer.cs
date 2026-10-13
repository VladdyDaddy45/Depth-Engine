global using BufferUsageARB = Silk.NET.OpenGL.BufferUsageARB;
global using BufferTargetARB = Silk.NET.OpenGL.BufferTargetARB;

namespace Depth.Graphics;
using Silk.NET.OpenGL;


public class BufferObject<T>
{
    private static GL gl = Video.gl;

    public BufferTargetARB bufferType;

    private uint handle;
    public T[] data;

    public unsafe BufferObject(T[] Data, BufferTargetARB Target)
    {
        handle = gl.GenBuffer();
        data = Data;
        bufferType = Target;

        gl.BindBuffer(bufferType, handle);

        fixed (T* buf = data)
            gl.BufferData(
                bufferType, 
                (nuint) (data.Length * sizeof(T)),
                buf,
                BufferUsageARB.DynamicDraw
            );
        
    }

    public void Bind()
    { gl.BindBuffer(bufferType, handle); }

    public void Unbind()
    { gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0); }

    public void Dispose()
    { gl.DeleteBuffer(handle); }

}