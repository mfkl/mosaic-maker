using LibVLCSharp.Shared;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var filename = "video1.mp4";
            var filename2 = "video2.mp4";

            Core.Initialize();

            var libvlc = new LibVLC("--verbose=2");

            // mosaic 1

            var media = new Media(libvlc, filename);
            media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=1,height=200,width=300}}");
            //media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=1,height=200,width=300},dst=display}");
            var mp1 = new MediaPlayer(media);
            media.Dispose();

            // mosaic 2

            media = new Media(libvlc, filename2);
            media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=2,height=200,width=300}}");
            //media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=2,height=200,width=300},dst=display}");
            var mp2 = new MediaPlayer(media);
            media.Dispose();

            // mosaic 3

            media = new Media(libvlc, filename);
            media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=3,height=200,width=300}}");
            //media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=3,height=200,width=300},dst=display}");
            var mp3 = new MediaPlayer(media);
            media.Dispose();

            // mosaic 4

            media = new Media(libvlc, filename2);
            media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=4,height=200,width=300}}");
            //media.AddOption(":sout=#duplicate{dst=mosaic-bridge{id=4,height=200,width=300},dst=display}");
            var mp4 = new MediaPlayer(media);
            media.Dispose();

            // mosaic out

            media = new Media(libvlc, "cone.png");
            //var soutOption = ":sout=#transcode{sfilter=mosaic{keep-picture,width=600,height=400,cols=2,rows=2},vcodec=mp4v,vb=20000,acodec=none,fps=10,scale=1}:file{dst=lol.ts,mux=ts}";
            var soutOption = ":sout=#transcode{sfilter=mosaic{keep-picture,width=600,height=400,cols=2,rows=2},vcodec=mp4v,vb=20000,acodec=none,fps=10,scale=1}:duplicate{dst=file{dst=lol.ts,mux=ts},dst=display}";
            media.AddOption(soutOption);
            media.AddOption(":image-duration=-1");

            var mpOut = new MediaPlayer(media);
            media.Dispose();

            mpOut.Play();

            mp1.Play();
            mp2.Play();
            mp3.Play();
            mp4.Play();

            await Task.Delay(10000);

            mp1.Stop();
            mp2.Stop();
            mp3.Stop();
            mp4.Stop();
            mpOut.Stop();

            mp1.Dispose();
            mp2.Dispose();
            mp3.Dispose();
            mp4.Dispose();
            mpOut.Dispose();

            libvlc.Dispose();
        }
    }
}