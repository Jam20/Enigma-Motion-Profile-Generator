using System;
using System.Collections.Generic;
using System.Text;

namespace BaseClassLibrary
{
    public class MotionProfile{
        public Path path;
        private Robot robot;
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
            while (pos[pos.Count-1] < path.getTotalDistance()) {

                if((vel[vel.Count-1] < robot.maxVel && pos[pos.Count-1] >= .5 * path.getTotalDistance()) || (pos[pos.Count-1] >= timeToDeselerate)) {
                    pos.Add(pos[pos.Count - 1] + vel[vel.Count - 1] * robot.timeIncrementInSec - .5 * robot.maxAccel * robot.timeIncrementInSec);
                    vel.Add(vel[vel.Count - 1] - robot.maxAccel * robot.timeIncrementInSec);
                    head.Add(path.getDirectionat(pos[pos.Count - 1]));
                }
                else if(vel[vel.Count-1] < robot.maxVel) {
                    pos.Add(pos[pos.Count - 1] + vel[vel.Count - 1] * robot.timeIncrementInSec + .5 * robot.maxAccel * robot.timeIncrementInSec);
                    vel.Add(vel[vel.Count - 1] + robot.maxAccel * robot.timeIncrementInSec);
                    head.Add(path.getDirectionat(pos[pos.Count - 1]));
                }
                else {
                    pos.Add(pos[pos.Count - 1] + vel[vel.Count - 1] * robot.timeIncrementInSec + .5 * robot.maxAccel * robot.timeIncrementInSec);
                    vel.Add(robot.maxVel);
                    head.Add(path.getDirectionat(pos[pos.Count - 1]));
                }
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
            double[][] output = new double[3][];
            for(int i= 0; i< output.Length; i++) {
                output[i] = new double[position.Length];
            }

            for(int i = 0; i< position.Length; i++) {
                output[0][i] = position[i];
                output[1][i] = velocity[i];
                output[2][i] = heading[i];
            }
            return output;

        }
    }
}
