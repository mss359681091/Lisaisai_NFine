using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;

namespace NFine.Web.WebApi.Demo
{
    [RoutePrefix("api/runexe")]
    public class RunExeController : ApiController
    {

        [Route("dotask")]
        /// <summary>
        /// demo模拟打开qq音乐程序
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string DoTask()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = @"D:\Program Files\Tencent\qqmusic\QQMusic.exe";
                //文件名必须加后缀。
                process.Start();
            }
            catch (Exception)
            {

            }
            return "ok";
        }

        [Route("dotasktoken")]
        [ApiSecurityFilter]
        [HttpGet]
        public string DoTaskByToken()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = @"D:\Program Files\Tencent\qqmusic\QQMusic.exe";
                //文件名必须加后缀。
                process.Start();
            }
            catch (Exception)
            {

            }
            return "ok";
        }
    }
}