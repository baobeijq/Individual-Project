using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using JetBlack.Caching.Collections.Generic;//Enumerable Circular array lib



//using Microsoft.Kinect;

namespace WebApi.Models
{
    public class HeadRepository: IHeadRepository
    {
        private List<Head> HeadPos = new List<Head>();
        public Dictionary<ulong, Head> users = new Dictionary<ulong, Head>();
        private int _nextId = 1;

        int frameNo = 0;

        public int FrameNo
        {
            get { return frameNo; }
            set { frameNo = value; }
        }

        private DateTime Latestframe;
        
        DateTime start;
        //private TimeSpan oneFrame = new TimeSpan(0, 0, 0, 0, 30);

        //Each of the following circular array will save 3-5 seconds' frame
        //5 frames are sent per second from the client, so 25 frames will be the max
        CircularBuffer<Head> KinectB_buffer = new CircularBuffer<Head>(25);//was 25
        CircularBuffer<Head> KinectC_buffer = new CircularBuffer<Head>(25);
        CircularBuffer<Head> KinectD_buffer = new CircularBuffer<Head>(25);
        CircularBuffer<Head> KinectE_buffer = new CircularBuffer<Head>(25);

        //Buffer for selecting head data which were created during a specific timespan
        CircularBuffer<Head> headSelectedByTimespan = new CircularBuffer<Head>(25);

        /*        CircularArray<Head> KinectB_buffer = new CircularArray<Head>(25);
                CircularArray<Head> KinectC_buffer = new CircularArray<Head>(25);
                CircularArray<Head> KinectD_buffer = new CircularArray<Head>(25);
                CircularArray<Head> KinectE_buffer = new CircularArray<Head>(25);*/

        /*        CameraSpacePoint pos = new CameraSpacePoint()
                {
                    X = 77.0000f,
                    Y = 77.0000f,
                    Z = 77.0000f
                };*/


        //public CameraSpacePoint headPosition;
        public HeadRepository()
        {
/*            Add(new Head { bodyIndex = 1, kinectNo ="M" , headX = 0.0007f,headY= 0.0007f ,headZ = 0.0007f,createdTime = DateTime.UtcNow });
            Add(new Head { bodyIndex = 1, kinectNo = "M", headX = 0.0008f,headY = 0.0007f, headZ = 0.0007f,createdTime = DateTime.UtcNow.AddMilliseconds(30) });
            Add(new Head { bodyIndex = 1, kinectNo = "No", headX =0.0009f, headY = 0.0007f, headZ = 0.0007f, createdTime = DateTime.UtcNow.AddMilliseconds(900) });*/
        }

        public IEnumerable<Head> GetAll()
        {
            //return HeadPos;
            return KinectB_buffer;          
        }

        public IEnumerable<Head> GetSelectedHeads(int id)//typeHead
        {            
            return headSelectedByTimespan;
            //return HeadPos.Find(p => p.Id == id);
        }

        public Head Add(Head oneHead)
        {
            if (oneHead == null)
            {
                throw new ArgumentNullException("oneHead");
            }
            oneHead.Id = _nextId++;

            switch (oneHead.kinectNo)
            {
                case "M":
                    KinectB_buffer.Enqueue(oneHead);
                    break;
                case "C":
                    KinectC_buffer.Enqueue(oneHead);
                    break;
                case "D":
                    KinectD_buffer.Enqueue(oneHead);
                    break;
                case "E":
                    KinectE_buffer.Enqueue(oneHead);
                    break;
                default:
                    //Console.WriteLine("Wrong KinectNo");
                    break;
            }
            
            //HeadPos.Add(oneHead);
            return oneHead;
        }

        public void integrateHead()
        {
            //Test it
            FrameTime timespan= new FrameTime(start);
            Debug.Print("The TimeSpan.start: " + timespan.start.ToString("hh:mm:ss.ffff") + "\n");
            Debug.Print("The TimeSpan.end: " + timespan.end.ToString("hh:mm:ss.ffff") + "\n");
            headSelectedByTimespan.Clear();
            selectHeadByCreatedTime(KinectB_buffer, timespan);
            selectHeadByCreatedTime(KinectC_buffer, timespan);
            selectHeadByCreatedTime(KinectD_buffer, timespan);
            selectHeadByCreatedTime(KinectE_buffer, timespan);
            start = Latestframe;//test

        }

        void selectHeadByCreatedTime(CircularBuffer<Head> buffer, FrameTime TimeSpan)
        {
            foreach (var head in buffer)
            {
                if ((DateTime.Compare(head.createdTime, TimeSpan.start) >= 0) &&
                   (DateTime.Compare(head.createdTime, TimeSpan.end) < 0))
                {
                    if (DateTime.Compare(Latestframe,head.createdTime) < 0)
                    {
                        Latestframe = head.createdTime;
                        //Debug.Print("The latest frame changed to: " + Latestframe.ToString("hh:mm:ss.ffff") + "\n");
                    }
                    headSelectedByTimespan.Enqueue(head);
                }
            }
        }

        public void setStartTime(DateTime starTime)
        {
            start = starTime;
            Latestframe = starTime;
        }
    }
}