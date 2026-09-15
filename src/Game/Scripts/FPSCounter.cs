using Depth;
using System.Timers;

class FramerateCounter : Script
{
    uint frames = 0;

    void Init()
    {
        Depth.Graphics.Video.RenderCallbacks.Add(Count);

        System.Timers.Timer fpstimer = new System.Timers.Timer();
        fpstimer.Elapsed += new ElapsedEventHandler(CalcFPS);
        fpstimer.Interval = 500;
        fpstimer.Enabled = true;
    }

    void Count(double delta)
    {
        frames++;
    }

    void CalcFPS(object? source, ElapsedEventArgs? e)
    {
        Console.WriteLine("FPS: " + frames*2);
        frames = 0;
    }
}