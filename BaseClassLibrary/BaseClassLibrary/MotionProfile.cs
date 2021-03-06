﻿using System;
using System.Collections.Generic;

namespace BaseClassLibrary
{
    public class MotionProfile
    {
        public Path Path { get; private set; }
        public Robot Robot { get; private set; }
        private double[] position, velocity, heading;
        public double ProfileTime;
        MotionProfile previousProfile;
        //Motion profile Constructor creates a profile based on a path object
        public MotionProfile(Path p,Robot robot = null, MotionProfile previousProfile = null) 
        {
            this.previousProfile = previousProfile;
            Path = p;
            if (robot == null)
            {
                Robot = new Robot(previousProfile.Robot.ToString());
            }
            else Robot = robot;
            CalcProfile();
        }

        public void CalcProfile()
        {
            if (Path.PathList.Count == 0) return;
            List<double> pos, vel, head;
            pos = new List<double>();
            vel = new List<double>();
            head = new List<double>();
            int timeInMs = 0;
            pos.Add(0);
            vel.Add(0);
            if (previousProfile != null) head.Add(-previousProfile.heading[previousProfile.heading.Length-1]);
            else head.Add(Path.GetDirectionat(0));
            Path.SetTotalDistance();
            while (Math.Abs(Path.TotalDistance - pos[pos.Count - 1]) > 0.01)
            {
                double currentPosition = pos[pos.Count - 1];
                double currentVelocity = vel[vel.Count - 1];


                double timeToDeccel = currentVelocity / Robot.MaxAccel;
                double distToDeccel = currentVelocity * timeToDeccel - .5 * Robot.MaxAccel * Math.Pow(timeToDeccel, 2);

                if (currentVelocity < Robot.MaxVel)
                {
                    
                }

                if (Path.TotalDistance - (currentPosition + distToDeccel) > .01)
                {
                    double distTraveledThisLoop;
                    double velThisLoop;
                    if (currentVelocity < Robot.MaxVel)
                    {
                        double newVel = currentVelocity + Robot.MaxAccel * Robot.TimeIncrementInSec;
                        
                        if (newVel > Robot.MaxVel)
                        {
                            double timeToMaxVel = (Robot.MaxVel - currentVelocity) / Robot.MaxAccel;
                            double pos1 = currentVelocity * (Robot.TimeIncrementInSec - timeToMaxVel) + .5 * Robot.MaxAccel * Math.Pow(Robot.TimeIncrementInSec - timeToMaxVel, 2);
                            distTraveledThisLoop = pos1 + Robot.MaxVel * timeToMaxVel + .5 * Robot.MaxAccel * Math.Pow(timeToMaxVel, 2);
                            velThisLoop = Robot.MaxVel;
                        }
                        else
                        {
                            distTraveledThisLoop =currentVelocity * Robot.TimeIncrementInSec + .5 * Robot.MaxAccel * Math.Pow(Robot.TimeIncrementInSec, 2);
                            velThisLoop = newVel;
                        }
                        
                    }
                    else
                    {
                        distTraveledThisLoop = Robot.MaxVel * Robot.TimeIncrementInSec;
                        velThisLoop = Robot.MaxVel;
                    }
                    if (Path.TotalDistance - (currentPosition + distTraveledThisLoop) > distToDeccel)
                    {
                        pos.Add(currentPosition +distTraveledThisLoop);
                        vel.Add(velThisLoop);
                    }
                    else
                    {
                        double distTillDecel = (Path.TotalDistance - currentPosition) - distToDeccel;
                        double timeTillDecel = distTillDecel / currentVelocity;
                        double timeDecel = Robot.TimeIncrementInSec - timeTillDecel;
                        pos.Add((Path.TotalDistance - distToDeccel) + timeDecel * currentVelocity - .5 * Robot.MaxAccel * Math.Pow(timeDecel, 2));
                        vel.Add(currentVelocity - Robot.MaxAccel * timeDecel);
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
                double possibleHeading = Path.GetDirectionat(pos[pos.Count - 1]);
                if (possibleHeading <0 && head[head.Count-1] >90)
                {
                    possibleHeading = (possibleHeading + 360) % 360;
                }
                else if(possibleHeading>0 && head[head.Count - 1] < -90)
                {
                    possibleHeading = (possibleHeading - 360) % 360;
                }
                head.Add(possibleHeading);
                //head.Add(Path.GetDirectionat(pos[pos.Count - 1]));
                timeInMs += (int)(Robot.TimeIncrementInSec * 1000);
            }
            ProfileTime = timeInMs / 1000.0;
            position = new double[pos.Count];
            velocity = new double[vel.Count];
            heading = new double[head.Count];

            if (Path.IsReversed)
            {
                for (int i = 0; i < position.Length; i++)
                {
                    position[i] = -pos[i];
                    velocity[i] = -vel[i];
                    heading[i] = -180 + -head[i];
                }
            }
            else
            {
                for (int i = 0; i < position.Length; i++)
                {
                    position[i] = pos[i];
                    velocity[i] = vel[i];
                    heading[i] = -head[i];
                }
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
