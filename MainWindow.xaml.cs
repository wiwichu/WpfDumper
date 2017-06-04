using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static System.Threading.ThreadPool;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using WpfDumper.ViewModel;
using System.Collections.Concurrent;
using WpfDumper.View;
using System.Text.RegularExpressions;

namespace WpfDumper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        private async void btnException_Click(object sender, RoutedEventArgs e)
        {
            Task t1 = Task.Run(() =>
            {
                throw new Exception("Crash App Task!");
            });

            t1.Wait();
        }

        private void btnPoolException_Click(object sender, RoutedEventArgs e)
        {
            QueueUserWorkItem(new System.Threading.WaitCallback((a) =>
            {
                throw new Exception("Crash App Pool!");
            }));

        }

        private void btnRx_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}");
            var query = from number in Enumerable.Range(1, 10)
                        select QueryNumber(number);
            var observableQuery = 
                query.ToObservable();
            observableQuery
                .ObserveOn(NewThreadScheduler.Default)
                .SubscribeOn(NewThreadScheduler.Default)
                .Subscribe(ProcessNumber, ImDone);
            //var scheduler = TestScheduler
        }
        static int QueryNumber(int number)
        {
            Debug.WriteLine($"{number} Query Thread {Thread.CurrentThread.ManagedThreadId}");
            return number;
        }
        static void ProcessNumber(int number)
        {
            Debug.WriteLine($"{number} Process Thread {Thread.CurrentThread.ManagedThreadId}");
        }
        static void ImDone()
        {
            Debug.WriteLine("I'm done!");
        }

        private void btnValidation_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnEventHang_Click(object sender, RoutedEventArgs e)
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            QueueUserWorkItem(new WaitCallback((a) =>
            {
                mre.WaitOne(60000);
            }));
            mre.WaitOne(60000);
        }
        static object locker = new object();

        private void btnLockHang_Click(object sender, RoutedEventArgs e)
        {
            //object locker = new object();
            QueueUserWorkItem(new WaitCallback((a) =>
            {
                lock (locker)
                {
                    Task.Delay(60000).Wait();
                }
            }));
            Task.Delay(100).Wait();
            lock (locker) { }
        }
        private ConcurrentBag<string> sBag = new ConcurrentBag<string>();
        private static List<string> sList = new List<string>(); 
        private void btnLeak_Click(object sender, RoutedEventArgs e)
        {
            string leakString = "BIG";
            Thread t = new Thread(() =>
            {

                while (true)
                {
                    Task.Delay(10).Wait();
                    {
                        lock (sList)
                        {
                            sBag.Add(DateTime.Now.ToString());
                        }
                        //leakString += leakString;
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        private void btnStackOverflow_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
            OverflowStack(11.11);
        },100000);
            t.IsBackground = true;
            
            t.Start();
            //try
            //{
            //    Task.Run(() => OverflowStack(11.11));
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        private void OverflowStack(double dbl)
        {
            double d = dbl;
            //Task.Delay(1).Wait();
            OverflowStack(dbl);
        }

        private void btnHighCpu_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => {
                while(true)
                {

                }
            });
        }

        private void btnWeakEventTableLeak_Click(object sender, RoutedEventArgs e)
        {
            var view = new WeakEventTableLeakView();
            view.Show();
        }

        private void btnGCCollect_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }

        private void Synchronous_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "Calculating Sync.";
            Thread.Sleep(20000);
            textBlock.Text = "Ready";
        }

        private void Async_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "Calculating Async.";
            Task.Run(() => {
            Thread.Sleep(20000);
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background,
                    new Action(()=>textBlock.Text="Ready"));
            });
        }
    }
    class MyObserver : IObserver<int>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(int value)
        {
            throw new NotImplementedException();
        }
    }
}
