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
        if (pathList.Count == 0) {
            firstPoint = newPt;
            return;
        }
        Segment newSegment = new Segment(pathList[pathList.Count - 1].getControlptFour(), newPt);
        pathList.Add(newSegment);
        standardizePath();
            
    }

    private void standardizePath() {
        if (pathList.Count < 2) return;
        for (int i = 0; i < pathList.Count-1; i++) {
            bool isContinous = (pathList[i].getControlptThree()[1] - pathList[i + 1].getControlptTwo()[1]) / (pathList[i].getControlptThree()[0] - pathList[i + 1].getControlptTwo()[0]) == (pathList[i].getControlptThree()[1] - pathList[i].getControlptFour()[1]) / (pathList[i].getControlptThree()[0] - pathList[i].getControlptFour()[0]);
            if (!isContinous) {
                double s = (pathList[i].getControlptThree()[1] - pathList[i + 1].getControlptTwo()[1]) / (pathList[i].getControlptThree()[0] - pathList[i + 1].getControlptTwo()[0]);
                double d = Segment.pow(Math.Abs(pathList[i].getControlptFour()[0] - pathList[i + 1].getControlptTwo()[0]), 2) + Segment.pow(Math.Abs(pathList[i].getControlptFour()[1] - pathList[i + 1].getControlptTwo()[1]), 2);
                double b = Math.Sqrt((d / (2 * Segment.pow(s, 2))));
                double a = b * s;
                double[] newControlptTwo = { pathList[i].getControlptFour()[0] + a, pathList[i].getControlptFour()[1] + b };
                pathList[i + 1].setControlptTwo(newControlptTwo);
            }
            
        }
    }

    public void modifyPoint(int index, double[] newPoint){
        if (newPoint.Length > 2) return;
        if (index == 0) pathList[0].setControlptOne(newPoint);
        else {
            pathList[index - 1].setControlptFour(newPoint);
            pathList[index].setControlptOne(newPoint);
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

    
}
