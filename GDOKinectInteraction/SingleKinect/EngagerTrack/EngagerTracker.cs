using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Kinect;
using SingleKinect.MyDataStructures;


namespace SingleKinect.EngagerTrack
{
    public class EngagerTracker
    {
        private static EngagerTracker engagerTracker;
        private EngagerTracker() { }

        public static EngagerTracker Instance
        {
            get
            {
                if (engagerTracker == null)
                {
                    engagerTracker = new EngagerTracker();
                }
                return engagerTracker;
            }
        }

        public Joint? preHandLeftPoint = null;
        public Joint? preHandRightPoint = null;
        public Joint curHandLeftPoint;
        public Joint curHandRightPoint;
        
        private Body engager;
        public Body Engager
        {
            get
            {
                return engager;
            }
            set
            {
                engager = value;
                updateHands();
            } 
        }

        public double ScrollDis { get; set; }
        public bool IsVerticalScroll { get; set; }
        public RECT IncrementRect { get; set; }

        //Positive value for moving down or moving right
        public double VerticalLeftMovement => preHandLeftPoint.Value.Position.Y - curHandLeftPoint.Position.Y;
        public double HorizontalLeftMovement => curHandLeftPoint.Position.X - preHandLeftPoint.Value.Position.X;
        public double VerticalRightMovement => preHandRightPoint.Value.Position.Y - curHandRightPoint.Position.Y;
        public double HorizontalRightMovement => curHandRightPoint.Position.X - preHandRightPoint.Value.Position.X;

        private HandState lastHighConfidenceRightState = HandState.Open;
        private HandState lastHighConfidenceLeftState = HandState.Open;

        public HandState LeftState
        {
            get
            {
                if (engager.HandLeftConfidence == TrackingConfidence.High)
                {
                    lastHighConfidenceLeftState = engager.HandLeftState;
                    if (engager.HandLeftState == HandState.NotTracked || engager.HandLeftState == HandState.Unknown)
                    {
                        lastHighConfidenceLeftState = HandState.Open;
                    }
                }
                
                return lastHighConfidenceLeftState;
            }
        }

        public HandState RightState
        {
            get
            {
                if (engager.HandRightConfidence == TrackingConfidence.High)
                {
                    lastHighConfidenceRightState = engager.HandRightState;
                    if (engager.HandRightState == HandState.NotTracked || engager.HandRightState == HandState.Unknown)
                    {
                        lastHighConfidenceRightState = HandState.Open;
                    }
                }
                
                return lastHighConfidenceRightState;
            }
        }

        public int Pitch { get; set; }
        public int Yaw { get; set; }
        public int Roll { get; set; }

/*        //not used
        public DataToSend DataToSend
        {
            get
            {
                return composeDataToSend();
            }
        }
        //not used
        private DataToSend composeDataToSend()
        {
            return new DataToSend
            {
                LeftHandJoint = engager.Joints[JointType.HandLeft].Position,
                RightHandJoint = engager.Joints[JointType.HandRight].Position,
                
                //new added skeleton data
                SpineBase = engager.Joints[JointType.SpineBase].Position,
                SpineMid = engager.Joints[JointType.SpineMid].Position,
                SpineShoulder = engager.Joints[JointType.SpineShoulder].Position,
                ShoulderLeft = engager.Joints[JointType.ShoulderLeft].Position,
                ShoulderRight = engager.Joints[JointType.ShoulderRight].Position,
                Neck = engager.Joints[JointType.Neck].Position,
                Head = engager.Joints[JointType.Head].Position,
                KneeLeft = engager.Joints[JointType.KneeLeft].Position,
                KneeRight = engager.Joints[JointType.KneeRight].Position,
                //FootLeft = engager.Joints[JointType.FootLeft].Position,
                //FootRight = engager.Joints[JointType.FootRight].Position,
                ElbowLeft = engager.Joints[JointType.ElbowLeft].Position,
                ElbowRight = engager.Joints[JointType.ElbowRight].Position,
                HipLeft = engager.Joints[JointType.HipLeft].Position,
                HipRight = engager.Joints[JointType.HipRight].Position,
                AnkleLeft = engager.Joints[JointType.AnkleLeft].Position,
                AnkleRight = engager.Joints[JointType.AnkleRight].Position,
                WristLeft = engager.Joints[JointType.WristLeft].Position,
                WristRight = engager.Joints[JointType.WristRight].Position,


                LeftHandState = LeftState,
                RightHandState = RightState,


                LeftTrackingConfidence = engager.HandLeftConfidence,
                RighTrackingConfidence = engager.HandRightConfidence,

                // more things needs to be added here as well as the DataToSend struct
                // including jointtype face orientation-directly can be used

                hasReceived = false,
                createdTime = DateTime.UtcNow

            };

        }*/

        private void updateHands()
        {
            Joint tempLeftJoint = curHandLeftPoint;
            Joint tempRightJoint = curHandRightPoint;

            curHandLeftPoint = Engager.Joints[JointType.HandLeft];
            curHandRightPoint = Engager.Joints[JointType.HandRight];

            if (!preHandRightPoint.HasValue)
            {
                preHandRightPoint = curHandRightPoint;
            }
            else
            {
                preHandRightPoint = tempRightJoint;

            }

            if (!preHandLeftPoint.HasValue)
            {
                preHandLeftPoint = curHandLeftPoint;
            }
            else
            {
                preHandLeftPoint = tempLeftJoint;

            }
        }

    }
}