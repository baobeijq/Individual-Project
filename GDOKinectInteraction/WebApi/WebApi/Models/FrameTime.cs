using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class FrameTime
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public FrameTime(DateTime setStart)
        {
            start = setStart;
            end = setStart.AddSeconds(1);
        }
    }
}