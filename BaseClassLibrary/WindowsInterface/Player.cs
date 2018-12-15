﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseClassLibrary;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace WindowsInterface
{
    class Player
    {
        public String TeamNumber;
        private Robot robot;
        private List<Layer> layers;
        public Canvas MainCanvas { get; private set; }

        public Player(double width, double height, String teamNum,Robot robot)
        {
            this.robot = robot;
            TeamNumber = teamNum;
            layers = new List<Layer>();
            MainCanvas = new Canvas();
            MainCanvas.Height = height;
            MainCanvas.Width = width;
        }

        public Player(String teamNum, Robot robot, List<Layer> layers)
        {
            this.robot = robot;
            TeamNumber = teamNum;
            this.layers = layers;
            MainCanvas = new Canvas();
            MainCanvas.Height = App.FieldCanvasHeight;
            MainCanvas.Width = App.FieldCanvasWidth;
        }

        public void CompileCanvas(int selectedLayerIndex)
        {
            MainCanvas.Children.Clear();
            for (int i = 0; i < layers.Count; i++)
            {
                if (i != selectedLayerIndex)
                {
                    layers[i].CompileCanvasNoEllipse();
                    MainCanvas.Children.Add(layers[i].MainCanvas);
                }
            }
            if (selectedLayerIndex != -1)
            {
                layers[selectedLayerIndex].CompileCanvas();
                MainCanvas.Children.Add(layers[selectedLayerIndex].MainCanvas);
            }
        }
        
        
        
        public void DeleteLayer(int index)
        {
            layers.RemoveAt(index);
            CompileCanvas(-1);
        }

        public void CreateLayer()
        {
            if (layers.Count > 0)
            {
                Path p = new Path();
                p.AddPoint(layers[layers.Count-1].GetEndPoint());
                layers.Add(new Layer(new MotionProfile(p, robot), MainCanvas.Width, MainCanvas.Height));
                CompileCanvas(-1);
            }
            else
            {
                layers.Add(new Layer(new MotionProfile(new Path(), robot), MainCanvas.Width, MainCanvas.Height));
                CompileCanvas(-1);
            }
        }

        public int GetNumberOfLayers()
        {
            return layers.Count;
        }
        
        public Layer GetLayer(int num)
        {
            return layers[num];
        }

    }
}
