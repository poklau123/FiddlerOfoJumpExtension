using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FiddlerExtensionDemo
{
    class JumperGameResponse
    {
        public string msg { get; set; }
        public int rt { get; set; }
        public bool success { get; set; }
        public data data { get; set; }
    }

    class data
    {
        public string sign { get; set; }
        public info info { get; set; }
    }

    class info
    {
        public decimal startTime { get; set; }
        public rule rule { get; set; }
    }

    class rule
    {
        public decimal initTime { get; set; }
        public List<float> xList { get; set; }
        public float cWidth { get; set; }
        public float jWidth { get; set; }
    }

    public class Faker
    {
        public static string ModifyJumpResponse(String trueResponseText)
        {
            JumperGameResponse res = (JumperGameResponse)JsonConvert.DeserializeObject(trueResponseText);
            List<float> xList = res.data.info.rule.xList;
            int count = xList.Count;
            xList.Clear();

            Random random = new Random(DateTime.Now.Millisecond);
            for(int i = 0; i < count; i++)
            {
                int _base = (i % 3 + 1) * 3;
                bool is_add = random.Next(1, 10) > 5;
                int seed = random.Next(1000, 9999);
                float _r = is_add ? _base + (float)seed / 1000 : _base - (float)seed / 1000;

                xList.Add(_r);
            }

            return JsonConvert.SerializeObject(res);
        }
    }
}
