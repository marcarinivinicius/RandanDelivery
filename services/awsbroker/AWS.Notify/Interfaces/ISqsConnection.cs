﻿
using Amazon.SQS;

namespace AWS.Notify.Interfaces
{
    public interface ISqsConnection
    {
        public IAmazonSQS Client { get; }
    }
}
