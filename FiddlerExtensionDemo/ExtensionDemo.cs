using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;

[assembly: Fiddler.RequiredVersion("2.3.5.0")]

namespace FiddlerExtensionDemo
{
    public class ExtensionDemo : IAutoTamper
    {
        private ExtensionInterface ui = null;

        private String targetFeature = "cmd=getGameRule";


        public void AutoTamperRequestAfter(Session oSession)
        {
            
        }

        public void AutoTamperRequestBefore(Session oSession)
        {
            
        }

        public void AutoTamperResponseAfter(Session oSession)
        {
            
        }

        public void AutoTamperResponseBefore(Session oSession)
        {
            if (this.ui != null && this.ui.isRunning)
            {
                if (oSession.RequestMethod == "POST" && oSession.uriContains(this.targetFeature))
                {
                    try
                    {
                        string response = oSession.GetResponseBodyEncoding().GetString(oSession.responseBodyBytes);
                        this.ui.Log("捕获到ofo跳一跳开始游戏请求");
                        string fakerResult = Faker.ModifyJumpResponse(response);
                        this.ui.Log("faker完成: " + fakerResult);
                        oSession.ResponseBody = oSession.GetResponseBodyEncoding().GetBytes(new String('\n', 17) + fakerResult);
                        this.ui.Log("Response修改完成");
                    }
                    catch(Exception e)
                    {
                        this.ui.Log("PEEK RESPONSE, BUT FAKER FAILURE!!! (" + e.Message + ")");
                    }
                }
            }
        }

        public void OnBeforeReturningError(Session oSession)
        {
            
        }

        public void OnBeforeUnload()
        {
            
        }

        public void OnLoad()
        {
            ui = new ExtensionInterface();
        }
    }
}
