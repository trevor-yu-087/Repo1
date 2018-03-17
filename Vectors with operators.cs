using System;
using static System.Console;

namespace Bme121
{
    
    
    class Vector
    {
        double x;
        double y;
        double z;
        
        public Vector() {x = 0; y = 0; z = 0;}
        
        public Vector( double x, double y, double z )
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public override string ToString()
        {
           string formatY = null;
           string formatZ = null;
            if( y > 0 ) { formatY = $"+{y:f2}";}
            else { formatY = $"{y:f2}";}
            if( z > 0 ) { formatZ = $"+{z:f2}";}
            else { formatZ = $"{z:f2}";}
            
            return string.Format( "[{0:f2}i {1:f2}j {2:f2}k]", x, formatY, formatZ );
        }
        
        public double DotProd( Vector U )           //Dot Product, returns double
        {
            return this.x*U.x + this.y*U.y + this.z*U.z;
        }
        
        public static double operator % (Vector V, Vector U) //Dot product operator
        {
            return V.x*U.x + V.y*U.y + V.z*U.z;
        }
        
        public Vector Minus ( Vector V )            //Vector subraction, returns vector
        {
            double x = this.x - V.x;
            double y = this.y - V.y;
            double z = this.z - V.z;
            
            Vector output = new Vector( x, y, z );
            return output;
        }
        
        public static Vector operator - ( Vector U, Vector V ) //Minus operator
        {
            double x = U.x - V.x;
            double y = U.y - V.y;
            double z = U.z - V.z;
            
            Vector output = new Vector( x, y, z );
            return output;
        }
        
        public Vector Plus ( Vector U )             //Vector addition, returns vector
        {
            double x = this.x + U.x;
            double y = this.y + U.y;
            double z = this.z + U.z;
            
            Vector output = new Vector (x, y, z);   
            return output;
        }
        
        public static Vector operator + (Vector U, Vector V)    //Plus operator
        {
            double x = V.x + U.x;
            double y = V.y + U.y;
            double z = V.z + U.z;
            
            Vector output = new Vector (x, y, z);   
            return output;
        }
        
        public double Magnitude      //Magnitude, returns double
        {
            get
            {
                return Math.Sqrt( x*x + y*y + z*z );
            }
        }
        
        public Vector CrossProd( Vector U )     //Cross product, returns vector
        {
            //  i  j  k  
            // t.x t.y t.z
            // U.x U.y U.z
            
            double i = this.y*U.z - U.y*this.z;
            double j = this.x*U.z - U.x*this.z;
            double k = this.x*U.y - U.x*this.y;
            
            Vector Result = new Vector (i, -j, k);
            return Result;
        }
        
        public static Vector operator ^ (Vector V, Vector U)    //Cross product operator
        {
            double i = V.y*U.z - U.y*V.z;
            double j = V.x*U.z - U.x*V.z;
            double k = V.x*U.y - U.x*V.y;
            
            Vector Result = new Vector (i, -j, k);
            return Result;
        }
        
        public static double CosTheta( Vector U, Vector V )     //Computes cos theta, returns double
        {
            double result = U.DotProd( V ) / (U.Magnitude * V.Magnitude);
            return result;
        }
        
        public Vector UnitVec    //Calculates unit vector of given vector, returns vector
        {
            get
            {
                double x = this.x / this.Magnitude;
                double y = this.y / this.Magnitude;
                double z = this.z / this.Magnitude;
                Vector result = new Vector( x, y, z);
                return result;
            }
        }
        
        public Vector ScaleUnitVec( double a )     //Calculates unit vector of given vector, returns scalar multiple
        {
            double x = this.x / this.Magnitude;
            double y = this.y / this.Magnitude;
            double z = this.z / this.Magnitude;
            Vector result = new Vector( a*x, a*y, a*z);
            return result;
        }
        
        public Vector ScalarMult( double a )        //Multiplies all components by scalar, returns vector
        {
            Vector result = new Vector( this.x * a, this.y * a, this.z * a );
            return result;
        }
        
        public static Vector operator * (double a, Vector V)
        {
            Vector result = new Vector( V.x * a, V.y * a, V.z * a );
            return result;
        }
        
        public Vector ParallelTo( Vector V )
        {
            return V.ScaleUnitVec( this.DotProd( V.UnitVec ) );
        }
        
        public Vector NormalTo( Vector V )
        {
            return this.Minus( this.ParallelTo(V) );
        }
        
        public static double TripleProduct( Vector V, Vector U, Vector W ) //V dot (U x W )
        {
            return V.DotProd( U.CrossProd( W ) );
        }
    }
    
    static class Program
    {
        static Program( ) { OutputEncoding = System.Text.Encoding.Unicode; }
        
        static void Main( )
        {
        /*
        Availible vector methods: 
        Vector.ScaleUnitVec( double ) --> returns scaled unit vector
        Vector.Plus(Vector), Vector.Minus(Vector), Vector.ScalarMult(double) --> returns vector
        Vector.ParallelTo(Vector), Vector.NormalTo(Vector) --> returns vector
        Vector.CrossProd(Vector) --> returns vector
        Vector.DotProd(Vector) --> returns double
        Vector.CosTheta( Vector1, Vector2 ) --> returns double
        
        Properties:
        Vector.Magnitude --> returns double
        Vector.UnitVec --> returns vector
        + will do addition
        - will do subtraction
        * will do scalar multiplaction with double
        ^ will do cross product
        % will do dot product
        */
        
    
       
        }
    }
}
