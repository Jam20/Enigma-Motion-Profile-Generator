using System;
using System.Collections.Generic;

namespace BaseClassLibrary
{
    public class MotionProfile
    {
        public Path Path;
        public Robot Robot;
        private double[] position, velocity, heading;
        public double ProfileTime;

        //Motion profile Constructor creates a profile based on a path object
        public MotionProfile(Path p, Robot r)
        {
            Path = p;
            Robot = r;
            List<double> pos, vel, head;
            pos = new List<double>();
            vel = new List<double>();
            head = new List<double>();
            int timeInMs = 0;
            pos.Add(0);
            vel.Add(0);
            head.Add(Path.GetDirectionat(0));
            while (Math.Abs(Path.TotalDistance - pos[pos.Count - 1]) > .01)
            {
                double currentPosition = pos[pos.Count - 1];
                double currentVelocity = vel[vel.Count - 1];


                double timeToDeccel = currentVelocity / Robot.MaxAccel;
                double distToDeccel = currentVelocity * timeToDeccel - .5 * Robot.MaxAccel * Math.Pow(timeToDeccel, 2);

                if (currentPosition < Path.TotalDistance - distToDeccel)
                {
                    if (currentVelocity < Robot.MaxVel)
                    {
                        double newVel = currentVelocity + Robot.MaxAccel * Robot.TimeIncrementInSec;
                        if (newVel > Robot.MaxVel)
                        {
                            double timeToMaxVel = (Robot.MaxVel - currentVelocity) / Robot.MaxAccel;
                            double pos1 = currentPosition + currentVelocity * (Robot.TimeIncrementInSec - timeToMaxVel) + .5 * Robot.MaxAccel * Math.Pow(Robot.TimeIncrementInSec - timeToMaxVel, 2);
                            pos.Add(pos1 + Robot.MaxVel * timeToMaxVel + .5 * Robot.MaxAccel * Math.Pow(timeToMaxVel, 2));
                            vel.Add(Robot.MaxVel);
                        }
                        else
                        {
                            pos.Add(currentPosition + currentVelocity * Robot.TimeIncrementInSec + .5 * Robot.MaxAccel * Math.Pow(Robot.TimeIncrementInSec, 2));
                            vel.Add(newVel);
                        }
                    }
                    else
                    {
                        pos.Add(currentPosition + Robot.MaxVel * Robot.TimeIncrementInSec);
                        vel.Add(Robot.MaxVel);
                    }
                }
                else
                {
                    if (currentPosition + currentVelocity * Robot.TimeIncrementInSec - .5 * Robot.MaxAccel * Math.Pow(Robot.TimeIncrementInSec, 2) > Path.TotalDistance)
                    {
                        pos.Add(Path.TotalDistance);
                        vel.Add(0);
                    }
                    else
                    {
                        pos.Add(currentPosition + currentVelocity * Robot.TimeIncrementInSec - .5 * Robot.MaxAccel * Math.Pow(Robot.TimeIncrementInSec, 2));
                        vel.Add(currentVelocity - Robot.MaxAccel * Robot.TimeIncrementInSec);
                    }
                }
                head.Add(Path.GetDirectionat(pos[pos.Count - 1]));
                timeInMs += (int)(Robot.TimeIncrementInSec * 1000);
            }
            ProfileTime = timeInMs / 1000;
            position = new double[pos.Count];
            velocity = new double[vel.Count];
            heading = new double[head.Count];

            for (int i = 0; i < position.Length; i++)
            {
                position[i] = pos[i];
                velocity[i] = vel[i];
                heading[i] = head[i];
            }
        }

        //outputs the profile to an array to be outputted into a save file
        public double[][] ToArray()
        {
            double[][] output = new double[position.Length][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = new double[] { position[i], velocity[i], heading[i] };
            }

            return output;

        }
    }
}
