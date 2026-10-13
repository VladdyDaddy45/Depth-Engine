global using VertAttribType = Silk.NET.OpenGL.VertexAttribPointerType;

using Silk.NET.OpenGL;

namespace Depth.Graphics;


public struct VertexAttribute
{
    public uint index;
    public uint count;
    public VertAttribType type;
};

public class VertexArray<VertType, IndexType>
    where VertType : unmanaged
    where IndexType: unmanaged
{
    private static GL gl = Video.gl;

    public uint handle;
    public List<VertexAttribute> attributes = new List<VertexAttribute>();

    public BufferObject<VertType> VBO;
    public BufferObject<IndexType> EBO;

    public VertexArray(BufferObject<VertType> vbo, BufferObject<IndexType> ebo)
    {
        handle = gl.GenVertexArray();
        VBO = vbo;
        EBO = ebo;
        BufferData();
    }

    public void AddAttribute(VertexAttribute attrib)
    { attributes.Add(attrib); } 

    public void AddAttribute(uint Index, uint Count, VertAttribType Type)
    { attributes.Add( new VertexAttribute {index = Index, count = Count, type = Type} ); } 

    public void CompileAttributes()
    {
        foreach (var attrib in attributes)
            AttribPointer(attrib.index, attrib.count, attrib.type);
    }

    public unsafe void AttribPointer(uint Index, uint Count, VertAttribType Type)
    {
        Bind();

        gl.EnableVertexAttribArray(Index);

        gl.VertexAttribPointer(
            Index, (int)Count, Type, false,
            CalcStride() * (uint)sizeof(VertType),
            (void*)(CalcOffset(Index) * sizeof(VertType))
        );

        Unbind();
    }

    public uint CalcStride()
    {
        uint result = 0;

        foreach (var attrib in attributes) 
            result += attrib.count;

        //Console.WriteLine(result);
        return result;
    }

    public uint CalcOffset(uint Index)
    {
        uint result = 0;
        if (Index == 0) return 0;

        for (int i = 0; i < Index; i++)
            result += attributes[i].count;
        
        return result;
    }

    public void BufferData()
    {
        Bind();
        VBO.Bind();
        EBO.Bind();
        Unbind();
    }

    public void Bind()
    { gl.BindVertexArray(handle); }

    public void Unbind()
    { gl.BindVertexArray(0); }

    public void Dispose()
    { gl.DeleteVertexArray(handle); }
}