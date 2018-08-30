using System;

public class Segment
{
    const double Length_Accuracy_Const = .001;
    private double[] controlptOne, controlptTwo,
                     controlptThree, controlptFour;

    //basic constructor for a semgent in which we don't know all of the control points
	public Segment(){
        controlptOne = new double[2];
        controlptTwo = new double[2];
        controlptThree = new double[2];
        controlptFour = new double[2];
    }

    //constructor for a segment given the start point and end points 
    public Segment(double[] controlptOne, double[] controlptFour) {
        this.controlptOne = controlptOne;
        this.controlptFour = controlptFour;

        double xDist = controlptFour[0] - controlptOne[0];
        double yDist = controlptFour[1] - controlptOne[1];
        this.controlptTwo = new double[] { xDist * (1.0 / 3.0) +controlptOne[0], yDist * (1.0 / 3.0) + controlptOne[1]};
        this.controlptThree = new double[] { xDist * (2.0 / 3.0) + controlptOne[0], yDist * (2.0 / 3.0) + controlptOne[1]};
    }

    //Constructor for segements from the save file
    //Need to check if this method is actually needed or if the 2 point one is good enough
    public Segment(double[] controlptOne, double[] controlptTwo, double[] controlptThree, double[] controlptFour)
    {
        this.controlptOne = controlptOne;
        this.controlptTwo = controlptTwo;
        this.controlptThree = controlptThree;
        this.controlptFour = controlptFour;
    }

    //Setter methods for the 4 control points
    public void setControlptOne(double[] newPoint) {
        controlptOne = newPoint;
    }

    public void setControlptTwo(double[] newPoint) {
        controlptTwo = newPoint;
    }
    
    public void setControlptThree(double[] newPoint) {
        controlptThree = newPoint;
    }

    public void setControlptFour(double[] newPoint) {
        controlptFour = newPoint;
    }

    //given the control points gives the function on the x axis 
    public double xFunctionAt(double t) {
        return pow((1.0F - t), 3)*controlptOne[0] + 3 * t * pow((1.0F - t), 2)*controlptTwo[0]
            + 3 * pow(t, 2) * (1.0F - t)*controlptThree[0] + pow(t, 3)*controlptFour[0];
    }

    //same as xFunctionAt but gets the y value
    public double yFunctionAt(double t) {
        return pow((1.0F - t), 3) * controlptOne[1] + 3 * t * pow((1.0F - t), 2) * controlptTwo[1]
            + 3 * pow(t, 2) * (1.0F - t) * controlptThree[1] + pow(t, 3) * controlptFour[1];
    }

    //simple power method
    public static double pow(double num, int pow) {
        double output = num;
        for (int i = 0; i < pow-1; i++) {
            output *= num;
        }
        return output;
    }

    //uses the accuracy constant to determine the length using the distance formula
    public double getSegmentLength() {
        double output = 0;
        for (double i = 0.0; i < 1.0; i+=Length_Accuracy_Const) {
            double xDif = Math.Abs(xFunctionAt(i + Length_Accuracy_Const) - xFunctionAt(i));
            double yDif = Math.Abs(yFunctionAt(i + Length_Accuracy_Const) - yFunctionAt(i));
            output += Math.Sqrt(pow(xDif, 2) + pow(yDif, 2));
        }
        return output;
    }

    //gets the t value based on the distance 
    public double getTBasedOnDistance(double d) {
        double output = 0;
        double distance = 0;
        double resolution = Length_Accuracy_Const;
        
        while(output != d || !(output <d-.005 || output > d + .005)) {
            if(output >= 1) {
                resolution = resolution * .1;
                output = 0;
                distance = 0;
            }
            double xDif = Math.Abs(xFunctionAt(output + resolution) - xFunctionAt(output));
            double yDif = Math.Abs(yFunctionAt(output + resolution) - yFunctionAt(output));
            distance += Math.Sqrt(pow(xDif, 2) + pow(yDif, 2));
            output += resolution;
        }
        return output;
    }

    //gets the direction at a given distance
    public double getDirectionAt(double d)
    {
        double t = d / getSegmentLength(); //getTBasedOnDistance(d);
        double y = 3*Math.Pow(1-t,2)*(controlptTwo[1]-controlptOne[1])+6*(1-t)*t*(controlptThree[1]-controlptTwo[1])+3*Math.Pow(t,2)*(controlptFour[1]-controlptThree[1]);
        double x = 3 * Math.Pow(1 - t, 2) * (controlptTwo[0] - controlptOne[0]) + 6 * (1 - t) * t * (controlptThree[0] - controlptTwo[0]) + 3 * Math.Pow(t, 2) * (controlptFour[0] - controlptThree[0]);
        if (x == 0 && y > 0) return 0;
        else if (x==0) return 180;
        double output = Math.Atan2(y, x) / Math.PI * 180;
        if (output < 90) return 90 - output;
        else return output + 90;
        
    }

    //getter methods for the control points
    public double[] getControlptOne() {
        return controlptOne;
    }

    public double[] getControlptTwo() {
        return controlptTwo; 
    }

    public double[] getControlptThree() {
        return controlptThree;
    }

    public double[] getControlptFour() {
        return controlptFour;
    }

    public double[][] toArry() {
        double[][] output = new double[4][];
        output[0] = getControlptOne();
        output[1] = getControlptTwo();
        output[2] = getControlptThree();
        output[3] = getControlptFour();
        return output;
    }


}
