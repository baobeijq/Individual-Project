using System;
using Microsoft.Kinect;

namespace SingleKinect.EngagerTrack
{
    public class DataToSend
    {
        public HandState RightHandState;
        public HandState LeftHandState;

        public TrackingConfidence RighTrackingConfidence;
        public TrackingConfidence LeftTrackingConfidence;

        public CameraSpacePoint RightHandJoint;
        public CameraSpacePoint LeftHandJoint;

        //Skeleton data needs to be sent
        public CameraSpacePoint SpineBase;
        public CameraSpacePoint SpineMid;
        public CameraSpacePoint SpineShoulder;
        public CameraSpacePoint ShoulderLeft;
        public CameraSpacePoint ShoulderRight;
        public CameraSpacePoint Neck;
        public CameraSpacePoint Head;
        public CameraSpacePoint KneeLeft;
        public CameraSpacePoint KneeRight;
        //public CameraSpacePoint FootLeft;
        //public CameraSpacePoint FootRight;
        public CameraSpacePoint ElbowRight;
        public CameraSpacePoint ElbowLeft;
        public CameraSpacePoint HipLeft;
        public CameraSpacePoint HipRight;
        public CameraSpacePoint AnkleLeft;
        public CameraSpacePoint AnkleRight;
        public CameraSpacePoint WristLeft;
        public CameraSpacePoint WristRight;



        public bool hasReceived;
        public DateTime createdTime { get; set; }
    }
}