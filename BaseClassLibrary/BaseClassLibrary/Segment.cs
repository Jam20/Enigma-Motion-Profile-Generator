using System;

public class Segment
{
    const int Length_Accuracy_Const = .001;
    private int[] controlptOne, controlptTwo, controlptThree, controlptFour;

	public Segment(){
        controlptOne = new int[2];
        controlptTwo = new int[2];
        controlptThree = new int[2];
        controlptFour = new int[2];
    }

    public Segment(int[] controlptOne, int[] controlptFour) {
        this.controlptOne = controlptOne;
        controlptTwo = new int[2];
        controlptThree = new int[2];
        this.controlptFour = controlptFour;
    }

    public void setControlptOne(int[] newPoint) {
        controlptOne = newPoint;
    }

    public void setControlptTwo(int[] newPoint) {
        controlptTwo = newPoint;
    }
    
    public void setControlptThree(int[] newPoint) {
        controlptThree = newPoint;
    }

    public void setControlptFour(int[] newPoint) {
        controlptFour = newPoint;
    }


    public float xFunctionAt(float t) {
        return pow((1.0F - t), 3)*controlptOne[0] + 3 * t * pow((1.0F - t), 2)*controlptTwo[0]
            + 3 * pow(t, 2) * (1.0F - t)*controlptThree[0] + pow(t, 3)*controlptFour[0];
    }

    public float yFunctionAt(float t) {
        return pow((1.0F - t), 3) * controlptOne[1] + 3 * t * pow((1.0F - t), 2) * controlptTwo[1]
            + 3 * pow(t, 2) * (1.0F - t) * controlptThree[1] + pow(t, 3) * controlptFour[1];
    }

    public static float pow(float num, int pow) {
        float output = num;
        for (int i = 0; i < pow-1; i++) {
            output *= num;
        }
        return output;
    }

    public float getSegmentLength() {
        float output = 0;
        for (float i = 0.0F; i < 1.0F; i+=Length_Accuracy_Const) {
            float xDif = Math.Abs(xFunctionAt(i + Length_Accuracy_Const) - xFunctionAt(i));
            float yDif = Math.Abs(yFunctionAt(i + Length_Accuracy_Const) - yFunctionAt(i));
            output += Math.Sqrt(pow(xDif, 2) + pow(yDif, 2));
        }
    }

    public int[] getControlptOne() {
        return controlptOne;
    }

    public int[] getControlptTwo() {
        return controlptTwo; 
    }

    public int[] getControlptThree() {
        return controlptThree;
    }

    public int[] getControlptFour() {
        return controlptFour;
    }
}
