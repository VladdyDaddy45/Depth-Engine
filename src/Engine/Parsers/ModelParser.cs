using Assimp;
using Utils;
using Engine.Graphics;

namespace Engine.Parsers;

public static class ModelParser
{
    public static VertexArray ParseObj(string path)
    {
        string text = Reader.ReadAll("assets/models/" + path);
        List<float> verts = new List<float>();
        List<uint> inds = new List<uint>();
        
        string[] lines = text.Split('\n');

        foreach (string line in lines)
        {
            string[] parts = line.Split();

            switch (parts[0])
            {
                case null: 
                    continue;

                case "":
                    continue;
                
                case "#":
                    continue;

                // Vertex Reading
                case "v":
                    verts.AddRange([
                        float.Parse(parts[1]),
                        float.Parse(parts[2]),
                        float.Parse(parts[3]),
                    ]);
                    verts.AddRange([
                        (float)new Random().NextDouble(),
                        (float)new Random().NextDouble(),
                        (float)new Random().NextDouble()
                    ]);
                    break;
                
                // Index reading
                case "f":
                    uint[] uints = new uint[3];

                    for (int i = 0; i < 3; i++)
                    {
                        uints[i] = uint.Parse(parts[i+1].Split('/',StringSplitOptions.RemoveEmptyEntries)[0])-1;
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