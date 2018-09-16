public class Robot
{
    public double MaxVel;
    public double MaxAccel;
    public double TimeIncrementInSec;

    //useful information on the robot to be used in graphical interfaces
    public enum DrivetrainType { Tank }
    public double Width;
    public double Length;
    public double WheelSize;
    public bool UsingBumpers;
    public double BumperThickness;

    public Robot()
    {

    }
    public Robot(string input)
    {
        string[] data = input.Split(' ');
        MaxVel = double.Parse(data[0]);
        MaxAccel = double.Parse(data[1]);
        TimeIncrementInSec = double.Parse(data[2]);
        Width = double.Parse(data[3]);
        Length = double.Parse(data[4]);
        WheelSize = double.Parse(data[5]);
        UsingBumpers = bool.Parse(data[6]);
        BumperThickness = double.Parse(data[7]);
    }


    public override string ToString()
    {
        return "" + MaxVel + " " + MaxAccel + " " + TimeIncrementInSec + " " + Width + " " + Length + " " + WheelSize + " " + UsingBumpers + " " + BumperThickness;
    }


}


