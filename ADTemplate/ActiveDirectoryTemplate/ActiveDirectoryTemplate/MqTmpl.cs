using IBM.WMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    public class MqTmpl
    {
        public string HostName { get; set; }
        public string Channel { get; set; }
        public int Port { get; set; }
        public string QueueManagerName { get; set; }
        public int? CharacterSet { get; set; }
        public int? WaitInterval { get; set; }
        private MQQueueManager queueManager;

        public MqTmpl(string hostName, string channel, int port, string queueManagerName)
        {
            HostName = hostName;
            Channel = channel;
            Port = port;
            QueueManagerName = queueManagerName;
            MQEnvironment.Hostname = HostName;
            MQEnvironment.Channel = Channel;
            MQEnvironment.Port = Port;
            queueManager = new MQQueueManager(QueueManagerName);
        }
        public MqTmpl(string hostName, string channel, int port, string queueManagerName, int characterSet, int waitInterval)
        {
            HostName = hostName;
            Channel = channel;
            Port = port;
            QueueManagerName = queueManagerName;
            CharacterSet = characterSet;
            WaitInterval = waitInterval;
            MQEnvironment.Hostname = HostName;
            MQEnvironment.Channel = Channel;
            MQEnvironment.Port = Port;
            queueManager = new MQQueueManager(QueueManagerName);
        }
        public static MQMessage BuildMessage(string message)
        {
            MQMessage msg = new MQMessage();
            msg.MessageType = MQC.MQMT_REQUEST;
            msg.Format = MQC.MQFMT_STRING;
            msg.WriteString(message);
            return msg;
        }
        public T PutAndGetByCorrelationId<T>(MQMessage putMessage, string putQueueName, string getQueueName, Func<MQMessage, T> msgCallback)
        {
            if (CharacterSet.HasValue)
            {
                putMessage.CharacterSet = CharacterSet.Value;
            }
            putMessage.ReplyToQueueManagerName = QueueManagerName;
            putMessage.ReplyToQueueName = getQueueName;

            using (MQQueue putQueue = queueManager.AccessQueue(putQueueName, MQC.MQOO_OUTPUT))
            { 
                putQueue.Put(putMessage, new MQPutMessageOptions()
                {
                    Options = MQC.MQPMO_NO_SYNCPOINT + MQC.MQPMO_NEW_MSG_ID
                });
                queueManager.Commit();           
            }

            MQMessage getMessage = new MQMessage();
            getMessage.Encoding = MQC.MQENC_NATIVE;
            if(CharacterSet.HasValue)
            {
                getMessage.CharacterSet = CharacterSet.Value;
            }
            byte[] messageId = putMessage.MessageId;
            getMessage.CorrelationId = messageId;

            MQGetMessageOptions gmo = new MQGetMessageOptions();
            if(WaitInterval.HasValue)
            {
                gmo.WaitInterval = WaitInterval.Value;
            }
            gmo.Options = gmo.Options | MQC.MQGMO_CONVERT | MQC.MQGMO_WAIT | MQC.MQGMO_NO_SYNCPOINT;
            gmo.MatchOptions = MQC.MQMO_MATCH_CORREL_ID;
            using (MQQueue replyQueue = queueManager.AccessQueue(getQueueName, MQC.MQOO_INPUT_AS_Q_DEF))
            {
                replyQueue.Get(getMessage, gmo);
                T result = msgCallback.Invoke(getMessage);
                queueManager.Commit();
                return result;
            }                    
        }
        public T PutAndGetFromDynamicQueue<T>(MQMessage putMessage, string putQueueName, string getQueueName, Func<MQMessage, T> msgCallback)
        {
            if (CharacterSet.HasValue)
            {
                putMessage.CharacterSet = CharacterSet.Value;
            }
            putMessage.ReplyToQueueManagerName = QueueManagerName;
            putMessage.ReplyToQueueName = getQueueName;

            using (MQQueue putQueue = queueManager.AccessQueue(putQueueName, MQC.MQOO_OUTPUT))
            {
                putQueue.Put(putMessage, new MQPutMessageOptions()
                {
                    Options = MQC.MQPMO_NO_SYNCPOINT + MQC.MQPMO_NEW_MSG_ID
                });
                queueManager.Commit();
            }

            MQMessage getMessage = new MQMessage();
            getMessage.Encoding = MQC.MQENC_NATIVE;
            if (CharacterSet.HasValue)
            {
                getMessage.CharacterSet = CharacterSet.Value;
            }

            MQGetMessageOptions gmo = new MQGetMessageOptions();
            if (WaitInterval.HasValue)
            {
                gmo.WaitInterval = WaitInterval.Value;
            }
            gmo.Options = gmo.Options | MQC.MQGMO_CONVERT | MQC.MQGMO_WAIT | MQC.MQGMO_NO_SYNCPOINT;
            using (MQQueue replyQueue = queueManager.AccessQueue(getQueueName, MQC.MQOO_INPUT_AS_Q_DEF))
            {
                replyQueue.Get(getMessage, gmo);
                T result = msgCallback.Invoke(getMessage);
                queueManager.Commit();
                return result;
            }
        }

    }
}
