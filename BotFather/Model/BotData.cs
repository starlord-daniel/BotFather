using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Unification.Model
{
    public class BotData
    {
        public Directline[] directLine { get; set; }

        public class Directline
        {
            public string name { get; set; }
            public string secret { get; set; }
        }
    }
}
