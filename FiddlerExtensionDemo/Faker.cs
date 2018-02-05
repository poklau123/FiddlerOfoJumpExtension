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
        public int rt { get; set; }
        public bool success { get; set; }
        public data data { get; set; }
        public string msg { get; set; }
    }

    class data
    {
        public string info { get; set; }
        public string sign { get; set; }
    }

    class info
    {
        public long startTime { get; set; }
        public rule rule { get; set; }
    }

    class rule
    {
        public long initTime { get; set; }
        public List<decimal> xList { get; set; }
        public float cWidth { get; set; }
        public float jWidth { get; set; }
    }

    public class Faker
    {
        public static string ModifyJumpResponse(String trueResponseText)
        {
            JumperGameResponse res = JsonConvert.DeserializeObject<JumperGameResponse>(trueResponseText);
            info info = JsonConvert.DeserializeObject<info>(res.data.info);
            List<decimal> xList = info.rule.xList;
            int count = xList.Count;
            xList.Clear();

            Random random = new Random(DateTime.Now.Millisecond);
            for(int i = 0; i < count; i++)
            {
                int _base = (i % 3 + 1) * 3;
                bool is_add = random.Next(1, 10) > 5;
                decimal seed = (decimal)random.Next(1000, 9999) / 10000;
                decimal _r = (is_add ? _base + seed : _base - seed);

                xList.Add((decimal)7.2516);
            }

            res.data.info = JsonConvert.SerializeObject(info);

            return JsonConvert.SerializeObject(res);
        }
    }
}
