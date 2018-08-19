using System;
using System.Collections.Generic;
using System.IO;
using BaseClassLibrary;


public static class SaveFile {
    public static List<string> GetSaveFile(MotionProfile motionProfile, String path) {
        //Person will push out the class that has the path
        //We will be taking a "motion profile" as a parameter and it will return a 3D array
        //[Segment(0 -> pathLength), ControlPoint(0 -> 3), x or y(0 -> 1)]
        //FileInfo fi = new FileInfo(path);
        List<string> output = new List<string>();

        //using(StreamWriter sw = fi.CreateText()) {
            
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
                //sw.WriteLine(SegmentToLine(segment));
            }

            //Writes the motion profile
            foreach(double[] point in profile){
                output.Add(PointToString(point) + timeDifference.ToString());
                //sw.WriteLine(PointToString(point) + timeDifference.ToString());
            }
            /*
            sw.Close();
        }
        */

        //The next thing will be outputted and will be the position, velocity, direction, and interval. 3 1D arrays
        //and a constant increment determined by motionProfile.getInterval

        return output;
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
