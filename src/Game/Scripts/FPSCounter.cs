using Depth;
using System.Timers;
using System.Diagnostics;

class FPSCounter : Script
{
    uint frames = 0;
    static Stopwatch watch;

    void Init()
    {

        System.Timers.Timer fpstimer = new System.Timers.Timer();
        fpstimer.Elapsed += new ElapsedEventHandler(CalcFPS);
        fpstimer.Interval = 500;
        fpstimer.Enabled = true;
    }

    void Render(double delta)
    {
        frames++;
    }

    void CalcFPS(object? source, ElapsedEventArgs? e)
    {
        Console.WriteLine("FPS: " + frames*2);
        frames = 0;
    }
}