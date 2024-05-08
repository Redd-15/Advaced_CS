using System.Threading;
using System.Windows;
using System;

namespace Telemetry_app
{

    public partial class MainWindow : Window
    {
        private readonly Thread PHY_Thread, Network_Thread;
        private readonly TelemetryModel _telemetryModel = new ();
        private bool terminate = false;

        private readonly DateTime decadeBegin = new(2020, 1, 1);
        private long time;
        public MainWindow()
        {
            Network_Thread = new Thread(new ThreadStart(NetworkConnectionHandler));
            Network_Thread.Start();

            PHY_Thread = new Thread(new ThreadStart(PHY_Calc));

            InitializeComponent();
            gMain.DataContext = this;
        }

        public TelemetryModel Model
        {
            get { return _telemetryModel; }
        }

        private void NetworkConnectionHandler()
        {
            Connection? firebaseConnection = new();

            while (firebaseConnection != null & !terminate)
            {
                try
                {
                    _telemetryModel.SetData(firebaseConnection.GetRawData());
                }
                catch (Exception)
                {
                    firebaseConnection = null;
                    terminate = true;
                }

                Thread.Sleep(200);
            }
        }

        private void PHY_Calc()
        {
            while (!terminate)
            {
                _telemetryModel.CalculatePhysics(DateTime.Now.Ticks - decadeBegin.Ticks - time);
                time = DateTime.Now.Ticks - decadeBegin.Ticks;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            terminate = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Main_Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(System.Windows.Input.Key.Escape))
            {
                this.Close();
            }
            else if (e.Key.Equals(System.Windows.Input.Key.M)) 
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        private void Main_Window_Initialized(object sender, EventArgs e)
        {
            time = DateTime.Now.Ticks - decadeBegin.Ticks;
            PHY_Thread.Start();
        }

        private void Modif_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
