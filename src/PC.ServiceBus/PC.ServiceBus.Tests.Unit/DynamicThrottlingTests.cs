using NUnit.Framework;

namespace PC.ServiceBus.Tests.Unit
{
    public class DynamicThrottlingFixture
    {
        [Test]
        public void GivenALowMinimumDegreesOfParallelism_WhenConstructing_ThenAvailableDegreesOfParallelismIsMinimum()
        {
            using (var sut = new DynamicThrottling(100, 10, 3, 5, 1, 8000))
            {
                Assert.AreEqual(10, sut.AvailableDegreesOfParallelism);
            }
        }

        [Test]
        public void GivenAValidDynamicThrottlingObject_WhenNotifiedOfWorkCompleted_ThenDegreesOfParallelismIncreased()
        {
            using (var sut = new DynamicThrottling(100, 10, 3, 5, 1, 8000))
            {
                sut.NotifyWorkStarted();
                var startingValue = sut.AvailableDegreesOfParallelism;

                sut.NotifyWorkCompleted();

                Assert.True(startingValue < sut.AvailableDegreesOfParallelism);
            }
        }

        [Test]
        public void GivenAValidDynamicThrottlingObject_WhenNotifiedOfWorkCompletedMultipleTImes_ThenDegreesOfParallelismIncreasedMultipleTimes()
        {
            using (var sut = new DynamicThrottling(100, 10, 3, 5, 1, 8000))
            {
                for (int i = 0; i < 10; i++)
                {
                    sut.NotifyWorkStarted();
                    var startingValue = sut.AvailableDegreesOfParallelism;
                    sut.NotifyWorkCompleted();
                    Assert.True(startingValue < sut.AvailableDegreesOfParallelism);
                }
            }
        }

        [Test]
        public void GivenAValidDynamicThrottlingObject_WhenPenalized_ThenDegreesOfParallelismDecreased()
        {
            using (var sut = new DynamicThrottling(100, 10, 3, 5, 1, 8000))
            {
                IncreaseDegreesOfParallelism(sut);

                sut.NotifyWorkStarted();
                var startingValue = sut.AvailableDegreesOfParallelism;
                sut.Penalize();

                Assert.True(startingValue > sut.AvailableDegreesOfParallelism);
            }
        }

        [Test]
        public void GivenAValidDynamicThrottlingObject_WhenNotifiesOfWorkCompletedWithError_ThenDegreesOfParallelismDecreasedMoreThanIfPenalized()
        {
            using (var sut1 = new DynamicThrottling(100, 10, 3, 5, 1, 8000))
            using (var sut2 = new DynamicThrottling(100, 10, 3, 5, 1, 8000))
            {
                IncreaseDegreesOfParallelism(sut1);
                IncreaseDegreesOfParallelism(sut2);

                sut1.NotifyWorkStarted();
                sut2.NotifyWorkStarted();

                sut1.Penalize();
                sut2.NotifyWorkCompletedWithError();

                Assert.True(sut1.AvailableDegreesOfParallelism > sut2.AvailableDegreesOfParallelism);
            }
        }

        private static void IncreaseDegreesOfParallelism(DynamicThrottling sut)
        {
            for (int i = 0; i < 10; i++)
            {
                // increase degrees to avoid being in the minimum boundary
                sut.NotifyWorkStarted();
                sut.NotifyWorkCompleted();
            }
        }
    }
}
