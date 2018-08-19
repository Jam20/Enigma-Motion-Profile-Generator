using System;
using System.Collections.Generic;
using System.Text;

namespace BaseClassLibrary
{
    public class MotionProfile{
        public Path path;
        public Robot robot;
        private double[] position, velocity, heading;
        public double profileTime;

        public MotionProfile(Path p, Robot r) {
            path = p;
            robot = r;
            List<double> pos, vel, head;
            pos = new List<double>();
            vel = new List<double>();
            head = new List<double>();
            int timeInMs = 0;
            double timeToDeselerate = path.getTotalDistance() - robot.maxVel * (robot.maxVel / robot.maxAccel) + robot.maxAccel * (Math.Pow(robot.maxVel, 2) / (2 * Math.Pow(robot.maxAccel, 2)));
            pos.Add(0);
            vel.Add(0);
            head.Add(path.getDirectionat(0));
            while (Math.Abs(path.getTotalDistance()-pos[pos.Count-1])>.01) {
                double currentPosition = pos[pos.Count - 1];
                double currentVelocity = vel[vel.Count - 1];


                double timeToDeccel = currentVelocity / robot.maxAccel;
                double distToDeccel =currentVelocity * timeToDeccel - .5 * robot.maxAccel * Math.Pow(timeToDeccel, 2);

                if (currentPosition < path.getTotalDistance() - distToDeccel) {
                    if (currentVelocity < robot.maxVel) {
                        double newVel = currentVelocity + robot.maxAccel * robot.timeIncrementInSec;
                        if (newVel > robot.maxVel) {
                            double timeToMaxVel = (robot.maxVel - currentVelocity) / robot.maxAccel;
                            double pos1 = currentPosition + currentVelocity * (robot.timeIncrementInSec - timeToMaxVel) + .5 * robot.maxAccel * Math.Pow(robot.timeIncrementInSec - timeToMaxVel, 2);
                            pos.Add(pos1+robot.maxVel*timeToMaxVel+.5*robot.maxAccel*Math.Pow(timeToMaxVel,2));
                            vel.Add(robot.maxVel);
                        }
                        else {
                            pos.Add(currentPosition + currentVelocity * robot.timeIncrementInSec + .5 * robot.maxAccel * Math.Pow(robot.timeIncrementInSec, 2));
                            vel.Add(newVel);
                        }
                    }
                    else {
                        pos.Add(currentPosition + robot.maxVel * robot.timeIncrementInSec);
                        vel.Add(robot.maxVel);
                    }
                }
                else {
                    pos.Add(currentPosition + currentVelocity * robot.timeIncrementInSec - .5 * robot.maxAccel * Math.Pow(robot.timeIncrementInSec, 2));
                    vel.Add(currentVelocity - robot.maxAccel * robot.timeIncrementInSec);
                }
                head.Add(path.getDirectionat(pos[pos.Count - 1]));
                timeInMs += (int)(robot.timeIncrementInSec * 1000);
            }
            profileTime = timeInMs / 1000;
            position = new double[pos.Count];
            velocity = new double[vel.Count];
            heading = new double[head.Count];
            
            for(int i =0; i< position.Length; i++) {
                position[i] = pos[i];
                velocity[i] = vel[i];
                heading[i] = head[i];
            }
        }

        public double[][] toArray() {
            double[][] output = new double[position.Length][];
            for(int i = 0; i < output.Length; i++) {
                output[i] = new double[] { position[i], velocity[i], heading[i] };
            }
            
            return output;

        }
    }
}
