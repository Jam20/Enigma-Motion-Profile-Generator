using System;
public class Robot
{
    public double maxVel;
    public double maxAccel;
    public double timeIncrementInSec;

    //useful information on the robot to be used in graphical interfaces
    public enum DrivetrainType { Tank }
    public double width;
    public double length;
    public double wheelSize;
    public bool usingBumpers;
    public double bumperThickness;

    public Robot()
    {

    }
    public Robot(string input)
    {
        String[] data = input.Split(' ');
        maxVel = Double.Parse(data[0]);
        maxAccel = Double.Parse(data[1]);
        timeIncrementInSec = Double.Parse(data[2]);
        width = Double.Parse(data[3]);
        length = Double.Parse(data[4]);
        wheelSize = Double.Parse(data[5]);
        usingBumpers = bool.Parse(data[6]);
        bumperThickness = Double.Parse(data[7]);
    }


    public override string ToString() {
        return "" + maxVel + " " + maxAccel + " " + timeIncrementInSec + " " + width + " " + length + " " + wheelSize + " " + usingBumpers + " " + bumperThickness;
    }


}


