using Silk.NET.Assimp;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AssimpMesh = Silk.NET.Assimp.Mesh;

namespace Depth.Parsing;

struct Vertex
{
    Vector3 Position;
    Vector2 TexCoords;
    Vector3 Normal;
}

struct Vertices
{
    Vector3[] Position;
    Vector2[] TexCoords;
    Vector3[] Normal;
}

public class Obj
{
    static Assimp assimp;
    public unsafe static void Load(string path)
    {
        var scene = assimp.ImportFile(path, (uint)PostProcessSteps.Triangulate);
    }
}