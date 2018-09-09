using System;
using System.Collections.Generic;

public class Path{

    private List<Segment> pathList;
    private int startingHeading;
    private int endingHeading;
    private double[] firstPoint;

    //Generic constuctor for first time use
    public Path() {
        pathList = new List<Segment>();
    }

    //Constructor with already determined Points list
    public Path(List<Segment> pathList) {
        this.pathList = pathList;
    }


    //Adds a point into the pathlist and standardizes the path to make it continous
    public void addPoint(double[] newPt) {
        if (pathList.Count ==0) {
            if (firstPoint == null) {
                firstPoint = newPt;
            }
            else {
                pathList.Add(new Segment(firstPoint, newPt));
            }
            return;
        }
        Segment newSegment = new Segment(pathList[pathList.Count - 1].getControlptFour(), newPt);
        pathList.Add(newSegment);
        standardizePath();
            
    }

    //Checks every segment and its ajesent segments for continuity and then ajusts if they are not continous
    public void standardizePath() {
        if (pathList.Count < 2) return;
        for (int i = 0; i < pathList.Count-1; i++) {
            double ratioOne = (pathList[i].getControlptThree()[1] - pathList[i].getControlptFour()[1]) / (pathList[i].getControlptThree()[0] - pathList[i].getControlptFour()[0]);
            double ratioTwo = (pathList[i+1].getControlptTwo()[1] - pathList[i].getControlptFour()[1]) / (pathList[i+1].getControlptTwo()[0] - pathList[i].getControlptFour()[0]);
            if((pathList[i].getControlptThree()[0] - pathList[i].getControlptFour()[0]) ==0)
            {
                
                double dist =Math.Sqrt( Math.Pow(pathList[i + 1].getControlptTwo()[1] - pathList[i].getControlptFour()[1], 2) + Math.Pow(pathList[i + 1].getControlptTwo()[0] - pathList[i].getControlptFour()[0], 2));
                pathList[i + 1].setControlptTwo(new double[] { pathList[i].getControlptFour()[0], pathList[i].getControlptFour()[1] + dist * -Math.Sign((pathList[i].getControlptThree()[1] - pathList[i].getControlptFour()[1])) });
                
            }
            else if(ratioOne != ratioTwo) {
                double xDist = Math.Abs(pathList[i + 1].getControlptTwo()[0] - pathList[i].getControlptFour()[0]);
                double yDist = Math.Abs(pathList[i + 1].getControlptTwo()[1] - pathList[i].getControlptFour()[1]);
                double distance = Math.Sqrt(Math.Pow(xDist, 2) + Math.Pow(yDist, 2));
                double newXValue = Math.Sqrt(Math.Pow(distance, 2) / (Math.Pow(ratioOne, 2) + 1));
                double newYValue = newXValue * ratioOne;
                if (pathList[i].getControlptThree()[0] < pathList[i].getControlptFour()[0]) {
                    pathList[i + 1].setControlptTwo(new double[] { pathList[i].getControlptFour()[0] + newXValue, pathList[i].getControlptFour()[1] + newYValue });
                }
                else {
                    pathList[i + 1].setControlptTwo(new double[] { pathList[i].getControlptFour()[0] - newXValue, pathList[i].getControlptFour()[1] - newYValue });
                }
                double newRatioTwo = (pathList[i + 1].getControlptTwo()[1] - pathList[i].getControlptFour()[1]) / (pathList[i + 1].getControlptTwo()[0] - pathList[i].getControlptFour()[0]);
                
            }
        }
    }
    
    //based on a @param index the 0 based index of the location of the main point in the path is modified from its original value to @param newPoint
    public void modifyPoint(int index, double[] newPoint){
        if (newPoint.Length > 2) return;
        if (index == 0) {
            if (pathList.Count == 0) firstPoint = newPoint;
            else pathList[0].setControlptOne(newPoint);
        }
        else {
            pathList[index - 1].setControlptFour(newPoint);
            if (pathList.Count >index) pathList[index].setControlptOne(newPoint);
        }
        standardizePath();
    }

    //Does the same as modifyPoint but instead of switching to acompletly new number simpily adds @param x,y to their respective values in the point
    public void modifyPointAdd(int index, double x, double y){
        
        if (index == 0)
        {
            if (pathList.Count == 0) firstPoint = new double[] { firstPoint[0] + x, firstPoint[1] + y };
            else pathList[0].setControlptOne(new double[] { firstPoint[0] + x, firstPoint[1] + y });
        }
        else
        {
            pathList[index - 1].setControlptFour(new double[] { firstPoint[0] + x, firstPoint[1] + y });
            if (pathList.Count > index) pathList[index].setControlptOne(new double[] { firstPoint[0] + x, firstPoint[1] + y });
        }
        standardizePath();
    }

    //gets the a main point from the pathlist at @param index
    public double[] getPoint(int index) {

        if (index == 0) {
            if (pathList.Count == 0) return firstPoint;
            else return pathList[0].getControlptOne();
        }
        else return pathList[index-1].getControlptFour();
    }

    //uses the getSegmentLength method from each segment and totals the distance
    public double getTotalDistance() {
        double output = 0;
        for(int i=0; i<pathList.Count; i++) {
            output += pathList[i].getSegmentLength();
        }
        return output;
    }

    //gets a 2d array reperesentation of the pathlist with each dimention 1 being the segment index and dimention 2 being a array of the control points
    public double[][] getPoints() {
        if(pathList.Count == 0) {
            if (firstPoint == null) return null;
            double[][] arry = new double[1][];
            arry[0] = firstPoint;
            return arry;
        }
        double[][] output = new double[pathList.Count+1][];
        output[0] = pathList[0].getControlptOne();
        for (int i = 0; i < output.Length-1; i++) {
            output[i+1] = pathList[i].getControlptFour();
        }
        return output;
    }

    //given a @param dist we find which segment we would be in givin the size of distance than we find based on the distance from the start of that semgent the direction using that individual segments getDirectionAt method
    public double getDirectionat(double dist) {
        double currentDistance = 0;
        int currentSegment = 0;
        while(currentDistance < dist) {
            currentDistance += pathList[currentSegment].getSegmentLength();
            currentSegment++;
        }
        if (currentSegment != 0) {
            currentSegment--;
            currentDistance -= pathList[currentSegment].getSegmentLength();
        }
        if(currentDistance == 0) return pathList[currentSegment].getDirectionAt(dist);
        return pathList[currentSegment].getDirectionAt(currentDistance - dist);
    }

    //gets the pathlist object 
    public List<Segment> getPathList() {
        return pathList;
    }

    public double[][][] toArray() {
        //Currently a placeholder that will be implemented later.
        double[][][] output = new double[pathList.Count][][];
        for(int i = 0; i < pathList.Count; i++) {
            output[i] = pathList[i].toArry();
        }
        return output;
    }

    
}
