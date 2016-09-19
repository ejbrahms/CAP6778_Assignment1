using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace costSensitiveClassifierWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Rate> dataList = new List<Rate>();
            // Loop from 0 to 10, increment by 0.25.
            for (float i = 0.10f; i <= 50f; i = i + 0.10f)
            {
                //var wekaCommand = string.Format("java -classpath weka.jar weka.classifiers.meta.CostSensitiveClassifier -cost-matrix \" [0.0 {0}; {0} 0.0] \" -S 1 -W weka.classifiers.trees.J48 -t \"C:\\Users\\Eric\\OneDrive\\School\\Fall2016\\Advanced_Data_Mining\\Assignments\\Lymphoma96x4026\\Lymphoma96x4026.arff\" -- -C 0.25 -M 2 > \"C:\\Users\\Eric\\OneDrive\\School\\Fall2016\\Advanced_Data_Mining\\Assignments\\Assignment1\\Outputs\\cc{1}.txt\"", i, i.ToString().Replace(".", "p"));

                var iAsString = i.ToString().Replace(".", "p");

                Console.WriteLine("Calculating for {0}", i);
                var process = new Process();
                process.StartInfo.Arguments = string.Format("{0} {1}", i, iAsString);
                process.StartInfo.FileName = "runWeka.bat";
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                // Read the Weka output file.
                var outputFileName = string.Format("C:\\Users\\Eric\\OneDrive\\School\\Fall2016\\Advanced_Data_Mining\\Assignments\\Assignment1\\Outputs\\cc{0}.txt", iAsString);
                var fileText = File.ReadAllLines(outputFileName);

                // Find the indexes where the file contains "Confusion Matrix".
                var matrixIndexes = fileText.Select((s, idx) => new { str = s, index = idx })
                                            .Where(x => x.str.Contains("Confusion Matrix"))
                                            .Select(x => x.index)
                                            .ToList();

                // The second index is where we care about.
                var matrixText = fileText.Skip(matrixIndexes[1]).ToList();
                var matrixTopRow = matrixText[3].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var matrixBottomRow = matrixText[4].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                Rate r = new Rate();
                r.Cost = i;
                r.TP = Convert.ToInt32(matrixTopRow[0]);
                r.FN = Convert.ToInt32(matrixTopRow[1]);
                r.TN = Convert.ToInt32(matrixBottomRow[1]);
                r.FP = Convert.ToInt32(matrixBottomRow[0]);
                r.TypeI = (float)r.FP / ((float)r.TN + (float)r.FP) * 100;
                r.TypeII = (float)r.FN / ((float)r.TP + (float)r.FN) * 100;
                dataList.Add(r);

                Console.WriteLine("Type I={0}   Type II={1}", r.TypeI, r.TypeII);
            }

            Console.Write("Writing results...");
            writeFileFromList(dataList, @"C:\Users\Eric\OneDrive\School\Fall2016\Advanced_Data_Mining\Assignments\Assignment1\calculatedOutput.csv");

            Console.WriteLine("DONE!");
            Console.ReadLine();
        }

        private static void writeFileFromList(List<Rate> list, string outputFileName)
        {
            StringBuilder sb = new StringBuilder();
            // Write header row.
            string header = "Cost,TP,FP,TN,FN,Type I Error,Type II Error";
            sb.AppendLine(header);
            foreach(var row in list)
            {
                var currentLine = string.Format("{0},{1},{2},{3},{4},{5},{6}", row.Cost, row.TP, row.FP, row.TN, row.FN, row.TypeI, row.TypeII);
                sb.AppendLine(currentLine);
            }

            File.WriteAllText(outputFileName, sb.ToString());
        }

        struct Rate
        {
            public float Cost { get; set; }
            public int TP { get; set; }
            public int FN { get; set; }
            public int TN { get; set; }
            public int FP { get; set; }
            public float TypeI { get; set; }
            public float TypeII { get; set; }
        }
    }
}