using System;
using System.Diagnostics;
using System.IO;
using System.Net;//new
using System.Net.Sockets;
using System.Threading;//new
using Newtonsoft.Json;
using SingleKinect.EngagementManage;
using System.ComponentModel;
using System.Windows;

namespace SingleKinect.SendData
{

    public class SendData
    {
        private static SendData sendData;

        private SendData() { }

        public static SendData Instance
        {
            get
            {
                if (sendData == null)
                {
                    sendData = new SendData();
                }
                return sendData;
            }
        }

        private TcpClient client;
        private Socket socket;
        private IPEndPoint IP_End;

        private NetworkStream nwStream;
        private StreamReader reader;
        private StreamWriter _writer;

        private bool isConnected;

        public void initialize()
        {
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName()); //get my own IP
            String myAddress;
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    myAddress = address.ToString();
                    //Console.WriteLine("Client Address is "+ myAddress );
                }
            }
            client = new TcpClient();
            //socket = client.Client;
            string server = "192.168.0.4";
            string port = "7777";
            IP_End = new IPEndPoint(IPAddress.Parse(server), int.Parse(port));
            client.Connect(IP_End);
        }

        public void connect(DataToSendnew sendingData)
        {           
            try
            {                
                if (client.Connected)
                {
                    Console.WriteLine("Connected to Server" );
                    _writer = new StreamWriter(client.GetStream());
                    reader = new StreamReader(client.GetStream());
                    _writer.AutoFlush = true;

                    //Console.WriteLine("YES IM HERE");

                    string lineToSend = getSendingJson(sendingData);
                    send(lineToSend);

                    
                }
            }
            catch (Exception x)
            {
                Debug.Print("not connected to server" + "\n");
                Debug.Print(x.Message.ToString());
                isConnected = false;
                return;
            }

            //Console.WriteLine("I am out!");//test
            isConnected = true;

        }

        public void send(string lineToSend)
        {
            //string lineToSend = getSendingJson(sendingData);
            try
            {
                /*File.WriteAllText(
                    @"C:\Users\Wei\Desktop\Interaction system for GDO\GDOKinectInteraction\SingleKinect\SendData\Output.json",
                    lineToSend);*/
                //Console.WriteLine("Json saved");//test

                _writer.WriteLine(lineToSend);
                Console.WriteLine("Me: " + lineToSend);
            }
            catch (Exception ex)
            {
                Debug.Print("Send failed" + "\n");
                Console.WriteLine(ex);
                isConnected = false;
            }

            lineToSend = "";

//            string acknowledge = reader.ReadLine();
//            while (acknowledge != null)
//            {
//                Console.WriteLine(acknowledge);
//                acknowledge = reader.ReadLine();
//            }
            //receive();

        }

        private string getSendingJson(DataToSendnew sendingData)
        {
            string json = JsonConvert.SerializeObject(sendingData);
            return json;
        }

        public void clientClose()
        {
            client.Close();
            //Console.WriteLine("Client is closed.\n");//test
        }

        //not used
        //        public void receive()
        //        {
        //            string lineReceived = reader.ReadLine();
        //            //Console.WriteLine("Received from server: " + lineReceived);
        //        }


        /* private void createJSON()
       {
           //Create Json file
           //string outputJSON = JsonConverter.SerializeObject(Datatosend);
           //File.WriteAllText("Output.json", outputJSON);

           //Parsing Json file
           //String JSONstring = File.ReadAllText("Json.json");
           //DataReceived data1=JsonConvert.DeserializeObject<DataReceived>(Datatosend);
       }*/
    }
}