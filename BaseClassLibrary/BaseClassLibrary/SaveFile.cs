using BaseClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
//I don't know how to reference the path/motion profile class and google isn't helping.
//This might just take parameters instead.

public static class SaveFile {
    public static void WriteSaveFile(MotionProfile motionProfile, String path) {
        //Person will push out the class that has the path
        //We will be taking a "motion profile" as a parameter and it will return a 3D array
        //[Segment(0 -> pathLength), ControlPoint(0 -> 3), x or y(0 -> 1)]
        FileInfo fi = new FileInfo(path);

        using(StreamWriter sw = fi.CreateText()) {
            double[][][] segments = motionProfile.path.toArray();
            double[][] profile = motionProfile.toArray();
            double timeDifference = motionProfile.profileTime;

            //Writes the file header
            foreach(double[][] segment in segments) {
                sw.WriteLine(SegmentToLine(segment));
            }

            //Writes the motion profile
            foreach(double[] point in profile){
                sw.WriteLine(PointToString(point) + timeDifference.ToString());
            }
            sw.Close();
        }

        //The next thing will be outputted and will be the position, velocity, direction, and interval. 3 1D arrays
        //and a constant increment determined by motionProfile.getInterval
    }

    private static String SegmentToLine(double[][] segment) {
        String output = "";
        for(int i = 0; i <= 3; i++) {
            output += PairItems(segment[i]) + ",";
        }
        return output;
    }

    private static String PairItems(double[] point) {
        //Generates the control point pairs.
        return point[0].ToString() + ":" + point[1].ToString();
    }

    private static String PointToString(double[] point) {
        String output = ",";
        foreach(double number in point) {
            output += number.ToString() + ",";
        }
        return output;
    }

    public static double[][][] ReadSaveFile(String path) {
        FileInfo fi = new FileInfo(path);
        StreamReader sr = new StreamReader(path);
        Boolean reachedEnd = false;
        String line = "";
        List<double[][]> output = new List<double[][]>();

        while(!reachedEnd) {
            line = sr.ReadLine();
            //Lines starting with a comma are the profile lines
            if(line[0] == ',') {
                break;
            }
            //Prevents an empty term at the end of the array
            line = line.TrimEnd(',');
            output.Add(LineToSegments(line.Split(',')));
        }
        return output.ToArray();
    }

    private static double[][] LineToSegments(String[] points) {
        double[][] output = new double[points.Length][];
        for(int i = 0; i < points.Length; i++) {
            String[] point = points[i].Split(':');
            output[i][0] = Convert.ToDouble(point[0]);
            output[i][1] = Convert.ToDouble(point[1]);
        }
        return output;
    }

}
