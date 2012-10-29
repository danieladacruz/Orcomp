using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Orc.Entities;
using Orc.Extensions;

namespace NUnit
{
    [TestFixture]
    public class AccountForEfficienciesTest_SimpleFixedStart
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
        public void AccountForEfficiencies_EmptyListOfDateIntervalEfficiencies_ReturnsUnchangedDateInterval()
        {
            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(1));

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies);

            // Assert
            Assert.AreEqual(dateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_OneZeroEfficiencyCalendarStartAndEndWithDateInterval_ReturnsDateIntervalOfSameDurationWhichStartsWhenTheEfficiencyFinishes()
        {
            // +-----------+ 
            // |-----0-----| 

            // Result:
            // +-----------------------+ 
            // |-----0-----|


            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(1));
            var efficiency1 = new DateIntervalEfficiency(dateInterval, 0);
            dateIntervalEfficiencies.Add(efficiency1);

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies);

            var correctDateInterval = new DateInterval(dateInterval.Min.Value, efficiency1.Max.Value.Add(dateInterval.Duration));

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_OneZeroEfficiencyCalendarStartAndEndsBeforeDateIntervalStarts_ReturnsSameDateInterval()
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
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies);

            var correctDateInterval = dateInterval;

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_OneZeroEfficiencyCalendarStartAndEndsAfterDateIntervalEnds_ReturnsSameDateInterval()
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
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies);

            var correctDateInterval = dateInterval;

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_OneZeroEfficiencyCalendarStartAndEndsWithinDateInterval_ReturnsCorrectResult()
        {
            // +-----------+ 
            //    |--0--| 

            // Result:
            // +-----------------+
            //    |--0--|  

            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(4));
            var efficiency1 = new DateIntervalEfficiency(now.AddDays(1), now.AddDays(2), 0);
            dateIntervalEfficiencies.Add(efficiency1);

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies);

            var correctDateInterval = new DateInterval(dateInterval.Min.Value, dateInterval.Duration.Add(efficiency1.Duration));

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_OneZeroEfficiencyCalendarStartBeforeAndEndsWithinDateInterval_ReturnsCorrectResult()
        {
            //       +-----------+ 
            //    |--0--| 

            // Result:
            //       +--------------+ 
            //    |--0--|  

            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(4));
            var efficiency1 = new DateIntervalEfficiency(now.AddDays(-1), now.AddDays(2), 0);
            dateIntervalEfficiencies.Add(efficiency1);

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies);

            var correctDateInterval = new DateInterval(dateInterval.Min.Value, dateInterval.Duration.Add(efficiency1.Max.Value - dateInterval.Min.Value));

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_OneZeroEfficiencyCalendarStartsInAndEndsAfterDateInterval_ReturnsCorrectResult()
        {
            // +-----------+ 
            //          |--0--| 

            // Result:
            // +-----------------+ 
            //          |--0--|  

            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(4));
            var efficiency1 = new DateIntervalEfficiency(now.AddDays(3), now.AddDays(5), 0);
            dateIntervalEfficiencies.Add(efficiency1);

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies);

            var correctDateInterval = new DateInterval(dateInterval.Min.Value, dateInterval.Duration.Add(efficiency1.Duration));

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }
    }
}
