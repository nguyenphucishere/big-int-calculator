using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bigIntCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInt bigIntA = new BigInt(int.MaxValue);
            BigInt bigIntB = new BigInt(int.MaxValue);
            Console.WriteLine(bigIntA * bigIntB);
            Console.ReadLine();
        }
    }
}
