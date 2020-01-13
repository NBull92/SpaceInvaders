using System.Windows;

namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game _model;

        public MainWindow()
        {
            InitializeComponent();
            _model = new Game();
            DataContext = _model;
            Content = _model.Canvas;
        }
    }
}
