using System;
using System.IO;

public static class ClassSaveFile {
    public static void WriteSaveFile(double[] pathInput, String path) {
        //Person will push out the class that has the path
        //We will be taking a "motion profile" as a parameter and it will return a 3D array
        //[Segment(0 -> pathLength), ControlPoint(0 -> 3), x or y(0 -> 1)]
        FileInfo fi = new FileInfo(path);

        using (StreamWriter sw = fi.CreateText()) {
            double[][][] segments = new double[2][][];
            /*segments[0] = new double[4][]
            {
                new double[] { 1, 2},
                new double[] { 3, 4},
                new double[] { 5, 6},
                new double[] { 7, 8}
            };
            segments[1] = new double[4][]
            {
                new double[] { 1.4, 2.4},
                new double[] { 3.4, 4.4},
                new double[] { 5.4, 6.4},
                new double[] { 7.4, 8.4}
            };*/
            foreach (double[][] segment in segments) {
                sw.WriteLine(SegmentToLine(segment));
            }
        }

        //The next thing will be outputted and will be the position, velocity, direction, and interval. 3 1D arrays
        //and a constant increment determined by motionProfile.getInterval
    }

    private static String SegmentToLine(double[][] segment) {
        String output = "";
        for (UInt16 i = 0; i <= 3; i++) {
            output += PairItems(segment[i]) + ",";
        }
        output.TrimEnd(',');
        return output;
    }

    private static String PairItems(double[] point) {
        //Generates the control point pairs.
        return point[0].ToString() + ":" + point[1].ToString();
    }

    public static double[][][] ReadSaveFile(String path) {
        FileInfo fi = new FileInfo(path);
        FileStream fs = fi.OpenRead();
        return new double[5][][];
    }
}
