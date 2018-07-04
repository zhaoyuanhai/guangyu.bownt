using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarWare.Common;

namespace BES2015
{
    /// <summary>
    /// uploadfilehandler 的摘要说明
    /// </summary>
    public class uploadfilehandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //if (!LoginBLL.isLogin()) return;

            string fileType = "all";
            UploadFile upfile = new UploadFile();
            upfile.fileflag = -3;
            string jsonStr = UploadFileInfo.ToJson(upfile);

            if (context.Request.Params["type"] != null && context.Request.Params["type"].ToString().Trim() != "")
            {
                fileType = context.Request.Params["type"];
            }

            try
            {

            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(jsonStr);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}