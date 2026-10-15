using System.Numerics;
using Depth;
using Depth.Graphics;

class SceneBuilder : Script
{
    Random random = new Random();
    List<Transform> transforms = new List<Transform>();

    void Init()
    {
        for (int i = 0; i < 10000; i++)
        {
            Transform trans = new Transform
            {
                Position = new Vector3(
                    (random.NextSingle() - .5f) * 100,
                    (random.NextSingle() - .5f) * 100,
                    (random.NextSingle() - .5f) * 100
                ),
                Rotation = new Vector3(
                    random.NextSingle(),
                    random.NextSingle(),
                    random.NextSingle()
                )
            };

            Scene.mainScene.AddInstance(trans, 0);
            transforms.Add(trans);
        }
    }
/*
    unsafe void Render(double delta)
    {
        foreach (var trans in transforms)
            trans.Position += new Vector3(
                (random.NextSingle()-.5f)/3f,
                (random.NextSingle()-.5f)/3f,
                (random.NextSingle()-.5f)/3f
            );
    }*/
}