using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseClassLibrary;

namespace WindowsInterface
{
    static class SaveSystem
    {
        /// <summary>
        /// Returns strings containing the segments used to make the profile and the profile for the robot
        /// </summary>
        public static String[] MakeSaveFile(Player player)
        {
            /// <remarks>
            /// Automatically appends a header at the top that contains information such as date created, player information
            /// and project version.
            /// </remarks>

            List<String> output = new List<String>();

            output.Add("# Motion Profile");
            output.Add(String.Concat("# Team Number: ", player.TeamNumber.ToString()));
            output.Add(String.Concat("# Description: "));
            output.Add(String.Concat("# Date Created: ", DateTime.Now.ToString()));
            output.Add(String.Concat("# Robot Save Code: ", player.GetLayer(0).Profile.Robot.ToString()));
            

            for (int i = 0; i < player.GetNumberOfLayers(); i++)
            {
                String[] layer = LayerToStrings(player.GetLayer(i));
                foreach (String line in layer)
                {
                    output.Add(line);
                }
            }

            return output.ToArray();
           
        }

        private static String[] LayerToStrings(Layer layer)
        {

            //Sample Layer:
            //
            //==MyLayer0==
            //0:0,1:2,1:3,3:5
            //5:3,1:0,2:5,8:7
            //,314.34234,234.3243,43.34323,10
            //,314.34234,234.3243,43.34323,10
            //,314.34234,234.3243,43.34323,10

            double[][][] segments = layer.Profile.Path.ToArray();
            double[][] profile = layer.Profile.ToArray();
            String[] output = new String[profile.Length + segments.Length + 1];
            //The +1 is there for the first row, which is the layer divider
            String timeDifference = (layer.Profile.Robot.TimeIncrementInSec * 1000).ToString();
            //Writes the layer header(the location of the control points)

            output[0] = String.Concat("==", layer.Profile.Path.IsReversed, "==");
            
            for (int i = 0; i < segments.Length; i++)
            {
                //This and the following loop start at 1 to account for the first row
                output[i+1] = (SegmentToLine(segments[i]));
            }

            for (int i = 0; i < profile.Length; i++)
            {
                output[segments.Length + i+1] = (String.Concat(PointToString(profile[i]),String.Concat(",", timeDifference)));
            }

            return output;
        }

        private static String SegmentToLine(double[][] segment)
        {
            /// <summary>
            /// Returns a path segment as a line for saving.
            /// </summary>
            String output = "";
            for (int i = 0; i <= 3; i++)
            {
                output += String.Concat(PairItems(segment[i]), ",");
            }
            return output;
        }

        private static String PairItems(double[] point)
        {
            /// <returns>
            /// Returns paired control points for saving.
            /// </returns>
            return String.Join(':', point);/*String.Concat(point[0].ToString(), ":", point[1].ToString());*/
        }

        private static String PointToString(double[] point)
        {
            /// <returns>
            /// Returns a motion profile point as a line for saving.
            /// </returns>
            
            /*
            String output = ",";
            foreach (double number in point)
            {
                output += String.Concat(number.ToString(), ",");
            }
            return output;*/
            return String.Concat(",", String.Join(',', point));
        }

        public static Player LoadSaveFile(IList<String> lines)
        {
            String teamNumber = "";
            String description;
            Robot robot = new Robot();

            List<Layer> layers = new List<Layer>();
            String workingLayerName;
            List<Segment> workingSegments = new List<Segment>();
            


            foreach (String line in lines)
            {
                switch (line[0])
                {
                    case '#':
                        //Header lines + robot save string
                        String[] item = line.Split(": ");
                        switch (item[0])
                        {
                            case "# Team Number":
                                teamNumber = item[1];
                                break;
                            case "# Description":
                                description = item[1];
                                break;
                            case "# Robot Save Code":
                                robot = new Robot(item[1]);
                                break;
                        }
                        break;

                    case ',':
                        //motion profile lines
                        //This will just ignore it and move on
                        break;

                    case '=':
                        //indicates start of a new layer
                        //to make a layer, we need to convert the csv to segments to make a path,
                        //the path and robot to make a motion profile, and from there make a layer

                        workingLayerName = line.Replace("=", "");

                        bool isReversed = false;
                        if (workingLayerName.Equals("True")) isReversed = true;
                        workingSegments = new List<Segment>();
                        if(layers.Count == 0) layers.Add(new Layer(new MotionProfile(new Path(workingSegments, isReversed), robot), App.FieldCanvasWidth, App.FieldCanvasHeight));
                        else layers.Add(new Layer(new MotionProfile(new Path(workingSegments, isReversed), robot,layers[layers.Count-1].Profile), App.FieldCanvasWidth, App.FieldCanvasHeight));

                        break;
                    
                    default:
                        //segment lines since they can start with any number
                        workingSegments.Add(LineToSegments(line.TrimEnd(',').Split(',')));
                        break;

                }
            }

            return new Player(teamNumber, layers);
        }

        /*
        public static IList<Segment> ReadSaveFile(IList<String> lines)
        {
            /// <returns>
            /// Returns an array of segments to be parsed into an interactive path that can be modified.
            /// </returns>
            /// 

            //TODO: This will have to return a player object
            IList<Segment> output = new List<Segment>();

            foreach (String line in lines)
            {
                //Lines starting with a comma are the profile lines
                if (line[0] == ',')
                {
                    break;
                }
                //Prevents an empty term at the end of the array and then splits the line
                output.Add(LineToSegments(line.TrimEnd(',').Split(',')));
            }
            return output;
        }
        */

        private static Segment LineToSegments(String[] points)
        {
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

            for (int i = 0; i < points.Length; i++)
            {
                String[] point = points[i].Split(':');
                output[i][0] = Convert.ToDouble(point[0]);
                output[i][1] = Convert.ToDouble(point[1]);
            }
            return new Segment(output[0], output[1], output[2], output[3]);
        }
    }
}
