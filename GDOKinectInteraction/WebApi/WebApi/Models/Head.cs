using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Microsoft.Kinect;

namespace WebApi.Models
{
    public class Head
    {
        //public CameraSpacePoint Headpos { get; set; }
        //public DateTime createdTime { get; set; }
        public int Id { get; set; }
        public string kinectNo { get; set; }
        public int bodyIndex { get; set; }
        public float headX { get; set; }
        public float headY { get; set; }
        public float headZ { get; set; }
        public DateTime createdTime { get; set; }
        //public CameraSpacePoint headPosition;
    }
}