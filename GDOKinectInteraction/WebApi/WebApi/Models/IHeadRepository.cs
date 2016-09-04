using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public interface IHeadRepository
    {
        void setStartTime(DateTime starTime);
        IEnumerable<Head> GetAll();
        IEnumerable<Head> GetSelectedHeads(int id); //type Head
        Head Add(Head item);
        void integrateHead();
        int FrameNo { get; set; }
    }
}