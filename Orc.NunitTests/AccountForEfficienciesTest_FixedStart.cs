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
    public class AccountForEfficienciesTest_FixedStart
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
        public void AccountForEfficiencies_MultipleDateIntervalEfficiencies_ReturnsCorrectAnswer()
        {
            // +------------+ 
            //    |0|  |0|  |0|

            // Result:
            // +-------------------+ 
            //    |0|  |0|  |0|

            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(5));

            var efficiency1 = new DateIntervalEfficiency(now.AddDays(1), now.AddDays(2), 0);
            var efficiency2 = new DateIntervalEfficiency(now.AddDays(3), now.AddDays(4), 0);
            var efficiency3 = new DateIntervalEfficiency(now.AddDays(5), now.AddDays(6), 0);

            dateIntervalEfficiencies.Add(efficiency1);
            dateIntervalEfficiencies.Add(efficiency2);
            dateIntervalEfficiencies.Add(efficiency3);


            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies);

            var correctDateInterval = new DateInterval(dateInterval.Min.Value, dateInterval.Duration.Add(efficiency1.Duration).Add(efficiency2.Duration).Add(efficiency3.Duration));

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

        [Test]
        public void AccountForEfficiencies_OneEfficiencyCalendarStartAndEndWithDateInterval_ReturnsCorrectAnswer()
        {
            // +-----------+ 
            // |----200----| 

            // Result:
            // +-----+ 
            // |----200----|  

            // Arrange
            var dateInterval = new DateInterval(now, now.AddDays(1));

            var efficiency1 = new DateIntervalEfficiency(dateInterval.Min.Value, dateInterval.Max.Value, 200);
            dateIntervalEfficiencies.Add(efficiency1);

            // Act
            var newDateInterval = dateInterval.AccountForEfficiencies(dateIntervalEfficiencies);
            var correctDateInterval = new DateInterval(dateInterval.Min.Value, dateInterval.Min.Value.AddTicks(dateInterval.Duration.Ticks / 2));

            // Assert
            Assert.AreEqual(correctDateInterval, newDateInterval);
        }

    }
}
