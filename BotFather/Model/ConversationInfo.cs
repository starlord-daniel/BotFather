using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotFather.Model
{
    public class ConversationInfo
    {
        public string conversationId { get; set; }
        public string token { get; set; }
        public int expires_in { get; set; }
        public string streamUrl { get; set; }
        public string referenceGrammarId { get; set; }
    }
}
