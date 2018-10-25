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
        private Robot robot;
        private List<Layer> layers;
        public Canvas MainCanvas { get; private set; }

        public Player()
        {
            layers = new List<Layer>();
            MainCanvas = new Canvas();
        }

        private void CompileCanvas()
        {
            
        }

        public void CreateLayer()
        {
            layers.Add(new Layer(new MotionProfile(new Path(),robot)));
        }
    }
}
