using Microsoft.EntityFrameworkCore;

namespace Weather.Context;

public class NormalDistribution
{
    public NormalDistribution(double Mean, double StandardDeviation)
    {
        this.Mean = Mean;
        this.StandardDeviation = StandardDeviation;
    }

    public double Mean { get; set; }
    public double StandardDeviation { get; set; }

    public double Generate(Random rand)
    {
        double u1, u2;
        u1 = 1 - rand.NextDouble();
        u2 = 1 - rand.NextDouble();
        double randomNormal = Math.Sqrt(-2.0 * Math.Log(u1))
            * Math.Sin(2.0 * Math.PI * u2);
        return Mean + StandardDeviation * randomNormal; 
    }

    public double Generate(Random rand, double pushFromCenter)
    {
        pushFromCenter /= 100;
        var normal = Generate(rand);
        if (normal > Mean)
            normal += pushFromCenter * StandardDeviation;
        else
            normal -= pushFromCenter * StandardDeviation;
        return normal;
    }
}
