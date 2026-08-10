using Engine.Graphics;
using Utils;

namespace Engine.Graphics;

public unsafe class Model
{
    public static void LoadObj(string path)
    {
        string text = Reader.ReadAll("assets/models/" + path);
        List<float> verts = new List<float>();
        List<uint> inds = new List<uint>();
        
        for (int i = 0; i < text.Length; i++)
        {
            switch (text[i])
            {
                case  'v':
                    if (text[i+1] != ' ') break;
                    string[] vertstrs = Extract3(text, i+2, " \n");
                    verts.Add( float.Parse(vertstrs[0]) );
                    verts.Add( float.Parse(vertstrs[1]) );
                    verts.Add( float.Parse(vertstrs[2]) );
                    break;

                case  'f':
                    if (text[i+1] != ' ') break;
                    string[] indstrs = Extract3(text, i+2, " \n");
                    inds.Add( uint.Parse(indstrs[0]) );
                    inds.Add( uint.Parse(indstrs[1]) );
                    inds.Add( uint.Parse(indstrs[2]) );
                    break;

                case '\n':
                    break;
            }
        }


        VertexArray vao;
        BufferObject<uint> buf;

        //return new Mesh3D(vao);
    }

    private static string[] Extract3(string text, int start, string endchars)
    {
        string[] result = ["", "", ""];

        int end = 0;

        for (int i = 0; i < 3; i++)
        {
            char chr = text[start+end];
            
            int num = 0; // variable to prevent infinite loop
            while ( num < 50 )
            {
                chr = text[start+end];
                if (endchars.Contains(chr)) break;

                result[i] += chr;
                num++;
                end++;
            }

            start += end+1;
            end = 0;
        }

        return result;
    }
} 