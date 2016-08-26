using System;
using System.Collections;
using System.Threading;
using Microsoft.Kinect;

namespace SingleKinect.EngagementManage
{
    public class DataToSendnew 
    {
        public char kinectNo = 'M';
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
        public int no;

        CameraSpacePoint pos = new CameraSpacePoint()
        {
            X = 77.0000f,
            Y = 77.0000f,
            Z = 77.0000f
        };
        //Initialize- empty constructor
        public DataToSendnew()
        {
            //Pack 19 JOINTS
            this.Head = pos;
            this.Neck = pos;
            this.SpineShoulder = pos;
            this.SpineMid = pos;
            this.SpineBase = pos;
            this.ShoulderRight = pos;
            this.ShoulderLeft = pos;
            this.ElbowRight = pos;
            this.ElbowLeft = pos;
            this.WristRight = pos;
            this.WristLeft = pos;
            this.HipRight = pos;
            this.HipLeft = pos;
            this.KneeRight = pos;
            this.KneeLeft = pos;
            this.AnkleRight = pos;
            this.AnkleLeft = pos;
            this.RightHandJoint = pos;
            this.LeftHandJoint = pos;
            //this.FootLeft = body.Joints[JointType.FootLeft].Position;
            //FootRight = body.Joints[JointType.FootRight].Position;
            hasReceived = false;
        }

        public DataToSendnew(Body body,int count)
        {
            //Pack 19 JOINTS
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
            no = count;
        }

        public CameraSpacePoint getPos(int jointKey)
        {
            switch (jointKey)
            {
                case 1:
                    return Head;                    
                case 2:
                    return Neck;
                case 3:
                    return SpineShoulder;
                case 4:
                    return SpineMid;
                case 5:
                    return SpineBase;
                case 6:
                    return ShoulderRight;
                case 7:
                    return ShoulderLeft;
                case 8:
                    return ElbowRight ;
                case 9:
                    return ElbowLeft ;
                case 10:
                    return WristRight ;
                case 11:
                    return WristLeft ;
                case 12:
                    return HipRight;
                case 13:
                    return HipLeft;
                case 14:
                    return KneeRight ;
                case 15:
                    return KneeLeft ;
                case 16:
                    return AnkleRight ;
                case 17:
                    return AnkleLeft ;
                case 18:
                    return RightHandJoint ;
                case 19:
                    return LeftHandJoint ;

                default:
                    Console.WriteLine("Default case- out of range");
                    break;

            }

            Console.WriteLine("Wrong pos is returned \n");
            return pos;
        }

        /*function to return joints type*/
        public Joint return_joint_type(int key,Body personBody)
        {
            switch (key)
            {
                case 1:
                    return personBody.Joints[JointType.Head];
                case 2:
                    return personBody.Joints[JointType.Neck];
                case 3:
                    return personBody.Joints[JointType.SpineShoulder];
                case 4:
                    return personBody.Joints[JointType.SpineMid];
                case 5:
                    return personBody.Joints[JointType.SpineBase];
                case 6:
                    return personBody.Joints[JointType.ShoulderRight];
                case 7:
                    return personBody.Joints[JointType.ShoulderLeft];
                case 8:
                    return personBody.Joints[JointType.ElbowRight];
                case 9:
                    return personBody.Joints[JointType.ElbowLeft];
                case 10:
                    return personBody.Joints[JointType.WristRight];
                case 11:
                    return personBody.Joints[JointType.WristLeft];
                case 12:
                    return personBody.Joints[JointType.HipRight];
                case 13:
                    return personBody.Joints[JointType.HipLeft];
                case 14:
                    return personBody.Joints[JointType.KneeRight];
                case 15:
                    return personBody.Joints[JointType.KneeLeft];
                case 16:
                    return personBody.Joints[JointType.AnkleRight];
                case 17:
                    return personBody.Joints[JointType.AnkleLeft];
                case 18:
                    return personBody.Joints[JointType.HandRight];
                case 19:
                    return personBody.Joints[JointType.HandLeft];

                default:
                    Console.WriteLine("Default case- out of range");
                    break;

            }

            //if none of the key matches, return handtipleft
            return personBody.Joints[JointType.HandTipLeft];
        }

    }
}