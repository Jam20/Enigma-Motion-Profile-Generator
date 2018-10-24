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
        private List<MotionProfile> layers;
        public Canvas MainCanvas { get; private set; }

        public Player()
        {
            layers = new List<MotionProfile>();
            MainCanvas = new Canvas();
        }

        private void CompileCanvas()
        {
            foreach(MotionProfile layer in layers)
            {

            }
        }
    }
}
