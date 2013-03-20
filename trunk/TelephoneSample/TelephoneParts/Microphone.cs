using System.Threading;

namespace TelephoneSample.TelephoneParts
{
    public class Microphone
    {
        // Private variables

        private readonly ISimpleLog _simpleLog;

        private Timer _timer;

        // Constructor

        public Microphone(ISimpleLog simpleLog)
        {
            _simpleLog = simpleLog;
        }

        public void StartReceivingChatter()
        {
            _timer = new Timer((callbackState) => _simpleLog.Write("Microphone receives... - YES!\r\n"), null, 4000, 4000);
        }

        public void StopReceivingChatter()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _timer.Dispose();
        }
    }
}