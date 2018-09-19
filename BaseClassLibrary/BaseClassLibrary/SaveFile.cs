using BaseClassLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public static class SaveFile {
    /// <summary>
    /// Returns strings containing the segments used to make the profile and the profile for the robot
    /// </summary>
    public static String[] GetSaveFile(MotionProfile motionProfile) {
        

        double[][][] segments = motionProfile.Path.ToArray();
        double[][] profile = motionProfile.ToArray();
        String[] output = new String[profile.Length + segments.Length];
        double timeDifference = motionProfile.Robot.TimeIncrementInSec*1000;
        var watchHeader = System.Diagnostics.Stopwatch.StartNew();
        //Writes the file header
        //Runs in only a couple milliseconds
        for (int i = 0; i < segments.Length; i++) {
            output[i] = (SegmentToLine(segments[i]));
            Debug.WriteLine(watchHeader.ElapsedMilliseconds);
        }
        watchHeader.Stop();
        Debug.WriteLine(watchHeader.ElapsedMilliseconds);

        var watchProfile = System.Diagnostics.Stopwatch.StartNew();
        //Writes the motion profile
        //Very Slow
        for(int i =0; i<profile.Length; i++) {
            output[segments.Length+i] = (PointToString(profile[i]) + timeDifference);
            Debug.WriteLine(watchProfile.ElapsedMilliseconds);
        }
        watchProfile.Stop();
        Debug.WriteLine(watchProfile.ElapsedMilliseconds);

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

    public static IList<Segment> ReadSaveFile(IList<String> lines) {
        /// <returns>
        /// Returns an array of segments to be parsed into an interactive path that can be modified.
        /// </returns>
        IList<Segment> output = new List<Segment>();

        foreach (String line in lines)
        {
            //Lines starting with a comma are the profile lines
            if(line[0] == ',')
            {
                break;
            }
            //Prevents an empty term at the end of the array and then splits the line
            output.Add(LineToSegments(line.TrimEnd(',').Split(',')));
        }
        return output;
    }

    private static Segment LineToSegments(String[] points) {
        /// <returns>
        /// Returns an array representing a path segment given an array of control point pairs.
        /// </returns>
        double[][] output = new double[4][]
        {
            new double[2],
            new double[2],
            new double[2],
            new double[2]
        };
        
        for (int i = 0; i < points.Length; i++) {
            String[] point = points[i].Split(':');
            output[i][0] = Convert.ToDouble(point[0]);
            output[i][1] = Convert.ToDouble(point[1]);
        }
        return new Segment(output[0], output[1], output[2], output[3]);
    }

}
