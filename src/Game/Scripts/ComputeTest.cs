using Depth;
using Depth.GPU;

class TestingCompute : Script
{
    void Init()
    {
        var comp = new Compute<int>("assets/shaders/compute/test.comp");
    }
}