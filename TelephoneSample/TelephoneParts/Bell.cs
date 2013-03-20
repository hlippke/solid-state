using System.Threading;

namespace TelephoneSample.TelephoneParts
{
    /// <summary>
    /// Starts/stops the telephone bell ringing.
    /// </summary>
    public class Bell
    {
        // Private variables

        private readonly ISimpleLog _simpleLog;

        private Timer _timer;

        // Constructor

        public Bell(ISimpleLog simpleLog)
        {
            _simpleLog = simpleLog;
        }

        // Methods

        /// <summary>
        /// Makes the bell ring.
        /// </summary>
        public void Ring()
        {
            _simpleLog.Write("RING... ");
        }

        public void StartRinging()
        {
            _simpleLog.Write("Bell goes... ");

            // Ring after 1 second and then every 3 seconds
            _timer = new Timer((callbackState) => _simpleLog.Write("RING... "),
                null, 1000, 3000);
        }

        public void StopRinging()
        {
            // Stop the bell timer
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            _timer.Dispose();

            _simpleLog.Write("SILENT!\r\n");
        }
    }
}