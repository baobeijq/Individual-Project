using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ILNumerics;//new added
using Microsoft.Kinect;
//using System.Windows.Media;
/*using SharpDX;
using SharpDX.Mathematics.Interop;
using SharpDX.Direct3D9;*/

//using Extreme.Mathematics;
//using Extreme.Mathematics.LinearAlgebra;

namespace SingleKinect.EngagementManage
{

    public class MatrixTransform
    {
        private static MatrixTransform TransformM;

        private MatrixTransform() { }

        public static MatrixTransform Instance
        {
            get
            {
                if (TransformM == null)
                {
                    TransformM = new MatrixTransform();
                }
                return TransformM;
            }
        }

        private const int ColMax = 4;
        private const int RowMax = 19;
        private const int TformSize = 4;
        private int row;
        private int col;
        private int matrixsize;

        float[,] skeletonM = new float[RowMax, ColMax];
        float[,] result = new float[RowMax, ColMax];
        float[,] transformedM = new float[RowMax, ColMax];

        float[] centralFixed;
        float[] centralMoving;

        float[,] tform;
        float[,] tformDtoC;
        float[,] tformEtoD;
        float[,] tformBtoC;

        private string KinectPair;
        private Body personBody;
        private int jointKEY;
        public DataToSendnew sendingData=new DataToSendnew();
        public SendHead sendHead= new SendHead();

        public void createMatrix(Body body)//Could add check tracking state
        {
            personBody = body;
            KinectPair="DtoC";

            tform = chooseTform(KinectPair);
//            if(tform==null)
//            {
//                Debug.Print("Null tform is return");
//                return;
//            }

//            Console.WriteLine("\n tform: ");
//            Printtform(tform);

            //Get the central point pos of the Moving skeleton
            centralMoving = getCentralMoving(KinectPair);
            //Get the central point pos of the reference skeleton
            centralFixed = getCentralFixed(KinectPair);

                    
            //Create 19x4 Matrix for the "decentralized"skeleton position of the Moving Matrix
            for (row = 0; row < RowMax ; row++)
            {
                for (col = 0; col < ColMax ; col++)
                {
                    jointKEY = row + 1;
                    Joint bodyJoint = return_joint_type(jointKEY);
                    if (bodyJoint.Equals(personBody.Joints[JointType.HandTipLeft]))
                    {
                        Console.WriteLine("Out of range - got wrong joint");
                    }

                    switch (col)
                    {
                        case 0:
                            skeletonM[row, col] = bodyJoint.Position.X- centralMoving[0];
                            break;
                        case 1:
                            skeletonM[row, col] = bodyJoint.Position.Y- centralMoving[1]; 
                            break;
                        case 2:
                            skeletonM[row, col] = bodyJoint.Position.Z- centralMoving[2];
                            break;
                        case 3:
                            skeletonM[row, col] = (float) 1.0000;
                            break;
                        default:
                            Console.WriteLine("Problem occur at assigning value, col= " + col);
                            break;
                    }
                }                
            }
            //Console.WriteLine("\n decentralized skeleton:");
            //PrintMatrix(skeletonM);

            #region 
            //Create 19x4 Matrix for the original skeleton position of the Moving Matrix //THIS IS FOR TESTING            
/*            for (row = 0; row < RowMax ; row++)
            {
                for (col = 0; col < ColMax ; col++)
                {
                    jointKEY = row + 1;
                    Joint bodyJoint = return_joint_type(jointKEY);
                    if (bodyJoint.Equals(personBody.Joints[JointType.HandTipLeft]))
                    {
                        Console.WriteLine("Out of range - got wrong joint");
                    }

                    switch (col)
                    {
                        case 0:
                            skeletonM[row, col] = bodyJoint.Position.X;
                            break;
                        case 1:
                            skeletonM[row, col] = bodyJoint.Position.Y;
                            break;
                        case 2:
                            skeletonM[row, col] = bodyJoint.Position.Z;
                            break;
                        case 3:
                            skeletonM[row, col] = (float)1.0000;
                            break;
                        default:
                            Console.WriteLine("Problem occur at assigning value, col= "+col);
                            break;
                    }
                }
            }

            Console.WriteLine("\n original skeleton:");
            PrintMatrix(skeletonM);*/

            #endregion

        }

        public SendHead transform()//return type was DataToSendnew
        {   
            //Apply the tform 
            for (row = 0; row < RowMax; row++)
            {
                for (col = 0; col < ColMax; col++)
                {
                    result[row,col]=0;
                    for (int i = 0; i < ColMax; i++)
                    {
                        result[row,col]= result[row,col]+ skeletonM[row,i]* tform[i,col];
                    }
                    transformedM[row, col] = 0;
                    switch (col)
                    {
                        case 0:
                            transformedM[row, col] = result[row, col] + centralFixed[0];
                            break;
                        case 1:
                            transformedM[row, col] = result[row, col] + centralFixed[1];
                            break;
                        case 2:
                            transformedM[row, col] = result[row, col] + centralFixed[2];
                            break;
                        case 3:
                            transformedM[row, col] = (float)1.0000;
                            break;
                        default:
                            Console.WriteLine("Problem occur at assigning value, col= " + col);
                            break;
                    }
                }

            }

            //Transfer the skeleton to the view of kinect C
            if (KinectPair == "EtoD")
            {
                multipleTransfer("DtoC");
            }

            //test
            //Console.WriteLine("\n transformed skeleton: \n");
            //PrintMatrix(transformedM);



            //Pack the JSON file with post-transformed xyz
            for (row = 0; row < RowMax; row++)
            {
                jointKEY = row + 1;
                Joint Joint = return_joint_type(jointKEY);
                for (col = 0; col < ColMax; col++)
                {


                    if (Joint.Equals(personBody.Joints[JointType.HandTipLeft]))
                    {
                        Console.WriteLine("Out of range - got wrong joint");
                    }

                    switch (col)
                    {
                        case 0:
                            Joint.Position.X = transformedM[row, col] ;
                            break;
                        case 1:
                            Joint.Position.Y = transformedM[row, col];
                            break;
                        case 2:
                            Joint.Position.Z = transformedM[row, col];
                            break;
                        case 3:                            
                            break;
                        default:
                            Console.WriteLine("Problem occur at assigning value, col= " + col);
                            break;
                    }
                    
                }
                if (jointKEY == 1)
                {
                    packHead(Joint.Position,jointKEY);//SendHead Structure
                }
                packData(Joint.Position, jointKEY); //DataToSendNew Structure
            }

            return sendHead;
            //return sendingData;
        }

        public DataToSendnew GetDataToSendnew()
        {
            return sendingData;
        }


        void multipleTransfer(string nextPair)
        {
            KinectPair = nextPair;
            tform = chooseTform(KinectPair);
            //Get the central point pos of the Moving skeleton
            centralMoving = getCentralMoving(KinectPair);
            //Get the central point pos of the reference skeleton
            centralFixed = getCentralFixed(KinectPair);
            //Get new decentralized skeleton
            for (row = 0; row < RowMax; row++)
            {
                for (col = 0; col < ColMax; col++)
                {
                    switch (col)
                    {
                        case 0:
                            skeletonM[row, col] = transformedM[row, col] - centralMoving[0];
                            break;
                        case 1:
                            skeletonM[row, col] = transformedM[row, col] - centralMoving[1];
                            break;
                        case 2:
                            skeletonM[row, col] = transformedM[row, col] - centralMoving[2];
                            break;
                        case 3:
                            break;
                        default:
                            Console.WriteLine("Problem occur at assigning value, col= " + col);
                            break;
                    }
                }
            }

            //Apply the second transform
            for (row = 0; row < RowMax; row++)
            {
                for (col = 0; col < ColMax; col++)
                {

                    result[row, col] = 0;
                    for (int i = 0; i < ColMax; i++)
                    {
                        result[row, col] = result[row, col] + skeletonM[row, i] * tform[i, col];
                    }
                    transformedM[row, col] = 0;
                    switch (col)
                    {
                        case 0:
                            transformedM[row, col] = result[row, col] + centralFixed[0];
                            break;
                        case 1:
                            transformedM[row, col] = result[row, col] + centralFixed[1];
                            break;
                        case 2:
                            transformedM[row, col] = result[row, col] + centralFixed[2];
                            break;
                        case 3:
                            break;
                        default:
                            Console.WriteLine("Problem occur at assigning value, col= " + col);
                            break;
                    }
                }

            }
        }

        float[,] chooseTform(string pair)
        {
            float[,] tformNull=null;

            //This is not an accurate tform -data need to be replaced
            tformBtoC = new float[TformSize, TformSize] {
                {(float) 0.1352, (float) -0.5235, (float) -0.8412, 0},
                {(float) 0.5159, (float) 0.7620, (float) -0.3913, 0},
                {(float) 0.8459, (float) -0.3811, (float) 0.3731, 0} ,
                {(float) -0.0289, (float) 0.0126, (float) 0.0189, (float) 1.0000} };

            //Create 4x4 Tform D1 to C1
            tformDtoC = new float[TformSize, TformSize] {
                {(float) 0.5295, (float) 0.4623, (float) 0.7113, 0},
                {(float) -0.4861, (float) 0.8525, (float) -0.1922, 0},
                {(float) -0.6953, (float) -0.2440, (float) 0.6761, 0} ,
                {(float) 0.0043, (float) 0.0041, (float) 0.0062, (float) 1.0000} };

            //Create 4x4 Tform E1 to D1            
            tformEtoD = new float[TformSize, TformSize] {
                {(float) 0.5121, (float) 0.4607, (float) 0.7249, 0},
                {(float) -0.4551, (float) 0.8613, (float) -0.2260, 0},
                {(float) -0.7285, (float) -0.2142, (float) 0.6507, 0} ,
                {(float) 0.0032, (float) 0.0162, (float) 0.0222, (float) 1.0000} };

            switch (pair)
            {
                case "BtoC":
                    return tformBtoC;
                case "DtoC":
                    return tformDtoC;
                case "EtoD":
                    return tformEtoD;
                //case 3:
                   // skeletonM[row, col] = (float)1.0000;
                   // break;
                default:
                    Console.WriteLine("Problem occur at choosing tform command= " + pair);
                    break;
            }
            return tformNull;
        }

        float[] getCentralMoving(string pair)
        {
            float[] centralMoving = null;

            switch (pair)
            {
                case "BtoC":                            
                    centralMoving = new float[3] { (float)0.2317, (float)0.1091, (float)3.2752 };//B3 Central
                    return centralMoving;
                case "DtoC":
                    centralMoving = new float[3] { (float)-0.0470, (float)0.3552, (float)3.4240 };//D1 Central
                    return centralMoving;
                case "EtoD":                           
                    centralMoving = new float[3] { (float)0.2159, (float)0.2843, (float)3.4088 };//E1 Central
                    return centralMoving;
                //case 3:
                // skeletonM[row, col] = (float)1.0000;
                // break;
                default:
                    Console.WriteLine("Problem occur at choosing central poing of the Moving skeleton= " + pair);
                    break;
            }
            return centralMoving;
        }

        float[] getCentralFixed(string pair)
        {
            float[] centralFixed = null;

            switch (pair)
            {
                case "BtoC":                           
                    centralFixed = new float[3] { (float)0.4057, (float)0.0555, (float)3.1351 };//C3 Central
                    return centralFixed;
                case "DtoC":
                    centralFixed = new float[3] { (float)0.3582, (float)0.2020, (float)3.3951 };//C1 Central
                    return centralFixed;
                case "EtoD":
                    centralFixed = new float[3] { (float)0.2159, (float)0.2843, (float)3.4088 };//D1 Central
                    return centralFixed;
                //case 3:
                // skeletonM[row, col] = (float)1.0000;
                // break;
                default:
                    Console.WriteLine("Problem occur at choosing central poing of the fixed skeleton= " + pair);
                    break;
            }
            return centralFixed;
        }

        /*function to pack the joints into a SendHead Structure*/
        void packHead(CameraSpacePoint pos, int jointKey)
        {
            sendHead.headX = pos.X;
            sendHead.headY = pos.Y;
            sendHead.headZ = pos.Z;

        }

        /*function to pack the joints into a DataToSend Structure*/
        void packData(CameraSpacePoint pos, int jointKey)
        {
            switch (jointKey)
            {
                case 1:
                    sendingData.Head = pos;
                    break;
                case 2:
                    sendingData.Neck = pos;
                    break;
                case 3:
                    sendingData.SpineShoulder = pos;
                    break;
                case 4:
                    sendingData.SpineMid = pos;
                    break;
                case 5:
                    sendingData.SpineBase = pos;
                    break;
                case 6:
                    sendingData.ShoulderRight = pos;
                    break;
                case 7:
                    sendingData.ShoulderLeft = pos;
                    break;
                case 8:
                    sendingData.ElbowRight = pos;
                    break;
                case 9:
                    sendingData.ElbowLeft = pos;
                    break;
                case 10:
                    sendingData.WristRight = pos;
                    break;
                case 11:
                    sendingData.WristLeft = pos;
                    break;
                case 12:
                    sendingData.HipRight = pos;
                    break;
                case 13:
                    sendingData.HipLeft = pos;
                    break;
                case 14:
                    sendingData.KneeRight = pos;
                    break;
                case 15:
                    sendingData.KneeLeft = pos;
                    break;
                case 16:
                    sendingData.AnkleRight = pos;
                    break;
                case 17:
                    sendingData.AnkleLeft = pos;
                    break;
                case 18:
                    sendingData.RightHandJoint = pos;
                    break;
                case 19:
                    sendingData.LeftHandJoint = pos;
                    break;

                default:
                    Console.WriteLine("Default case- out of range");
                    break;

            }

        }

        /*function to return joints type*/
        Joint return_joint_type(int key)
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

        void PrintMatrix(float[,] matrix)
        {
            if (matrix == null) return;
            for (row = 0; row < RowMax ; row++)
            {
                for (col = 0; col < ColMax; col++)
                {
                    Console.Write(string.Format("{0}", matrix[row,col]) + "  ");
                }
                Console.Write("\n");
            }
            Console.WriteLine();
        }

        void Printtform(float[,] matrix)
        {
            if (matrix == null) return;
            for (row = 0; row < TformSize; row++)
            {
                for (col = 0; col < TformSize; col++)
                {
                    Console.Write(string.Format("{0}", matrix[row, col])+"  ");
                }
                Console.Write("\n");
            }
            Console.WriteLine();
        }


        /* foreach (JointType item in Enum.GetValues(typeof(JointType)))
         {
             bodyJoint = personBody.Joints[item];

             //personBody.Joints[item].Position.X = 1.0000f;

             // applying the new values to the joint
             CameraSpacePoint pos = new CameraSpacePoint()
             {
                 X = result[row, col],
                 Y = result[row, col],
                 Z = result[row, col]
             };

             bodyJoint.Position = pos;
             personBody.Joints[item] = bodyJoint;
         }*/
    }
}
