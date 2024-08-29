using System;
using System.Linq;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using DiceRolling.Expressions;
using DiceRolling.Expressions.Visitors;

namespace DiceRolling.Benchmarking
{
    public class Benchmarks
    {
        [Params(
            "1d10",
            "1d[-1]",
            "((87%640160)+5/+240+142*9.0%1%+055952.76-(20)+2%(621/2.887+5.7342641)*(-36.85+4643*57/90.05))+3-3%3945/+0-(-7)+45%35/+062/65977492/62157*12*(-03.492)--9194/+273+-299/4.251*27+58-5-+5++8+0.6%(+68.62207-5458.5*+2023.6*65+5657+-55%937.2+-232/754/35*-106-80.1*-669+-8)%ed2*(-57)++3--54/(+21.6)-(43+2.815)/(5)%-26*(282.8)/-6+-9*36-+1/(6.18)*209"
        )]
        public string Roll;
        private Expression expr = default!;
        
        [GlobalSetup]
        public void Setup()
        {
            expr = new DiceRoller().Parse(Roll);
        }

        [Benchmark]
        public bool ErrorFinder()
        {
            var checker = new ErrorFinder();
            return checker.Visit(expr).Any();
        }
    }
}
