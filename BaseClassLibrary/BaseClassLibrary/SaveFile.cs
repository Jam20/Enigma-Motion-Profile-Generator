using BaseClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;

public static class SaveFile {
    /// <summary>
    /// Returns strings containing the segments used to make the profile and the profile for the robot
    /// </summary>
    public static List<string> GetSaveFile(MotionProfile motionProfile) {
        List<string> output = new List<string>();

        double[][][] segments = motionProfile.path.toArray();
        double[][] profile = motionProfile.toArray();
        double timeDifference = motionProfile.robot.timeIncrementInSec*1000;

        /*
        double[][][] segments = new double[][][] {
            new double[4][] {
                new double[2] {1, 2},
                new double[2] {3, 4},
                new double[2] {5, 6},
                new double[2] {7, 8}
            },
            new double[4][] {
                new double[2] {10, 20},
                new double[2] {30, 40},
                new double[2] {50, 60},
                new double[2] {70, 80}
            },
            new double[4][] {
                new double[2] {14, 24},
                new double[2] {34, 44},
                new double[2] {54, 64},
                new double[2] {74, 84}
            }
        };
        double[][] profile = new double[][] {
            new double[4] {77, 88, 99, 1010},
            new double[4] {777, 888, 99, 101010},
            new double[4] {7777, 8888, 999, 10101010}
        };
        double timeDifference = 10;
        */

        //Writes the file header
        foreach(double[][] segment in segments) {
            output.Add(SegmentToLine(segment));
        }

        //Writes the motion profile
        foreach(double[] point in profile) {
            output.Add(PointToString(point) + timeDifference.ToString());
        }
        return output;
    }

    private static String SegmentToLine(double[][] segment) {
        /// <summary>
        /// Returns a path segment as a line for saving.
        /// </summary>
        String output = "";
        for(int i = 0; i <= 3; i++) {
            output += PairItems(segment[i]) + ",";
        }
        return output;
    }

    private static String PairItems(double[] point) {
        /// <returns>
        /// Returns paired control points for saving.
        /// </returns>
        return point[0].ToString() + ":" + point[1].ToString();
    }

    private static String PointToString(double[] point) {
        /// <returns>
        /// Returns a motion profile point as a line for saving.
        /// </returns>
        String output = ",";
        foreach(double number in point) {
            output += number.ToString() + ",";
        }
        return output;
    }

    public static double[][][] ReadSaveFile(String path) {
        /// <returns>
        /// Returns an array of segments to be parsed into an interactive path that can be modified.
        /// </returns>
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
        /// <returns>
        /// Returns an array representing a path segment given an array of control point pairs.
        /// </returns>
        double[][] output = new double[points.Length][];
        for(int i = 0; i < points.Length; i++) {
            String[] point = points[i].Split(':');
            output[i][0] = Convert.ToDouble(point[0]);
            output[i][1] = Convert.ToDouble(point[1]);
        }
        return output;
    }

}
