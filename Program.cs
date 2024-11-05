using System;

class Program
{
    static void Main()
    {
        int n = -1;
        double x, y, precision;

        Console.WriteLine("Solving systems of nonlinear equations using Newton's method.");
        while (n != 0)
        {
            Console.WriteLine("System of equations:");
            Console.WriteLine("1. sin(x) + 2 * y = 1.6");
            Console.WriteLine("2. cos(y - 1) = 1");

            Console.WriteLine("\nEnter an initial approximation for x and y.");
            Console.Write("x: ");
            while (!double.TryParse(Console.ReadLine(), out x))
            {
                Console.WriteLine("Invalid input. Please enter a valid double number: ");
            }

            Console.Write("y: ");
            while (!double.TryParse(Console.ReadLine(), out y))
            {
                Console.WriteLine("Invalid input. Please enter a valid double number: ");
            }

            Console.Write("\nEnter a precision Е: ");
            while (!double.TryParse(Console.ReadLine(), out precision))
            {
                Console.WriteLine("Invalid input. Please enter a valid number: ");
            }

            NewtonMethod(x, y, precision);

            Console.Write("Enter 1 to continue, 0 to exit: ");
            if (int.TryParse(Console.ReadLine(), out n))
            {
                continue;
            }
            else
            {
                Console.WriteLine("Invalid input. Exiting.");
                break;
            }
        }
    }

    static double[] SystemOfEquations(double x, double y)
    {
        return new double[]
        {
            Math.Sin(x) + 2 * y - 1.6,
            Math.Cos(y - 1) - 1
        };
    }

    static double[,] JacobiMatrix(double x, double y)
    {
        return new double[,]
        {
            { Math.Cos(x), 2 },
            { 0, -Math.Sin(y - 1) }
        };
    }

    static double[] FindValueOfVectorZ(double[,] A, double[] F)
    {
        // determinant A - Jacobi Matrix
        double detA = A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0];
        if (Math.Abs(detA) == 0)
        {
            throw new Exception("The matrix is degenerate, and the system cannot be solved.");
        }

        // A^(-1)
        double[,] A_inv = new double[,]
        {
        { A[1, 1] / detA, -A[0, 1] / detA },
        { -A[1, 0] / detA, A[0, 0] / detA }
        };

        // z = A^(-1) * F
        double[] z = new double[2];
        z[0] = A_inv[0, 0] * F[0] + A_inv[0, 1] * F[1];
        z[1] = A_inv[1, 0] * F[0] + A_inv[1, 1] * F[1];

        return z;
    }

    static void NewtonMethod(double x, double y, double precision)
    {
        int maxIterations = 100;
        int iteration = 1;

        while (iteration < maxIterations)
        {
            double[] F = SystemOfEquations(x, y);
            double[,] A = JacobiMatrix(x, y);

            double[] z = FindValueOfVectorZ(A, F);

            double z_norm = Math.Max(Math.Abs(z[0]), Math.Abs(z[1]));

            x -= z[0];
            y -= z[1];

            Console.WriteLine($"Iteration {iteration}:");
            Console.WriteLine($"k = {iteration - 1}");
            Console.WriteLine($"F_k = [{F[0]}, {F[1]}]");
            Console.WriteLine($"A_k = [({A[0, 0]}, {A[0, 1]}), ({A[1, 0]}, {A[1, 1]})]");
            Console.WriteLine($"z1 = {z[0]}, z2 = {z[1]}");
            Console.WriteLine($"z_norm = {z_norm}");
            Console.WriteLine($"\nx_k = {x}, y_k = {y}");
            Console.WriteLine($"-----------------------");

            if (z_norm < precision)
            {
                Console.WriteLine("Iteration: " + iteration + "\nSolution:");
                Console.WriteLine($"x = {x}");
                Console.WriteLine($"y = {y}");
                Console.WriteLine($"z_norm = {z_norm}");
                return;
            }

            iteration++;
        }

        Console.WriteLine("No solution found in 100 iterations.");
    }
}