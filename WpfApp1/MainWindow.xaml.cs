using LibVLCSharp.Shared;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        LibVLC libVLC;
        MediaPlayer mp0;
        MediaPlayer mp1;
        MediaPlayer mp2;
        MediaPlayer mp3;
        MediaPlayer mpOut;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {           
            var filename = "video1.mp4";
            var filename2 = "video2.mp4";

            Core.Initialize();

            libVLC = new LibVLC("--verbose=2");

            // mosaic 1

            var media = new Media(libVLC, filename);
            media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=1,height=200,width=300}}");

            mp0 = new MediaPlayer(media);
            media.Dispose();

            // mosaic 2

            media = new Media(libVLC, filename2);
            media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=2,height=200,width=300}}");
            mp1 = new MediaPlayer(media);
            media.Dispose();

            // mosaic 3

            media = new Media(libVLC, filename);
            media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=3,height=200,width=300}}");
            mp2 = new MediaPlayer(media);
            media.Dispose();

            // mosaic 4

            media = new Media(libVLC, filename2);
            media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=4,height=200,width=300}}");
            mp3 = new MediaPlayer(media);
            media.Dispose();

            // mosaic out

            media = new Media(libVLC, "cone.png");
            var soutOption = ":sout=#transcode{sfilter=mosaic{keep-picture,width=600,height=400,cols=2,rows=2},vcodec=mp4v,vb=20000,acodec=none,fps=10,scale=1}:duplicate{dst=file{dst=lol.ts,mux=ts},dst=display}";
            media.AddOption(soutOption);
            media.AddOption(":image-duration=-1");

            mpOut = new MediaPlayer(media);
            VideoView0.MediaPlayer = mpOut;
            media.Dispose();

            mpOut.Play();

            mp0.Play();
            mp1.Play();
            mp2.Play();
            mp3.Play();

            Wait(5);

            mp0.Stop();
            mp1.Stop();
            mp2.Stop();
            mp3.Stop();
            mpOut.Stop();

            mp0.Dispose();
            mp1.Dispose();
            mp2.Dispose();
            mp3.Dispose();
            mpOut.Dispose();

            libVLC.Dispose();
        }

        void Wait(double seconds)
        {
            var frame = new DispatcherFrame();
            new Thread(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(seconds));
                frame.Continue = false;
            }).Start();
            Dispatcher.PushFrame(frame);
        }
    }
}