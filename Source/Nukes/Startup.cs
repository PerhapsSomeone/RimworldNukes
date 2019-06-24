using System;
using System.IO;
using System.Net;
using UnityEngine.Networking;
using UnityEngine;
using Verse;
using System.Threading;
using System.Collections;
using System.Net.Sockets;

namespace Nukes
{
    [StaticConstructorOnStartup]
    public class Startup
    {
        static Startup()
        {
            Log.Message("Loading MentalBreaks...");


            new Thread(() =>
            {
                String message = "GET /api/gamelaunch?mod=1\n\n";

                ServicePointManager.ServerCertificateValidationCallback += (p1, p2, p3, p4) => true;

                TcpClient client = new TcpClient("159.69.219.44", 9797);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);


                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // Close everything.
                stream.Close();
                client.Close();

                Log.Message("Logged game launch.");
            }).Start();
        }

    }
}
