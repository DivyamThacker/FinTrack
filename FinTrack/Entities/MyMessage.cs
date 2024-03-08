using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FinTrack
{

    public class MyMessage : ValueChangedMessage<string>
    {
        public MyMessage(string value) : base(value)
        {
        }
    }
}
