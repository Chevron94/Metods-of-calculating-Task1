using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

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
        double[] f_real;
        double[] f;
        double[] x;
        double[] x_real;
        double[] solution;
        double[] solution_real;

        public Solver()
        {
            
        }

        private void Generate(int Dimention, int Range) // генерация векторов (размерность, диапазон значений)
        {
            Random rand = new Random();
            for (int i = 0; i < Dimention - 1; i++)
            {
                a[i] = 1.001 * rand.Next(-Range, Range);
                b[i] = 1.001 * rand.Next(-Range, Range);
                c[i] = 1.001 * rand.Next(-Range, Range);
                p[i] = 1.001 * rand.Next(-Range, Range);
                q[i] = 1.001 * rand.Next(-Range, Range);
                x_real[i] = 1.001 * rand.Next(-Range, Range);
                x[i] = 1;
            }
            b[Dimention - 1] = 1.001 * rand.Next(-Range, Range);
            p[Dimention - 1] = 1.001 * rand.Next(-Range, Range);
            q[Dimention - 1] = 1.001 * rand.Next(-Range, Range);
            f[Dimention - 1] = 1.001*rand.Next(-Range, Range);
            x_real[Dimention - 1] = 1.001 * rand.Next(-Range, Range);
            x[Dimention - 1] = 1;


            p[Dimention - 3] = c[Dimention - 3];
            p[Dimention - 2] = b[Dimention - 2];
            p[Dimention - 1] = a[Dimention - 2];
            q[Dimention - 2] = c[Dimention - 2];
            q[Dimention - 1] = b[Dimention - 1];
            Generate_f();
            Generate_f_real();
            Thread.Sleep(1);
    
        }
        private void Generate_f() // генерация вектора f для еденичного решения
        {
            f[0] = (b[0] + c[0] + p[0] + q[0]);
            for (int i = 1; i < n-3; i++)
            {
                f[i] = (a[i - 1] + b[i] + c[i] + p[i] + q[i]);
            }
            f[n - 3] = (a[n - 4] + b[n - 3] + c[n - 3] + q[n - 3]);
            f[n - 2] = (a[n - 3] + b[n - 2] + c[n - 2]);
            f[n - 1] = (a[n - 2] + b[n - 1]);
        }
        private void Generate_f_real()
        {
            f_real[0] = b[0] * x_real[0] + c[0] * x_real[1] + p[0] * x_real[n - 2] + q[0] * x_real[n - 1];
            for (int i = 1; i < n - 3; i++)
            {
                f_real[i] = a[i - 1] * x_real[i - 1] + b[i] * x_real[i] + c[i] * x_real[i + 1] + p[i] * x_real[n - 2] + q[i] * x_real[n - 1];
            }
            f_real[n - 3] = (a[n - 4] * x_real[n - 4] + b[n - 3] * x_real[n - 3] + c[n - 3] * x_real[n - 2] + q[n - 3] * x_real[n - 1]);
            f_real[n - 2] = (a[n - 3] * x_real[n - 3] + b[n - 2] * x_real[n - 2] + c[n - 2] * x_real[n - 1]);
            f_real[n - 1] = (a[n - 2] * x_real[n - 2] + b[n - 1] * x_real[n - 1]);
        }

        private double Get_Avg(double[] solution, double[] x)
        {
            double res = 0;
            for (int i = 0; i < n; i++)
            {
                if (x[i] != 0)
                    res = Math.Max(res, Math.Abs(solution[i] - x[i]) / x[i]);
                else res = Math.Max(res, Math.Abs(solution[i] - x[i]));
            }
            return res;
        }

        public void Form_Answer(int count, int Dimention, int Range, ref double avg_1, ref double avg_2)// формирование выходных данных
            //( число тесов, размерность, диапазон, относ. погрешность, оценка точности)
        {
            n = Dimention;
            a = new double[n - 1];
            b = new double[n];
            c = new double[n - 1];
            p = new double[n];
            q = new double[n];

            f = new double[n];
            f_real = new double[n];

            x = new double[n] ;
            x_real = new double[n];

            solution_real = new double[n];
            solution = new double[n];
            avg_1 = 0;
            avg_2 = 0;
            for (int i = 0; i < count; i++)
            {
                Generate(n, Range);
                while (Solve() == null)
                {
                    Generate(n, Range);
                }
                avg_1 += Get_Avg(solution_real, x_real);
                avg_2 += Get_Avg(solution, x);
                //avg_1 += Math.Max(Math.Abs(solution.Max() - 1), Math.Abs(solution.Min() - 1));
                //avg_2 = Math.Max(avg_2, Math.Max(Math.Abs(solution.Max() - 1), Math.Abs(solution.Min() - 1)));
            }
            avg_1 /= count;
            avg_2 /= count;
        }

        private bool Check_Vectors()
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

        public double[] Solve()// решение системы
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
                    f_real[i] -= div_result * f_real[i - 1]; 

                    a[i - 1] = 0;
                }
            } 
            // Обнуление правого предпоследнего элемента
            if (q[n - 2] != 0 && q[n - 1] != 0)
            {
                f[n - 2] -= q[n - 2] / q[n - 1] * f[n - 1];
                f_real[n - 2] -= q[n - 2] / q[n - 1] * f_real[n - 1];

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
                    f_real[i] -= f_real[n - 2] * (p[i] / p[n - 2]);
                    p[i] = 0;
                }
                if (q[i] != 0 && q[n - 1] != 0)
                {                        
                    f[i] -= f[n - 1] * (q[i] / q[n - 1]);
                    f_real[i] -= f_real[n - 1] * (q[i] / q[n - 1]);

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
                    f_real[i - 1] -= div_result * f_real[i];
                }
            }
            // Формирование результата
            if (Check_Vectors())
            {
                for (int i = 0; i < n; i++)
                    if (b[i] == 0)
                    {
                        solution[i] = 0;
                        solution_real[i] = 0;
                    }
                    else
                    {
                        solution[i] = f[i] / b[i];
                        solution_real[i] = f_real[i] / b[i];
                    }
                return solution;
            }
            else return null;
        }
    }
}
