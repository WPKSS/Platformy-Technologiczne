using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Collections.Concurrent;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;


namespace lab10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int ThreadCount = 4;
        private string Path = String.Empty;
        private Task searchEngine;
        private long totalSize = 0;
        private long pathSize = 0;
        //private ConcurrentBag<long> totalSize = new ConcurrentBag<long>();
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token;

        private bool taskWorking = false;
        private string logBook = "";
        BlockingCollection<string> blockingQueue;

        public MainWindow()
        {
            InitializeComponent();
            blockingQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
            ThreadCountText.Text = ThreadCount.ToString();
        }

        private void Change_Threads_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(ThreadBox.Text))
            {

                var value = ThreadBox.Text;

                ThreadCount = Convert.ToInt32(value);
                ThreadCountText.Text = ThreadCount.ToString();

            }
        }

        private void Change_Path_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new OpenFileDialog();
            folderDialog.ValidateNames = false;
            folderDialog.CheckFileExists = false;
            folderDialog.CheckPathExists = true;
            folderDialog.FileName = "Choose folder";

            if (folderDialog.ShowDialog() == true)
            {
                Path = System.IO.Path.GetDirectoryName(folderDialog.FileName);
                PathText.Text = Path;
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }
        private async void Begin_Search_Click(object sender, RoutedEventArgs e)
        {
            token = cts.Token;
            if (string.IsNullOrEmpty(Path))
            {
                MessageBox.Show("Wybierz ścieżkę.");
                return;
            }
            if(taskWorking)
            {
                return;
            }
            blockingQueue.Add(Path);
            FileInfo info = new FileInfo(Path);
            List<Task> tasks = new List<Task>();
            var allFiles = Directory.EnumerateFiles(Path, "*", SearchOption.AllDirectories).ToList();
            IProgress<int> progress = new Progress<int>(value =>
            {
                progressBar.Value = value;
            });
            taskWorking = true;
            for(int i = 0; i < ThreadCount; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Anulowano wyszukiwanie.");
                        return;
                    }

                    if (blockingQueue.TryTake(out string dir, TimeSpan.FromSeconds(5)))
                    {
                        string[] subDirs = Directory.GetDirectories(dir);
                        foreach (string subDir in subDirs)
                        {
                            blockingQueue.Add(subDir);
                        }

                        var time = Stopwatch.StartNew();
                        long dirSize = calculateDirSize(dir);
                        time.Stop();

                        Interlocked.Add(ref totalSize, dirSize);

                        string text = $"Folder: {dir}, rozmiar: {dirSize}, czas: {time.ElapsedMilliseconds} ms \n";
                            int percent = (int)((pathSize / (double)allFiles.Count) * 100);
                            progress.Report(percent);
                            logBook += text;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LogTextBox.AppendText(text);
                            LogTextBox.ScrollToEnd();
                        });
                        }
                        else
                        {
                            break;
                        }
                    }
                }));
            }
            await Task.WhenAll(tasks);
            MessageBox.Show($"Koniec Task. Rozmiar: {totalSize}B");
            

            blockingQueue.Add(Path);
            LogTextBox.Clear();
            totalSize = 0; pathSize = 0;
            AsyncOperation searcher = searchAsync;
            for (int i = 0; i < ThreadCount; i++)
            {
                tasks.Add(Task.Run(() => searcher(token, progress)));
            }
            await Task.WhenAll(tasks);
            MessageBox.Show($"Koniec delegatow. Rozmiar: {totalSize}B");


            blockingQueue.Add(Path);
            LogTextBox.Clear();
            totalSize = 0; pathSize = 0;
            for (int i = 0; i < ThreadCount; i++)
            {
                tasks.Add(searchAsync(token, progress));
            }
            await Task.WhenAll(tasks);
            MessageBox.Show($"Koniec async-await. Rozmiar: {totalSize}B");

            blockingQueue.Add(Path);
            LogTextBox.Clear();
            totalSize = 0; pathSize = 0;
            for (int i = 0; i < ThreadCount; i++)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, ex) =>
                {
                    searchAsync(token, progress).Wait(); 
                };

                worker.RunWorkerAsync();
            }
            await Task.WhenAll(tasks);
            MessageBox.Show($"Koniec workera. Rozmiar: {totalSize}B");
            taskWorking = false;
        }

        delegate Task<int> AsyncOperation(CancellationToken token, IProgress<int> progress);



        async Task<int> searchAsync(CancellationToken token, IProgress<int> progress)
        {
            var allFiles = Directory.EnumerateFiles(Path, "*", SearchOption.AllDirectories).ToList();
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Anulowano wyszukiwanie.");
                    return 0; // lub inny sposób na zakończenie
                }

                if (blockingQueue.TryTake(out string dir, TimeSpan.FromSeconds(5)))
                {
                    string[] subDirs = Directory.GetDirectories(dir);
                    foreach (string subDir in subDirs)
                    {
                        blockingQueue.Add(subDir);
                    }

                    var time = Stopwatch.StartNew();
                    long dirSize = calculateDirSize(dir);
                    time.Stop();

                    Interlocked.Add(ref totalSize, dirSize);
                    

                    string text = $"Folder: {dir}, rozmiar: {dirSize}, czas: {time.ElapsedMilliseconds} ms \n";
                    int percent = (int)((pathSize / (double)allFiles.Count) * 100);
                    progress.Report(percent);
                    logBook += text;
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        LogTextBox.AppendText(text);
                        LogTextBox.ScrollToEnd();
                    });
                }
                else
                {
                    break;
                }
            }
            return 0;
        }

        private long calculateDirSize(string path)
        {
            long size = 0;
            string[] allFiles = Directory.GetFiles(path);
            foreach (string file in allFiles)
            {
                if (File.Exists(file))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    size += fileInfo.Length;
                    pathSize++;
                }
            }
            return size;
        }
    }
}
