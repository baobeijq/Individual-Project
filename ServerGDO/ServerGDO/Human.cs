using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;

namespace ServerGDO
{
    public class Human
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

        public void InitializeBody(Body body)
        {
            this.RightHandJoint = body.Joints[JointType.HandRight].Position;
            this.LeftHandJoint = body.Joints[JointType.HandLeft].Position;

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
            //this.FootRight = body.Joints[JointType.FootRight].Position;
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
