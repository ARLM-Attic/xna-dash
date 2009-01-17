#region

using System;
using System.Timers;

#endregion

namespace XNADash.Level
{
    /// <summary>
    /// Simple class that counts down from whatever is specified.
    /// </summary>
    internal class CountDownTimer
    {
        private readonly Timer aTimer;
        private int countDownFrom;

        /// <summary>
        /// Constructor
        /// </summary>
        public CountDownTimer()
        {
            aTimer = new Timer();
            aTimer.Interval = 1000; // 1000 miliseconds = 1 second
            aTimer.Enabled = true;
            aTimer.Elapsed += aTimer_Elapsed;
        }

        private void aTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            countDownFrom--;
        }

        /// <summary>
        /// Sets the number of seconds we should count down from
        /// </summary>
        /// <param name="countDown">Number of secounds</param>
        public void SetTimer(int countDown)
        {
            if (countDown > 0 && countDown < int.MaxValue)
                countDownFrom = countDown;
            else
                throw new ArgumentOutOfRangeException("Timer setting must be between 1 and " + int.MaxValue);
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            aTimer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            aTimer.Stop();
        }

        /// <summary>
        /// Returns the number of seconds left as a string
        /// </summary>
        /// <returns>Number of seconds left</returns>
        public override string ToString()
        {
            return countDownFrom.ToString();
        }
    }
}