using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Task1
{
    class Solver
    {
        int n;
        double[] a;
        double[] b;
        double[] c;
        double[] p;
        double[] q;
        double[] f;
        double[] x;
        double[] solution;

        public Solver()
        {
            
        }

        private void Generate(int Dimention, int Range)
        {
            Random rand = new Random();
            for (int i = 0; i < Dimention - 1; i++)
            {
                a[i] = (double)rand.Next(-Range, Range);
                b[i] = (double)rand.Next(-Range, Range);
                c[i] = (double)rand.Next(-Range, Range);
                p[i] = (double)rand.Next(-Range, Range);
                q[i] = (double)rand.Next(-Range, Range);
              //  f[i] = (double)rand.Next(-Range, Range);
            }
            b[Dimention-1] = (double)rand.Next(-Range, Range);
            p[Dimention - 1] = (double)rand.Next(-Range, Range);
            q[Dimention - 1] = (double)rand.Next(-Range, Range);
            f[Dimention - 1] = (double)rand.Next(-Range, Range);

            p[Dimention - 3] = c[Dimention - 3];
            p[Dimention - 2] = b[Dimention - 2];
            p[Dimention - 1] = a[Dimention - 2];
            q[Dimention - 2] = c[Dimention - 2];
            q[Dimention - 1] = b[Dimention - 1];
            GenerateX1();
            Generate_f();
    
        }
        private void GenerateX1()
        {
            for (int i = 0; i < n; i++)
            {
                x[i] = 1;
            }
        }
        private void Generate_f()
        {
            f[0] = b[0] + c[0] + p[0] + q[0];
            for (int i = 1; i < n-3; i++)
            {
                f[i] = a[i - 1] + b[i] + c[i] + p[i] + q[i];
            }
            f[n - 3] = a[n - 4] + b[n - 3] + c[n - 3] + q[n - 3];
            f[n - 2] = a[n - 3] + b[n - 2] + c[n - 2];
            f[n - 1] = a[n - 2] + b[n - 1];
        }

        public void Form_Answer(int count, int Dimention, int Range, ref double avg_1, ref double avg_2)
        {
            n = Dimention;
            a = new double[n - 1];
            b = new double[n];
            c = new double[n - 1];
            p = new double[n];
            q = new double[n];
            f = new double[n];
            x = new double[n];
            solution = new double[n];
            Generate(n, Range);
            for (int i = 0; i < count; i++)
            {
                while (Solve() == null)
                {
                    Generate(n, Range);
                }
                avg_1 += Math.Abs(1 - Math.Max(solution.Max(), Math.Abs(solution.Min())));
            }
            avg_1 /= count;
        }

        public bool Check_Vectors()
        {
            for (int i = 0; i < n - 1; i++)
            {
                if (a[i] != 0 || c[i] != 0)
                    return false;
            }
            for (int i = 0; i < n - 2; i++)
                if (p[i] != 0 || q[i] != 0)
                    return false;
            if (p[n - 1] != 0 || q[n - 2] != 0)
                return false;
            return true;
        }

        public double[] Solve()
        {
            double div_result;
            // Обнуление вектора а
            for (int i = 1; i < n; i++)
            {
                if (b[i - 1] != 0 && a[i - 1] != 0)
                {
                    div_result = a[i - 1] / b[i - 1];
                    p[i] -= div_result * p[i - 1];
                    q[i] -= div_result * q[i - 1];
                    if (i == n - 3)
                        c[i] = p[i];
                    else if (i == n - 2)
                        c[i] = q[i];
                    b[i] -= div_result * c[i - 1];
                    f[i] -= div_result * f[i - 1];
                    a[i - 1] = 0;
                }
            } 
            // Обнуление правого предпоследнего элемента
            if (q[n - 2] != 0 && q[n - 1] != 0)
            {
                f[n - 2] -= q[n - 2] / q[n - 1] * f[n - 1];
                c[n - 2] = 0;
                q[n - 2] = 0;
                //c[n - 3] = 0;
            }
            // Обнуление n и n-1 столбцов
            for (int i = n - 3; i >= 0; i--)
            {
                if (p[i] != 0 && p[n - 2] != 0)
                {
                    f[i] -= f[n - 2] * (p[i] / p[n - 2]);
                    p[i] = 0;
                }
                if (q[i] != 0 && q[n - 1] != 0)
                {                        
                    f[i] -= f[n - 1] * (q[i] / q[n - 1]);
                    q[i] = 0;
                }
                if (Double.IsInfinity(f[i]))
                    return null;
             }
            c[n - 3] = 0;
            // Обнуление вектора с
            for (int i = n - 3; i > 0; i--)
            {
                if (b[i] != 0 && c[i - 1] != 0)
                {
                    div_result = c[i - 1] / b[i];
                    c[i - 1] = 0;
                    f[i - 1] -= div_result * f[i];
                }
            }
            // Формирование результата
            if (Check_Vectors())
            {
                for (int i = 0; i < n; i++)
                    if (b[i] == 0)
                        solution[i] = 0;
                    else solution[i] = f[i] / b[i];
                return solution;
            }
            else return null;
        }
    }
}
