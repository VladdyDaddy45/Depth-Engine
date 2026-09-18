using static Depth.Game.Input;
using System.Numerics;
using Depth;
using Depth.Graphics;
using Utils.CMath;

class Player : Script
{
    Camera camera = new Camera(new Transform(),120f);
    float sensitivity = 0.4f;
    float movespeed = 5f;

    void Init(string[] args)
    {
        Console.Write(args.Length>0?args[0]+'\n':"");
        AddMouseMoveCallback(CameraMovement);

        camera.transform.Rotation = new Vector3(0, CMath.rad(-90), 0);
    }

    void Render(double delta)
    {
        float felta = (float)delta;
        movespeed = GetKey(Key.ShiftLeft)? 30f : 5f;

        if (GetKey(Key.W)) camera.transform.Position +=  camera.transform.Forward * movespeed * felta;
        if (GetKey(Key.S)) camera.transform.Position += -camera.transform.Forward * movespeed * felta;
        if (GetKey(Key.A)) camera.transform.Position +=  camera.transform.Right   * movespeed * felta;
        if (GetKey(Key.D)) camera.transform.Position += -camera.transform.Right   * movespeed * felta;
        if (GetKey(Key.E)) camera.transform.Position +=  camera.transform.Up      * movespeed * felta;
        if (GetKey(Key.Q)) camera.transform.Position += -camera.transform.Up      * movespeed * felta;
    }

    void CameraMovement(Vector2 mpos)
    {
        camera.transform.Rotation += new Vector3(
            -CMath.rad(MouseDelta.X/3) * sensitivity,
             CMath.rad(MouseDelta.Y/3) * sensitivity,
             0f
        );

        float Y = camera.transform.Rotation.Y;
        float X = camera.transform.Rotation.X;
        float Z = camera.transform.Rotation.Z;
        if (Y > CMath.rad(90) || Y < CMath.rad(-90))
            camera.transform.Rotation = new Vector3(
                X,
                (float)Math.Clamp(Y, CMath.rad(-90),CMath.rad(90)),
                Z
            );
    }
}