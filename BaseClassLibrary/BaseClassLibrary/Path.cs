using System;

public class Path
{
    private List<Segment> pathList;
    private int startingHeading;
    private int endingHeading;
    private int[] firstPoint;

    public Path() {
        pathList = new List<Segment>();
    }

    public Path(List<Segment> pathList) {
        this.pathList = pathList;
    }

    public void addPoint(int[] newPt) {
        if (pathList.Length == 0) {
            firstPoint = newPt;
            return;
        }
        Segment newSegment = new Segment(pathList[pathList.Length - 1].getControlptFour(), newPt);
        StandardizePath;
            
    }

    private void StandardizePath() {
        if (pathList.Length < 2) return;
        for (int i = 0; i < pathList.Length-1; i++) {
            bool isContinous = (pathList[i].getControlptThree()[1] - pathList[i + 1].getControlptTwo()[1]) / (pathList[i].getControlptThree()[0] - pathList[i + 1].getControlptTwo()[0]) == (pathList[i].getControlptThree()[1] - pathList[i].getControlptFour()[1]) / (pathList[i].getControlptThree()[0] - pathList[i].getControlptFour()[0]);
            if (!isContinous) {
                float s = (pathList[i].getControlptThree()[1] - pathList[i + 1].getControlptTwo()[1]) / (pathList[i].getControlptThree()[0] - pathList[i + 1].getControlptTwo()[0]);
                float d = Segment.pow(Math.Abs(pathList[i].getControlptFour[0] - pathList[i + 1].getControlptTwo[0]), 2) + Segment.pow(Math.Abs(pathList[i].getControlptFour[1] - pathList[i + 1].getControlptTwo[1]), 2);
                float b = Math.Sqrt((d / (2 * Segment.pow(s, 2))));
                float a = b * s;
                int[] newControlptTwo = { pathList[i].getControlptFour[0] + a, pathList[i].getControlptFour[1] + b };
                pathList[i + 1].setControlptTwo(newControlptTwo);
            }
            
        }
    }

}
