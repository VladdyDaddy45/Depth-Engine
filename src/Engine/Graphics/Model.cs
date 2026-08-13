using Engine.Graphics;
using Utils;
using static System.Console;

namespace Engine.Graphics;

public unsafe class Model
{
    public static VertexArray LoadObj(string path)
    {
        string text = Reader.ReadAll("assets/models/" + path);
        List<float> verts = new List<float>();
        List<uint> inds = new List<uint>();
        
        string[] elems = text.Split();

        for (int i = 0; i < elems.Length; i++)
        {
            switch (elems[i])
            {
                case "v":
                    verts.AddRange([
                        float.Parse(elems[i+1]),
                        float.Parse(elems[i+2]),
                        float.Parse(elems[i+3]),
                    ]);
                    verts.AddRange([
                        (float)new Random().NextDouble(),
                        (float)new Random().NextDouble(),
                        (float)new Random().NextDouble()
                    ]);
                    break;
                
                case "f":
                    uint[] uints = new uint[3];
                    for (int j = 1; j <= 3; j++)
                    {
                        WriteLine(j);
                        uints[j-1] = uint.Parse(
                            elems[i+j].Split(' ','/')[0]
                        );
                    }
                    
                    inds.AddRange(uints);
                    break;
            }
        }

        VertexArray vao = new VertexArray(verts.ToArray(), 6);
        vao.SetIndices(inds.ToArray());
        vao.SetAttribute(0, 3, VertAttribType.Float);
        vao.SetAttribute(1, 3, VertAttribType.Float);

        return vao;
    }
} 