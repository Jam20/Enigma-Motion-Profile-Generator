using System;
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

        public void CompileCanvas()
        {
            MainCanvas.Children.Clear();
            for (int i = 0; i < layers.Count; i++)
            {
                Canvas testcanvas = layers[i].MainCanvas;
                MainCanvas.Children.Add(layers[i].MainCanvas);
            }
        }

        public void DeleteLayer(int index)
        {
            layers.RemoveAt(index);
            CompileCanvas();
        }

        public void CreateLayer()
        {
            layers.Add(new Layer(new MotionProfile(new Path(),robot), MainCanvas.Width, MainCanvas.Height));
            CompileCanvas();
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
