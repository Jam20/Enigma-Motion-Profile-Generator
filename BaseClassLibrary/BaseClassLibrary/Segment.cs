﻿using System;

public class Segment
{
    const double Length_Accuracy_Const = .001;
    private double[] controlptOne, controlptTwo,
controlptThree, controlptFour;

	public Segment(){
        controlptOne = new double[2];
        controlptTwo = new double[2];
        controlptThree = new double[2];
        controlptFour = new double[2];
    }

    public Segment(double[] controlptOne, double[] controlptFour) {
        this.controlptOne = controlptOne;
        controlptTwo = new double[2];
        controlptThree = new double[2];
        this.controlptFour = controlptFour;
    }

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


    public double xFunctionAt(double t) {
        return pow((1.0F - t), 3)*controlptOne[0] + 3 * t * pow((1.0F - t), 2)*controlptTwo[0]
            + 3 * pow(t, 2) * (1.0F - t)*controlptThree[0] + pow(t, 3)*controlptFour[0];
    }

    public double yFunctionAt(double t) {
        return pow((1.0F - t), 3) * controlptOne[1] + 3 * t * pow((1.0F - t), 2) * controlptTwo[1]
            + 3 * pow(t, 2) * (1.0F - t) * controlptThree[1] + pow(t, 3) * controlptFour[1];
    }

    public static double pow(double num, int pow) {
        double output = num;
        for (int i = 0; i < pow-1; i++) {
            output *= num;
        }
        return output;
    }

    public double getSegmentLength() {
        double output = 0;
        for (double i = 0.0; i < 1.0; i+=Length_Accuracy_Const) {
            double xDif = Math.Abs(xFunctionAt(i + Length_Accuracy_Const) - xFunctionAt(i));
            double yDif = Math.Abs(yFunctionAt(i + Length_Accuracy_Const) - yFunctionAt(i));
            output += Math.Sqrt(pow(xDif, 2) + pow(yDif, 2));
        }
        return output;
    }
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

    public double getDirectionAt(double d)
    {
        double t = getTBasedOnDistance(d);
        double y = 3 * controlptOne[1] * pow((1 - t), 2) + 3 * pow((1 - t), 2) + 3 * t * 2 * controlptTwo[1] * (1 - t) + 18 * t * (1 - t) + 9 * pow(t, 2) * controlptThree[1] + 81 * pow(t, 2) * controlptFour[1];
        double x = 3 * controlptOne[0] * pow((1 - t), 2) + 3 * pow((1 - t), 2) + 3 * t * 2 * controlptTwo[0] * (1 - t) + 18 * t * (1 - t) + 9 * pow(t, 2) * controlptThree[0] + 81 * pow(t, 2) * controlptFour[0];
        return Math.Atan2(y, x);
    }

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
}
