using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//new added
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Microsoft.Kinect;


namespace ServerGDO
{
    class TCPserver
    {
        static void Main(string[] args)
        {
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName()); //get my own IP
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    string myAddr = address.ToString();
                    Console.WriteLine(myAddr);//test
                }
            }

            //public Dictionary<ulong, Human> users = new Dictionary<ulong, Human>();
            TcpListener serverSocket = new TcpListener(7777);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            counter = 0;
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine("/n>> " + "Client No:" + Convert.ToString(counter) + " started!");
                handleClinet client = new handleClinet();
                client.startClient(clientSocket, Convert.ToString(counter));
            }

            clientSocket.Close();
            serverSocket.Stop();
            Console.WriteLine(" >> " + "exit");
            Console.ReadLine();
        }
    }

    //Class to handle each client request separatly
    public class handleClinet
    {
        TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        string clNo;
        public void startClient(TcpClient inClientSocket, string clientNo)
        {
            this.client = inClientSocket;
            this.clNo = clientNo;
            Thread skeletonThread = new Thread(getData);
            skeletonThread.Start();
        }

        private void getData()
        {
            int requestCount = 0;
            string dataFromClient = null;

            if (!client.Connected)
            {
                return;
            }

            STR = new StreamReader(client.GetStream());
            //STW = new StreamWriter(client.GetStream());
            //STW.AutoFlush = true;

            dataFromClient = STR.ReadLine();
            while (client.Connected && dataFromClient != null)
            {
                try
                {
                    requestCount = requestCount + 1;

                    if (dataFromClient == null)
                    {
                        requestCount = requestCount - 1;
                        dataFromClient = STR.ReadLine();
                        continue;
                    }

                    Console.WriteLine(dataFromClient);

                    Human user = new Human();
                    //var json = JsonConvert.DeserializeObject(receive);
                    JsonConvert.PopulateObject(dataFromClient, user);

                    Console.WriteLine(" >> " + "From client-" + clNo + "'s No." + requestCount + " requests :" +
                                      user.RightHandJoint.X );
                    dataFromClient = "";
                    dataFromClient = STR.ReadLine(); //need to ignore none and add multiple threading(later)
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                }

            }

        }

        /*private void temp()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[10025];
            string dataFromClient = null;

            //Byte[] sendBytes = null;
            //string serverResponse = null;
            //string rCount = null;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;

                    STR = new StreamReader(client.GetStream());
                    STW = new StreamWriter(client.GetStream());
                    STW.AutoFlush = true;

                    NetworkStream networkStream = client.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)client.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine(" >> " + "From client-" + clNo + dataFromClient);

                    //                    rCount = Convert.ToString(requestCount);
                    //                    serverResponse = "Server to clinet(" + clNo + ") " + rCount;
                    //                    sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    //                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    //                    networkStream.Flush();                
                    //                    Console.WriteLine(" >> " + serverResponse);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }*/ //no use
    }


}
