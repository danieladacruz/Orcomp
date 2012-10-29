using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Orc.Entities;
using Orc.Extensions;

namespace Orc.NunitTests
{
    [TestFixture]
    public class AccountForEfficienciesTest_SimpleFixedEnd
    {
        DateTime now;
        List<DateIntervalEfficiency> dateIntervalEfficiencies;

        [SetUp]
        public void Init()
        {
            this.now = DateTime.Now;
            this.dateIntervalEfficiencies = new List<DateIntervalEfficiency>();
        }

        [TearDown]
        public void Cleanup()
        {
            this.dateIntervalEfficiencies = null;
        }

        [Test]
        public void AccountForEfficiencies_FixedEnd_EmptyListOfDateIntervalEfficiencies_ReturnsUnchangedDateInterval()
        {
            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(1));

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies, FixedEndPoint.Max);

            // Assert
            Assert.AreEqual(dateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_FixedEnd_OneZeroEfficiencyCalendarStartAndEndWithDateInterval_ReturnsDateIntervalOfSameDurationWhichStartsWhenTheEfficiencyFinishes()
        {
            // +-----------+ 
            // |-----0-----| 

            // Result:
            // +-----------------------+
            //             |-----0-----|


            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(1));
            var efficiency1 = new DateIntervalEfficiency(dateInterval.Min.Value, dateInterval.Max.Value, 0);
            dateIntervalEfficiencies.Add(efficiency1);

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies, FixedEndPoint.Max);

            var correctDateInterval = new DateInterval(dateInterval.Min.Value.Add(-efficiency1.Duration), dateInterval.Max.Value);

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_FixedEnd_OneZeroEfficiencyCalendarStartAndEndsBeforeDateIntervalStarts_ReturnsSameDateInterval()
        {
            //             +-----------+ 
            // |-----0-----| 

            // Result:
            //             +-----------+  
            // |-----0-----|


            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(1));

            dateIntervalEfficiencies.Add(new DateIntervalEfficiency(now.AddDays(-4), now, 0));

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies, FixedEndPoint.Max);

            var correctDateInterval = dateInterval;

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_FixedEnd_OneZeroEfficiencyCalendarStartAndEndsAfterDateIntervalEnds_ReturnsSameDateInterval()
        {
            // +-----------+ 
            //             |-----0-----| 

            // Result:
            // +-----------+ 
            //             |-----0-----|  

            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(1));

            dateIntervalEfficiencies.Add(new DateIntervalEfficiency(now.AddDays(1), now.AddDays(3), 0));

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies, FixedEndPoint.Max);

            var correctDateInterval = dateInterval;

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_FixedEnd_OneZeroEfficiencyCalendarStartAndEndsWithinDateInterval_ReturnsCorrectResult()
        {
            //      +-----------+ 
            //         |--0--| 

            // Result:
            //  +---------------+ 
            //         |--0--| 

            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(4));

            var efficiency1 = new DateIntervalEfficiency(now.AddDays(1), now.AddDays(2), 0);
            dateIntervalEfficiencies.Add(efficiency1);

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies, FixedEndPoint.Max);

            var correctDateInterval = new DateInterval(dateInterval.Min.Value.Add(-efficiency1.Duration), dateInterval.Max.Value);

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_FixedEnd_OneZeroEfficiencyCalendarStartBeforeAndEndsWithinDateInterval_ReturnsCorrectResult()
        {
            //       +-----------+ 
            //    |--0--| 

            // Result:
            // +-----------------+ 
            //    |--0--|  

            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(4));

            var efficiency1 = new DateIntervalEfficiency(now.AddDays(-1), now.AddDays(2), 0);
            dateIntervalEfficiencies.Add(efficiency1);

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies, FixedEndPoint.Max);

            var correctDateInterval = new DateInterval(efficiency1.Min.Value.Add(-(efficiency1.Max.Value - dateInterval.Min.Value)), dateInterval.Max.Value);

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_FixedEnd_OneZeroEfficiencyCalendarStartsInAndEndsAfterDateInterval_ReturnsCorrectResult()
        {
            //      +-----------+ 
            //               |--0--| 

            // Result:
            //  +---------------+ 
            //               |--0--|

            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(4));

            var efficiency1 = new DateIntervalEfficiency(now.AddDays(3), now.AddDays(5), 0);
            dateIntervalEfficiencies.Add(efficiency1);

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies, FixedEndPoint.Max);

            var correctDateInterval = new DateInterval(dateInterval.Min.Value.Add(-(dateInterval.Max.Value - efficiency1.Min.Value)), dateInterval.Max.Value);

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

    }
}
