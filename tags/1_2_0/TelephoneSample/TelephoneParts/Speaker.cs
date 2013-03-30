using System.Threading;

namespace TelephoneSample.TelephoneParts
{
    /// <summary>
    /// Represents the telephone speaker, playing various sounds by writing
    /// them in the log...
    /// </summary>
    public class Speaker
    {
        // Private variables

        private readonly ISimpleLog _simpleLog;

        private Timer _timer;

        // Private methods

        private void StopTimer()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _timer.Dispose();
        }

        // Constructor

        public Speaker(ISimpleLog simpleLog)
        {
            _simpleLog = simpleLog;
        }

        // Methods

        public void StartLongBeep()
        {
            _simpleLog.Write("Speaker goes... BEE");

            _timer = new Timer((callbackState) => _simpleLog.Write("E"), null, 1000, 1000);
        }

        public void StopLongBeep()
        {
            // Stop long beep timer
            StopTimer();

            _simpleLog.Write("P!\r\n");
        }

        public void StartWaitForAnswerBeep()
        {
            _simpleLog.Write("Speaker goes... ");
            _timer = new Timer((callbackState) => _simpleLog.Write("BEEP... "), null, 1000, 4000);
        }

        public void StopWaitForAnswerBeep()
        {
            StopTimer();
            _simpleLog.Write("SILENT!\r\n");
        }

        public void StartFastBeeps()
        {
            _simpleLog.Write("Speaker goes... ");
            _timer = new Timer((callbackState) => _simpleLog.Write("BEEP... "), null, 500, 500);
        }

        public void StopFastBeeps()
        {
            StopTimer();
            _simpleLog.Write("SILENT!\r\n");
        }

        public void StartPlayingChatter()
        {
            _timer = new Timer((callbackState) => _simpleLog.Write("Speaker goes... - REALLY?\r\n"), null, 2000, 4000);
        }

        public void StopPlayingChatter()
        {
            StopTimer();
        }
    }
}