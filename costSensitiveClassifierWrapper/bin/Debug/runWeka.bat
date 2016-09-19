@echo off
rem Change to weka directory
cd "C:\Program Files\Weka-3-6"
rem Run the input
java -classpath weka.jar weka.classifiers.meta.CostSensitiveClassifier -cost-matrix "[0.0 %1;1.0 0.0]" -S 1 -W weka.classifiers.trees.J48 -t "C:\Users\Eric\OneDrive\School\Fall2016\Advanced_Data_Mining\Assignments\Lymphoma96x4026\Lymphoma96x4026.arff" -- -C 0.25 -M 2 > "C:\Users\Eric\OneDrive\School\Fall2016\Advanced_Data_Mining\Assignments\Assignment1\Outputs\cc%2.txt"