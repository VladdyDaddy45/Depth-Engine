global using VertAttribType = Silk.NET.OpenGL.VertexAttribPointerType;

using Silk.NET.OpenGL;

namespace Depth.Graphics;

public struct VertAttrib
{
    public uint Position;
    public uint Size;
    public VertAttribType Type;
};

public class VertexArray
{
    private static GL gl = Video.gl;

    public uint handle;
    public uint stride;
    public VertAttrib[] attributes = new VertAttrib[1];
    public int attribNumber = 0;

    public BufferObject<float> VBO;
    public BufferObject<uint> EBO;

    public VertexArray(float[] data, uint[] inds, uint Stride)
    {
        handle = gl.GenVertexArray();
        stride = Stride;
        Bind();
        VBO = new BufferObject<float>(data, BufferTargetARB.ArrayBuffer);
        EBO = new BufferObject<uint>(inds,BufferTargetARB.ElementArrayBuffer);
        Unbind();
    }

    public unsafe void Draw()
    {
        Bind();
        gl.DrawElements(PrimitiveType.Triangles, (uint)EBO.data.Length, DrawElementsType.UnsignedInt, (void*) 0);
        Unbind();
    }

    public unsafe void AttribPointer(uint Position, uint Size, VertAttribType Type)
    {
        Bind();
        VBO.Bind();

        gl.EnableVertexAttribArray(Position);
        gl.VertexAttribPointer(
            Position, 
            (int)Size, 
            Type,
            false,
            stride * sizeof(float), 
            (void*) (0 + (Position * Size * sizeof(float)))
        );

        AddAttribute(
        new VertAttrib {
            Position = Position,
            Size = Size,
            Type = Type
        });

        Unbind();
        VBO.Unbind();
    }

    public void Bind()
    { gl.BindVertexArray(handle); }

    public void Unbind()
    { gl.BindVertexArray(0); }

    private void AddAttribute(VertAttrib attrib)
    {
        attribNumber++;
        VertAttrib[] temp = new VertAttrib[attribNumber];

        for (int i = 0; i < attributes.Length; i++) 
            temp[i] = attributes[i];

        temp[attribNumber-1] = attrib;
        attributes = temp;
    }
}