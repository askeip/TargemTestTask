using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargemTestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = new RPN_Calc();
            try
            {
                if (args.Length == 1)
                    Console.WriteLine(calc.Calculate(args[0]));
                else
                {
                    Console.WriteLine("Enter expression");
                    Console.WriteLine(calc.Calculate(Console.ReadLine()));
                }
            }
            catch (Exception e) when (
                e is RPNCalculatorException
                || e is ArgumentException
            )
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
