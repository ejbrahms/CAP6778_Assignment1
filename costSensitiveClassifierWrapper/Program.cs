using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace costSensitiveClassifierWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Loop from 0 to 10, increment by 0.25.
            for(float i = 0.25f; i <= 20f; i=i + 0.25f)
            {
                var wekaCommand = string.Format("java -classpath weka.jar weka.classifiers.meta.CostSensitiveClassifier -cost-matrix \" [0.0 {0}; {0} 0.0] \" -S 1 -W weka.classifiers.trees.J48 -t \"C:\\Users\\Eric\\OneDrive\\School\\Fall2016\\Advanced_Data_Mining\\Assignments\\Lymphoma96x4026\\Lymphoma96x4026.arff\" -- -C 0.25 -M 2 > \"C:\\Users\\Eric\\OneDrive\\School\\Fall2016\\Advanced_Data_Mining\\Assignments\\Assignment1\\Outputs\\cc{1}.txt\"", i, i.ToString().Replace(".", "p"));

                Console.WriteLine("Calculating for {0}", i);
                var process = new Process();
                process.StartInfo.Arguments = string.Format("{0} {1}", i, i.ToString().Replace(".", "p"));
                process.StartInfo.FileName = "runWeka.bat";
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
            }

            Console.WriteLine("DONE!");
            Console.ReadLine();
        }
    }
}