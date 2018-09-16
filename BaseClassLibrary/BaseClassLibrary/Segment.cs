using System;

public class Segment
{
    const double Length_Accuracy_Const = .001;
    public double[] ControlptOne;
    public double[] ControlptTwo;
    public double[] ControlptThree;
    public double[] ControlptFour;

    //basic constructor for a semgent in which we don't know all of the control points
    public Segment()
    {
        ControlptOne = new double[2];
        ControlptTwo = new double[2];
        ControlptThree = new double[2];
        ControlptFour = new double[2];
    }

    //constructor for a segment given the start point and end points 
    public Segment(double[] controlptOne, double[] controlptFour)
    {
        this.ControlptOne = controlptOne;
        this.ControlptFour = controlptFour;

        double xDist = controlptFour[0] - controlptOne[0];
        double yDist = controlptFour[1] - controlptOne[1];
        this.ControlptTwo = new double[] { xDist * (1.0 / 3.0) + controlptOne[0], yDist * (1.0 / 3.0) + controlptOne[1] };
        this.ControlptThree = new double[] { xDist * (2.0 / 3.0) + controlptOne[0], yDist * (2.0 / 3.0) + controlptOne[1] };
    }

    //Constructor for segements from the save file
    //Need to check if this method is actually needed or if the 2 point one is good enough
    public Segment(double[] controlptOne, double[] controlptTwo, double[] controlptThree, double[] controlptFour)
    {
        this.ControlptOne = controlptOne;
        this.ControlptTwo = controlptTwo;
        this.ControlptThree = controlptThree;
        this.ControlptFour = controlptFour;
    }

    //given the control points gives the function on the x axis 
    public double XFunctionAt(double t)
    {
        return Math.Pow((1.0F - t), 3) * ControlptOne[0] + 3 * t * Math.Pow((1.0F - t), 2) * ControlptTwo[0]
            + 3 * Math.Pow(t, 2) * (1.0F - t) * ControlptThree[0] + Math.Pow(t, 3) * ControlptFour[0];
    }

    //same as xFunctionAt but gets the y value
    public double YFunctionAt(double t)
    {
        return Math.Pow((1.0F - t), 3) * ControlptOne[1] + 3 * t * Math.Pow((1.0F - t), 2) * ControlptTwo[1]
            + 3 * Math.Pow(t, 2) * (1.0F - t) * ControlptThree[1] + Math.Pow(t, 3) * ControlptFour[1];
    }


    //uses the accuracy constant to determine the length using the distance formula
    public double GetSegmentLength()
    {
        double output = 0;
        for (double i = 0.0; i < 1.0; i += Length_Accuracy_Const)
        {
            double xDif = Math.Abs(XFunctionAt(i + Length_Accuracy_Const) - XFunctionAt(i));
            double yDif = Math.Abs(YFunctionAt(i + Length_Accuracy_Const) - YFunctionAt(i));
            output += Math.Sqrt(Math.Pow(xDif, 2) + Math.Pow(yDif, 2));
        }
        return output;
    }

    //gets the t value based on the distance 
    public double GetTBasedOnDistance(double d)
    {
        double output = 0;
        double distance = 0;
        double resolution = Length_Accuracy_Const;

        while (output != d || !(output < d - .005 || output > d + .005))
        {
            if (output >= 1)
            {
                resolution = resolution * .1;
                output = 0;
                distance = 0;
            }
            double xDif = Math.Abs(XFunctionAt(output + resolution) - XFunctionAt(output));
            double yDif = Math.Abs(YFunctionAt(output + resolution) - YFunctionAt(output));
            distance += Math.Sqrt(Math.Pow(xDif, 2) + Math.Pow(yDif, 2));
            output += resolution;
        }
        return output;
    }

    //gets the direction at a given distance
    public double GetDirectionAt(double d)
    {
        double t = d / GetSegmentLength(); //getTBasedOnDistance(d);
        double y = 3 * Math.Pow(1 - t, 2) * (ControlptTwo[1] - ControlptOne[1]) + 6 * (1 - t) * t * (ControlptThree[1] - ControlptTwo[1]) + 3 * Math.Pow(t, 2) * (ControlptFour[1] - ControlptThree[1]);
        double x = 3 * Math.Pow(1 - t, 2) * (ControlptTwo[0] - ControlptOne[0]) + 6 * (1 - t) * t * (ControlptThree[0] - ControlptTwo[0]) + 3 * Math.Pow(t, 2) * (ControlptFour[0] - ControlptThree[0]);
        if (x == 0 && y > 0) return 0;
        else if (x == 0) return 180;
        double output = Math.Atan2(y, x) / Math.PI * 180;
        if (output < 90) return 90 - output;
        else return output + 90;

    }

    //Outputs the segment to an 2d array
    public double[][] ToArry()
    {
        double[][] output = new double[4][];
        output[0] = ControlptOne;
        output[1] = ControlptTwo;
        output[2] = ControlptThree;
        output[3] = ControlptFour;
        return output;
    }


}
