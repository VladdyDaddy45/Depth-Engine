global using BufferUsageARB = Silk.NET.OpenGL.BufferUsageARB;
global using BufferTargetARB = Silk.NET.OpenGL.BufferTargetARB;
global using VertAttribType = Silk.NET.OpenGL.VertexAttribPointerType;

namespace Engine.Graphics;
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


public struct VertAttrib
{
    public uint Position;
    public uint Size;
    public VertAttribType Type;
};

public class VertexArray
{
    private static GL gl = Video.gl;

    private uint _vao;
    private float[] data;
    private uint[] indices;
    private uint stride;
    private VertAttrib[] attributes = new VertAttrib[1];
    private int attribNumber = 0;

    public BufferObject<float> VBO;
    public BufferObject<uint>? EBO;

    public VertexArray(float[] Data, uint Stride)
    {
        _vao = gl.GenVertexArray();
        Bind();

        data = Data;
        stride = Stride;
        
        VBO = new BufferObject<float>(data, BufferTargetARB.ArrayBuffer);
    }

    public unsafe void Draw()
    {
        Bind();
        gl.DrawElements(PrimitiveType.Triangles, (uint)indices.Length, DrawElementsType.UnsignedInt, (void*) 0);
    }

    public unsafe void SetAttribute(uint Position, uint Size, VertAttribType Type)
    {
        Bind();
        VBO.Bind();
        EBO?.Bind();
        
        gl.EnableVertexAttribArray(Position);
        gl.VertexAttribPointer(
            Position, 
            (int)Size, 
            Type,
            false,
            stride * sizeof(float), 
            (void*) (0 + (Position * Size * sizeof(float)))
        );

        VertAttrib attrib;
        attrib.Position = Position;
        attrib.Size = Size;
        attrib.Type = Type;

        AddAttribute(attrib);

        Unbind();
        VBO.Unbind();
        EBO?.Unbind();
    }

    public void SetIndices(uint[] Indices)
    {
        indices = Indices;
        EBO = new BufferObject<uint>(indices,BufferTargetARB.ElementArrayBuffer);
    }

    public void Bind()
    {
        gl.BindVertexArray(_vao);
    }

    public void Unbind()
    {
        gl.BindVertexArray(0);
    }

    private void AddAttribute(VertAttrib attrib)
    {

        attribNumber++;
        VertAttrib[] holder = new VertAttrib[attribNumber];

        for (int i = 0; i < attributes.Length; i++) 
            holder[i] = attributes[i];

        holder[attribNumber-1] = attrib;
        attributes = holder;
    }
}