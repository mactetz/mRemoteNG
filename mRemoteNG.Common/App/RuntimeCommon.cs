using mRemoteNG.Messages;
using mRemoteNG.Security;
using System.Security;

namespace mRemoteNG.App
{
    public static class RuntimeCommon
    {
        public static MessageCollector MessageCollector { get; } = new MessageCollector();
    }
}