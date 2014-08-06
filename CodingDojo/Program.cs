using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodingDojo
{
    class Program
    {
        public static void Main()
        {			
            var ways = banker.FindBreakingWays(100).ToList();
            ways.ForEach(x => Console.WriteLine(x.ToString()));
            Console.Read();
        }
    }
}
