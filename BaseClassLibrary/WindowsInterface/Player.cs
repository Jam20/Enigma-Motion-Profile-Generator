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
        public int SelectedLayer;
        public Canvas MainCanvas { get; private set; }

        public Player(double width, double height, String teamNum)
        {
            TeamNumber = teamNum;
            layers = new List<Layer>();
            MainCanvas = new Canvas();
            MainCanvas.Height = height;
            MainCanvas.Width = width;
        }

        public void CompileCanvas()
        {
            Canvas output = new Canvas();
            output.Height = MainCanvas.Height;
            output.Width = MainCanvas.Width;
            for (int i = 0; i < layers.Count; i++)
            {
                if (i != SelectedLayer)
                {
                    MainCanvas.Children.Add(layers[i].MainCanvas);
                }
                else
                {
                    Canvas.SetZIndex(layers[i].MainCanvas, 1000);
                    output.Children.Add(layers[i].MainCanvas);
                }
            }
        }

        public void CreateLayer()
        {
            layers.Add(new Layer(new MotionProfile(new Path(),robot), MainCanvas.Width, MainCanvas.Height));
            CompileCanvas();
        }

    }
}
