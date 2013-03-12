using System;
using System.Threading;

namespace PC.ServiceBus
{
    /// <summary>
    /// Provides a way to throttle the work depending on the number of jobs it is able to complete and whether
    /// the job is penalized for trying to parallelize too many jobs.
    /// </summary>
    public class DynamicThrottling : IDisposable
    {
        private readonly int _maxDegreeOfParallelism;
        private readonly int _minDegreeOfParallelism;
        private readonly int _penaltyAmount;
        private readonly int _workFailedPenaltyAmount;
        private readonly int _workCompletedParallelismGain;
        private readonly int _intervalForRestoringDegreeOfParallelism;

        private readonly AutoResetEvent _waitHandle = new AutoResetEvent(true);
        private readonly Timer _parallelismRestoringTimer;

        private int _currentParallelJobs = 0;
        private int _availableDegreesOfParallelism;

        /// <summary>
        /// Initializes a new instance of <see cref="DynamicThrottling"/>.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">Maximum number of parallel jobs.</param>
        /// <param name="minDegreeOfParallelism">Minimum number of parallel jobs.</param>
        /// <param name="penaltyAmount">Number of degrees of parallelism to remove when penalizing slightly.</param>
        /// <param name="workFailedPenaltyAmount">Number of degrees of parallelism to remove when work fails.</param>
        /// <param name="workCompletedParallelismGain">Number of degrees of parallelism to restore on work completed.</param>
        /// <param name="intervalForRestoringDegreeOfParallelism">Interval in milliseconds to restore 1 degree of parallelism.</param>
        public DynamicThrottling(
            int maxDegreeOfParallelism,
            int minDegreeOfParallelism,
            int penaltyAmount,
            int workFailedPenaltyAmount,
            int workCompletedParallelismGain,
            int intervalForRestoringDegreeOfParallelism)
        {
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
            _minDegreeOfParallelism = minDegreeOfParallelism;
            _penaltyAmount = penaltyAmount;
            _workFailedPenaltyAmount = workFailedPenaltyAmount;
            _workCompletedParallelismGain = workCompletedParallelismGain;
            _intervalForRestoringDegreeOfParallelism = intervalForRestoringDegreeOfParallelism;
            _parallelismRestoringTimer = new Timer(s => IncrementDegreesOfParallelism(1));

            _availableDegreesOfParallelism = minDegreeOfParallelism;
        }

        public int AvailableDegreesOfParallelism
        {
            get { return _availableDegreesOfParallelism; }
        }

        public void WaitUntilAllowedParallelism(CancellationToken cancellationToken)
        {
            while (_currentParallelJobs >= _availableDegreesOfParallelism)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                _waitHandle.WaitOne();
            }
        }

        public void NotifyWorkCompleted()
        {
            Interlocked.Decrement(ref _currentParallelJobs);
            IncrementDegreesOfParallelism(_workCompletedParallelismGain);
        }

        public void NotifyWorkStarted()
        {
            Interlocked.Increment(ref _currentParallelJobs);
        }

        public void Penalize()
        {
            // Slightly penalize with removal of some degrees of parallelism.
            DecrementDegreesOfParallelism(_penaltyAmount);
        }

        public void NotifyWorkCompletedWithError()
        {
            // Largely penalize with removal of several degrees of parallelism.
            DecrementDegreesOfParallelism(_workFailedPenaltyAmount);
            Interlocked.Decrement(ref _currentParallelJobs);
            _waitHandle.Set();
        }

        public void Start(CancellationToken cancellationToken)
        {
            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() => _parallelismRestoringTimer.Change(Timeout.Infinite, Timeout.Infinite));
            }

            _parallelismRestoringTimer.Change(_intervalForRestoringDegreeOfParallelism, _intervalForRestoringDegreeOfParallelism);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _waitHandle.Dispose();
                _parallelismRestoringTimer.Dispose();
            }
        }

        private void IncrementDegreesOfParallelism(int count)
        {
            if (_availableDegreesOfParallelism < _maxDegreeOfParallelism)
            {
                _availableDegreesOfParallelism += count;
                if (_availableDegreesOfParallelism >= _maxDegreeOfParallelism)
                {
                    _availableDegreesOfParallelism = _maxDegreeOfParallelism;
                }
            }

            _waitHandle.Set();
        }

        private void DecrementDegreesOfParallelism(int count)
        {
            _availableDegreesOfParallelism -= count;
            if (_availableDegreesOfParallelism < _minDegreeOfParallelism)
            {
                _availableDegreesOfParallelism = _minDegreeOfParallelism;
            }
        }
    }
}
