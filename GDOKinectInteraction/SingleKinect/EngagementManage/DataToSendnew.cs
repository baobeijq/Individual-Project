using System;
using Microsoft.Kinect;

namespace SingleKinect.EngagementManage
{
    public class DataToSendnew
    {
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

        public DataToSendnew(Body body)
        {
            this.RightHandJoint = body.Joints[JointType.HandRight].Position;
            this.RightHandJoint = body.Joints[JointType.HandRight].Position;

                //new added skeleton data
            this.SpineBase = body.Joints[JointType.SpineBase].Position;
            this.SpineMid = body.Joints[JointType.SpineMid].Position;
            this.SpineShoulder = body.Joints[JointType.SpineShoulder].Position;
            this.ShoulderLeft = body.Joints[JointType.ShoulderLeft].Position;
            this.ShoulderRight = body.Joints[JointType.ShoulderRight].Position;
            this.Neck = body.Joints[JointType.Neck].Position;
            this.Head = body.Joints[JointType.Head].Position;
            this.KneeLeft = body.Joints[JointType.KneeLeft].Position;
            this.KneeRight = body.Joints[JointType.KneeRight].Position;
            //this.FootLeft = body.Joints[JointType.FootLeft].Position;
                //FootRight = body.Joints[JointType.FootRight].Position;
            this.ElbowLeft = body.Joints[JointType.ElbowLeft].Position;
            this.ElbowRight = body.Joints[JointType.ElbowRight].Position;
            this.HipLeft = body.Joints[JointType.HipLeft].Position;
            this.HipRight = body.Joints[JointType.HipRight].Position;
            this.AnkleLeft = body.Joints[JointType.AnkleLeft].Position;
            this.AnkleRight = body.Joints[JointType.AnkleRight].Position;
            this.WristLeft = body.Joints[JointType.WristLeft].Position;
            this.WristRight = body.Joints[JointType.WristRight].Position;

            hasReceived = false;
            createdTime = DateTime.UtcNow;
        }
    }
}