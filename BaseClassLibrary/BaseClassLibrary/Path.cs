using System;
using System.Collections.Generic;

public class Path
{
    private List<Segment> pathList;
    private int startingHeading;
    private int endingHeading;
    private double[] firstPoint;

    public Path() {
        pathList = new List<Segment>();
    }

    public Path(List<Segment> pathList) {
        this.pathList = pathList;
    }

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

    public void standardizePath() {
        if (pathList.Count < 2) return;
        for (int i = 0; i < pathList.Count-1; i++) {
            double ratioOne = (pathList[i].getControlptThree()[1] - pathList[i].getControlptFour()[1]) / (pathList[i].getControlptThree()[0] - pathList[i].getControlptFour()[0]);
            double ratioTwo = (pathList[i+1].getControlptTwo()[1] - pathList[i].getControlptFour()[1]) / (pathList[i+1].getControlptTwo()[0] - pathList[i].getControlptFour()[0]);
            if(ratioOne != ratioTwo) {
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

    public double getTotalDistance() {
        double output = 0;
        for(int i=0; i<pathList.Count; i++) {
            output += pathList[i].getSegmentLength();
        }
        return output;
    }

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

    public double getDirectionat(double dist) {
        double currentDistance = 0;
        int currentSegment = 0;
        while(currentDistance < dist) {
            currentDistance += pathList[currentSegment].getSegmentLength();
            currentSegment++;
        }
        return pathList[currentSegment].getDirectionAt(currentDistance - dist);
    }

    public List<Segment> getPathList() {
        return pathList;
    }

    public double[][][] toArray() {
        //Currently a placeholder that will be implemented later.
        double[][][] output = new double[3][][];
        return output;
    }

    
}
