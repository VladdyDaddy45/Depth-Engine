using System.Collections;
using System.IO;

namespace Utils;

public class Reader
{
    public string space = "";

    public Reader(string Space)
    {
        space = Space;
    }

    public string ReadAllText(string path)
    {
        return ReadAll(space + "/" + path);
    }

    public static string ReadAll(string path)
    {
        return File.ReadAllText(path);
    }

    public static string GetExtension(string path)
    {
        return Path.GetExtension(path);
    }
}

