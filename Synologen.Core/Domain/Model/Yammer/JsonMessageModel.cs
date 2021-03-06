﻿using System.Linq;

namespace Spinit.Wpc.Synologen.Core.Domain.Model.Yammer
{
    public class JsonMessageModel
    {
        public MessageModel[] messages { get; set; }
        public ReferenceModel[] references { get; set; }

        public void RemoveMessagesWithIdLessOrEqualTo(int id)
        {
            messages = messages.Where(x => x.id > id).ToArray();
        }
    }
}
