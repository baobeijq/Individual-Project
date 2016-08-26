using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Kinect;
using SingleKinect.EngagerTrack;
using SingleKinect.Draw;
using SingleKinect.SendData;
using Newtonsoft.Json;
using System.IO;
//using System.Windows.Media;
using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Direct3D9;

//using Extreme.Mathematics;
//using Extreme.Mathematics.LinearAlgebra;

namespace SingleKinect.EngagementManage
{
    public class EngagementManager
    {
        private const int ENGAGE_BAR = 40;
        private const int DISENGAGE_BAR = 80;
        private bool engage;
        private ulong engageUserID = 0;

        public Dictionary<ulong, MyHuman> users = new Dictionary<ulong, MyHuman>();
        public Dictionary<ulong, int> holdTime = new Dictionary<ulong, int>();
        private readonly Drawer drawer = Drawer.Instance;

        public EngagerTracker eTracker = EngagerTracker.Instance;

        public SendData.SendData SendJson= SendData.SendData.Instance;//new
        public MatrixTransform TransformM= MatrixTransform.Instance;

        public bool HasEngaged
        {
            get
            {
                checkEngage();

                return engage;
            }
            set { engage = value; }
        }

        public bool DisablingEngagement { get; set; }

        public MyHuman Engager
        {
            get
            {
                try
                {
                    return users[engageUserID];
                }
                catch (KeyNotFoundException)
                {
                    Debug.Print("Engager Leaving");
                    users.Remove(engageUserID);
                    engageUserID = 0;
                    HasEngaged = false;

                    return null;
                }
            }
        }


        private void checkEngage()
        {
            
            clearUntrackedUser();
            drawer.clear();

            //SendJson.initialize();
            int count = 0;//test

            foreach (var userTuple in users)
            {
                var user = userTuple.Value;

                //1.Here to do the matrix transformation, then send the transformed data
                //2.visualize the transformation
                Body preTformBody = user.body;//Body before transformation
                TransformM.createMatrix(preTformBody);
                DataToSendnew tformBody = TransformM.transform();//Skeleton after transformation

                tformBody.no = count;
                tformBody.createdTime = DateTime.UtcNow;

                drawer.currentCanvasName = "tformBody";
                drawer.drawSkeleton(preTformBody);
                drawer.drawSkeleton(tformBody,preTformBody);//Get Joint type from preTformBody.draw tformBody
                drawer.clear();//need or not?
                Console.WriteLine("I CLEARED \n");//test


                //3.Get data needed from txt
                //4.Test the skelton rotation in matlab
                //5.Solve the visualize problem
                //6.build basic babylon

                //DataToSendnew sendingData =new DataToSendnew(user.body,count);//new
                Console.WriteLine("Get JSON \n");//test
                //test
                string lineToSend = getSendingJson(tformBody) +"\n";
                //File.WriteAllText(
                  // @"C:\Users\Wei\Desktop\Interaction system for GDO\GDOKinectInteraction\SingleKinect\SendData\Output.json",
                   //lineToSend);

                File.AppendAllText(@"C:\Users\Wei\Desktop\Interaction system for GDO\GDOKinectInteraction\SingleKinect\SendData\OutputTest.json",
                   lineToSend);
                //Connect to server and send data
                //SendJson.connect(sendingData); //seperate the connect and the send
                //send the data of all people tracked to server --color need to be added
                count = count + 1;
                Console.WriteLine("TCP "+count+ " part ends");//test

                if (!engage &&
                    user.body.Joints[JointType.HandRight].Position.Y > user.body.Joints[JointType.Head].Position.Y)
                {
                    if (Math.Abs(user.headYaw) > 30)
                    {
                        Debug.Print("user {0} continued by head yaw {1}", user.body.TrackingId, user.headYaw);
                        //holdTime[userTuple.Key] = 0;
                        continue;
                    }
                    Debug.Print("user {0} pass by head yaw {1}", user.body.TrackingId, user.headYaw);

                    //Debug.Print("user {0}, {1}", userTuple.Key, userTuple.Value);
                    holdTime[userTuple.Key] += 1;
                    Debug.Print("headHoldTime " + userTuple.Key + ": " + holdTime[userTuple.Key]);

                    if (holdTime[userTuple.Key] < ENGAGE_BAR)
                    {
                        continue;
                    }

                    alterEngageState(true, userTuple.Key);
                    Debug.Print("Engage ");

                    break;
                }
            }
            
            //SendJson.clientClose();

            if (engage)
            {
                if (Engager == null)
                {
                    return;
                }

                if (Math.Abs(Engager.headYaw) > 30)
                {
                    //DisablingEngagement = true;
                    holdTime[engageUserID] += 1;
                    Debug.Print("Disengage time: " + holdTime[engageUserID]);

                }
                if (Engager.body.Joints[JointType.HandRight].Position.Y <
                    Engager.body.Joints[JointType.SpineBase].Position.Y + 0.1 || Math.Abs(Engager.headYaw) > 30)
                {
                    DisablingEngagement = true;
                    holdTime[engageUserID] += 1;
                    Debug.Print("Disengage time: " + holdTime[engageUserID]);
                    if (holdTime[engageUserID] < DISENGAGE_BAR)
                    {
                        return;
                    }

                    alterEngageState(false, 0);
                    Debug.Print("Disengage");
                }
                else
                {
                    resetHoldtime();
                    DisablingEngagement = false;
                }
            }

            
            

        }

        private string getSendingJson(DataToSendnew sendingData)//test
        {
            string json = JsonConvert.SerializeObject(sendingData);
            return json;
        }

        private void alterEngageState(bool engageState, ulong key)
        {
            engage = engageState;
            engageUserID = key;

            resetHoldtime();
        }

        private void resetHoldtime()
        {
            List<ulong> keys = new List<ulong>(holdTime.Keys);
            foreach (var key in keys)
            {
                holdTime[key] = 0;
            }
        }

        private void clearUntrackedUser()
        {
            List<ulong> userKeys = new List<ulong>(users.Keys);
            foreach (var key in userKeys)
            {
                if (!users[key].body.IsTracked)
                {
                    users.Remove(key);
                    holdTime.Remove(key);
                }
            }
        }

        public void setTracker()
        {
            eTracker.Engager = Engager.body;
        }

        public void getTrackerInfo(out Body body, out HandState left, out HandState right, out int yaw, out int pitch,
            out int roll)
        {
            body = eTracker.Engager;
            left = eTracker.LeftState;
            right = eTracker.RightState;
            yaw = eTracker.Yaw;
            pitch = eTracker.Pitch;
            roll = eTracker.Roll;
        }

        public void setTrackerFaceOrientation(ulong trackingId)
        {
            eTracker.Pitch = users[trackingId].headPitch;
            eTracker.Roll = users[trackingId].headRoll;
            eTracker.Yaw = users[trackingId].headYaw;
        }


    }
}