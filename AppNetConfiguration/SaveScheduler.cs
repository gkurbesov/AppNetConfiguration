using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AppNetConfiguration
{
    /// <summary>
    /// Delayed save scheduler
    /// </summary>
    public abstract class SaveScheduler
    {
        private Task _task = null;
        protected int WaitSaveInterval = 50;
        /// <summary>
        /// Set wait interval
        /// </summary>
        /// <param name="ms"></param>
        public void SetWaitInterval(int ms) => WaitSaveInterval = ms > 0 ? ms : 50;
        /// <summary>
        /// save with a delay
        /// </summary>
        /// <param name="action"></param>
        protected void ExecuteSave(Action action) => ExecuteSave(action, WaitSaveInterval);
        /// <summary>
        /// save with a delay
        /// </summary>
        /// <param name="action"></param>
        /// <param name="interval"></param>
        protected void ExecuteSave(Action action, int interval)
        {
            if (_task == null)
            {
                _task = Task.Factory.StartNew(() =>
                {
                    Task.Delay(interval > 0 ? interval : 50).Wait();
                    action.Invoke();
                }).ContinueWith(x => _task = null);
            }
        }

    }
}
