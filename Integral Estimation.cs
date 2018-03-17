using System;
using static System.Console;
using static System.Math;

namespace ConsoleApplication
{
    static class Program
    {
        static void Main()
        {
            int numParts = 10000;
            
            double interval = PI/numParts;
            
            WriteLine( $"Upper estimation with {numParts} parts is " + UpperEst(numParts, interval));
            WriteLine( $"Lower estimation with {numParts} parts is " + LowerEst(numParts, interval));
            
        }
        
        static double UpperEst( int numParts, double interval )
        {
			double result = 0;
			double lBound = 0.0;
			for( int i = 0; i < numParts; i ++ )
			{
				result = result + interval * Max(Sin(lBound), Sin(lBound+interval));
				lBound += interval;
			}
			return result;
		}
		
        static double LowerEst( int numParts, double interval )
        {
			double result = 0;
			double lBound = 0.0;
			for( int i = 0; i < numParts; i ++ )
			{
				result = result + interval * Min(Sin(lBound), Sin(lBound+interval));
				lBound += interval;
			}
			return result;
		}
		
        
        
    }
}
