using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Verse;

namespace Nukes
{
    [StaticConstructorOnStartup]
    public class Startup
    {
        static Startup()
        {
            Log.Message("Loading Nukes...");


            new Thread(() =>
            {
                // This has to be some janky ass TCP socket because C# and the whole .NET Framework is a garbage language
                // and I don't feel like reading through 500 pages of MDN just to do this shit properly

                String message = "GET /api/gamelaunch?mod=1 HTTP/1.1\nHost: rimworld.privateger.me\n\n";

                //TcpClient client = new TcpClient("159.69.219.44", 80);
                TcpClient client = new TcpClient("rimworld.privateger.me", 80);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                // Empty response, no need to read.

                // Close everything.
                stream.Close();
                client.Close();

                Log.Message("Logged game launch.");
            }).Start();
        }

    }
}
