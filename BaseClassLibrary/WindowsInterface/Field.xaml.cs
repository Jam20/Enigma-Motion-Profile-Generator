using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsInterface
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Field : Page
    {
        public int FieldCanvasHeight { get; private set; }
        public int FieldCanvasWidth { get; private set; }

        public Field()
        {
            this.InitializeComponent();
        }
    }
}
