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
                        this.ui.Log(response);
                        string fakerResult = Faker.ModifyJumpResponse(response);
                        this.ui.Log(fakerResult);
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
