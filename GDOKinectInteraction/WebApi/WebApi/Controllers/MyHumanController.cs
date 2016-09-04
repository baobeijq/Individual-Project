using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using JetBlack.Caching.Collections.Generic;
using WebApi.Models;
using Newtonsoft.Json;
//using Microsoft.Kinect;

namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    public class MyHumanController : ApiController
    {
        static readonly IHeadRepository repository = new HeadRepository();        

        public IEnumerable<Head> GetAllHeads()
        {
            return repository.GetAll();
        }

/*        public Head GetHeadById(int id)
        {
            Head item = repository.Get(id);
            //var product = products.FirstOrDefault((p) => p.Id == id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }*/

        public IEnumerable<Head> GetHeadByBuffer(int id)
        {
            return repository.GetSelectedHeads(id); 
        }

        public HttpResponseMessage PostHead(Head item)
        {
            Debug.Print("frameNo pl1: " + repository.FrameNo + "\n");
            if (repository.FrameNo == 0) //When the first frame comes in
            {
                repository.setStartTime(item.createdTime);
                Debug.Print("The first frame arrived with created time: " + item.createdTime.ToString("hh:mm:ss.ffff") + "\n");
            }

            item = repository.Add(item);

            repository.FrameNo = repository.FrameNo+ 1;
            if (repository.FrameNo % 5 == 0)//set number to 25 in lab
            {
                Debug.Print("frameNo pl2: " + repository.FrameNo + "\n");
                repository.integrateHead();
            }
            //DataReceived data1=JsonConvert.DeserializeObject<DataReceived>(Datatosend);

            //响应码（Response code）：默认地，Web API框架把响应状态码设置为200（OK）。
            //但据HTTP/1.1协议，在POST请求形成资源创建时，服务器应当用状态201（已创建）进行回答。

            //这个CreateResponse方法创建一个HttpResponseMessage，
            //并自动地把一个序列化的Product对象表达式写入响应消息体
            var response = Request.CreateResponse<Head>(HttpStatusCode.Created, item);
            string uri = Url.Link("DefaultApi", new { id = item.Id });
            //当服务器创建一个资源时，它应当在响应的Location报头中包含新资源的URI。
            response.Headers.Location = new Uri(uri);
            
            return response;
        }

/*        public IEnumerable<Head> GetHeadByCreatedTime(FrameTime frameTimeSpan)
        {

            return repository.GetAll().Where(
                head =>(DateTime.Compare(head.createdTime, frameTimeSpan.frameStart)>0) &&
                (DateTime.Compare(head.createdTime, frameTimeSpan.frameEnd) < 0));

        }*/
    }
}
