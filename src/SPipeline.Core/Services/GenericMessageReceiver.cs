using SPipeline.Core.Interfaces.Pipeline;

namespace SPipeline.Core.Services
{
    using SPipeline.Core.Interfaces;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class GenericMessageReceiver : IMessageReceiver
    {
        private readonly object _lockObject = new object();
        private bool _isRunning;
        private readonly int _messageReceiveThreadTimeoutMilliseconds;

        public GenericMessageReceiver(int messageReceiveThreadTimeoutMilliseconds)
        {
            _messageReceiveThreadTimeoutMilliseconds = messageReceiveThreadTimeoutMilliseconds;
        }

        public bool IsRunning
        {
            get
            {
                lock (_lockObject)
                {
                    return _isRunning;
                }
            }

            set
            {
                lock (_lockObject)
                {
                    _isRunning = value;
                }
            }
        }

        public Action StartCallback { get; set; }

        public Action StopCallback { get; set; }

        public void Start()
        {
            IsRunning = true;
            var task = Task.Factory.StartNew(() =>
            {
                while (IsRunning)
                {
                    StartCallback?.Invoke();

                    Thread.Sleep(_messageReceiveThreadTimeoutMilliseconds);
                }
            });

            task.Wait();
        }

        public void Stop()
        {
            StopCallback?.Invoke();

            IsRunning = false;
        }
    }
}
