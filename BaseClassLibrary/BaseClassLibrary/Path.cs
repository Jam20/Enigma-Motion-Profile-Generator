using System;
using System.Collections.Generic;

public class Path
{

    public List<Segment> PathList { get; private set; }
    private double[] firstPoint;
    public double TotalDistance { get; private set; }
    public bool IsReversed { get; set; }

    
    //Constructor with already determined Points list
    public Path(List<Segment> pathList = null , bool isReversed = false)
    {
        if (pathList == null) pathList = new List<Segment>();
        IsReversed = isReversed;
        PathList = pathList;
        SetTotalDistance();
    }
    public void DeleteSegment()
    {
        PathList.RemoveAt(PathList.Count - 1);
    }

    //Adds a point into the pathlist and standardizes the path to make it continous
    public void AddPoint(double[] newPt)
    {
        if (PathList.Count == 0)
        {
            if (firstPoint == null)
            {
                firstPoint = newPt;
            }
            else
            {
                PathList.Add(new Segment(firstPoint, newPt));
                SetTotalDistance();
            }
            return;
        }
        Segment newSegment = new Segment(PathList[PathList.Count - 1].ControlptFour, newPt);
        PathList.Add(newSegment);
        StandardizePath(PathList.Count-2);
        SetTotalDistance();
    }

    //Checks every segment and its ajesent segments for continuity and then ajusts if they are not continous
    public void StandardizePath()
    {
        if (PathList.Count < 2) return;
        for (int i = 0; i < PathList.Count - 1; i++)
        {
            StandardizePath(i);
        }
    }

    public void StandardizePath(int segmentIndex)
    {
        if (segmentIndex == PathList.Count - 1 || segmentIndex<0) return;
        double ratioOne = (PathList[segmentIndex].ControlptThree[1] - PathList[segmentIndex].ControlptFour[1]) / (PathList[segmentIndex].ControlptThree[0] - PathList[segmentIndex].ControlptFour[0]);
        double ratioTwo = (PathList[segmentIndex + 1].ControlptTwo[1] - PathList[segmentIndex].ControlptFour[1]) / (PathList[segmentIndex + 1].ControlptTwo[0] - PathList[segmentIndex].ControlptFour[0]);
        if ((PathList[segmentIndex].ControlptThree[0] - PathList[segmentIndex].ControlptFour[0]) == 0)
        {

            double dist = Math.Sqrt(Math.Pow(PathList[segmentIndex + 1].ControlptTwo[1] - PathList[segmentIndex].ControlptFour[1], 2) + Math.Pow(PathList[segmentIndex + 1].ControlptTwo[0] - PathList[segmentIndex].ControlptFour[0], 2));
            PathList[segmentIndex + 1].ControlptTwo = new double[] { PathList[segmentIndex].ControlptFour[0], PathList[segmentIndex].ControlptFour[1] + dist * -Math.Sign((PathList[segmentIndex].ControlptThree[1] - PathList[segmentIndex].ControlptFour[1])) };

        }
        else if (ratioOne != ratioTwo)
        {
            double xDist = Math.Abs(PathList[segmentIndex + 1].ControlptTwo[0] - PathList[segmentIndex].ControlptFour[0]);
            double yDist = Math.Abs(PathList[segmentIndex + 1].ControlptTwo[1] - PathList[segmentIndex].ControlptFour[1]);
            double distance = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
            double newXValue = Math.Sqrt(Math.Pow(distance, 2) / (Math.Pow(ratioOne, 2) + 1));
            double newYValue = newXValue * ratioOne;
            if (PathList[segmentIndex].ControlptThree[0] < PathList[segmentIndex].ControlptFour[0])
            {
                PathList[segmentIndex + 1].ControlptTwo = new double[] { PathList[segmentIndex].ControlptFour[0] + newXValue, PathList[segmentIndex].ControlptFour[1] + newYValue };
            }
            else
            {
                PathList[segmentIndex + 1].ControlptTwo = new double[] { PathList[segmentIndex].ControlptFour[0] - newXValue, PathList[segmentIndex].ControlptFour[1] - newYValue };
            }
            double newRatioTwo = PathList[segmentIndex + 1].ControlptTwo[1] - PathList[segmentIndex].ControlptFour[1] / (PathList[segmentIndex + 1].ControlptTwo[0] - PathList[segmentIndex].ControlptFour[0]);

        }
    }
    //based on a @param index the 0 based index of the location of the main point in the path is modified from its original value to @param newPoint
    public void ModifyPoint(int index, double[] newPoint)
    {
        if (newPoint.Length > 2) return;
        if (index == 0)
        {
            if (PathList.Count == 0) firstPoint = newPoint;
            else PathList[0].ControlptOne = newPoint;
        }
        else
        {
            PathList[index - 1].ControlptFour = (newPoint);
            if (PathList.Count > index)
            {
                PathList[index].ControlptOne = (newPoint);
                StandardizePath(index - 1);
            }
        }
    }



    //gets the a main point from the pathlist at @param index
    public double[] GetPoint(int index)
    {
        if (index == 0)
        {
            if (PathList.Count == 0) return firstPoint;
            else return PathList[0].ControlptOne;
        }
        else return PathList[index - 1].ControlptFour;
    }

    //uses the getSegmentLength method from each segment and totals the distance
    public void SetTotalDistance()
    {
        double output = 0;
        for (int i = 0; i < PathList.Count; i++)
        {
            PathList[i].SetSegmentLength();
            output += PathList[i].SegmentLength;
        }
        TotalDistance = output;
    }

    //gets a 2d array reperesentation of the pathlist with each dimention 1 being the segment index and dimention 2 being a array of the control points
    public double[][] GetPoints()
    {
        if (PathList.Count == 0)
        {
            if (firstPoint == null) return null;
            double[][] arry = new double[1][];
            arry[0] = firstPoint;
            return arry;
        }
        double[][] output = new double[PathList.Count + 1][];
        output[0] = PathList[0].ControlptOne;
        for (int i = 0; i < output.Length - 1; i++)
        {
            output[i + 1] = PathList[i].ControlptFour;
        }
        return output;
    }

    //given a @param dist we find which segment we would be in givin the size of distance than we find based on the distance from the start of that semgent the direction using that individual segments getDirectionAt method
    public double GetDirectionat(double dist)
    {
        double currentDistance = 0;
        int currentSegment = 0;
        while (currentDistance < dist)
        {
            if (currentSegment >= PathList.Count) currentDistance = dist;
            else
            {
                currentDistance += PathList[currentSegment].SegmentLength;
                currentSegment++;
            }
        }
        if (currentSegment != 0)
        {
            currentSegment--;
            currentDistance -= PathList[currentSegment].SegmentLength;
        }
        if (currentDistance == 0) return PathList[currentSegment].GetDirectionAt(dist);
        return PathList[currentSegment].GetDirectionAt(dist- currentDistance);
    }


    public double[][][] ToArray()
    {
        //Currently a placeholder that will be implemented later.
        double[][][] output = new double[PathList.Count][][];
        for (int i = 0; i < PathList.Count; i++)
        {
            output[i] = PathList[i].ToArry();
        }
        return output;
    }


}
