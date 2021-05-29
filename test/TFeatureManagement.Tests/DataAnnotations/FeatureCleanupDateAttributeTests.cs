using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TFeatureManagement.DataAnnotations;

namespace TFeatureManagement.Tests.DataAnnotations
{
    [TestClass]
    public class FeatureCleanupDateAttributeTests
    {
        private FeatureCleanupDateAttribute _underTest;

        public void Constructor_SpecifiedYearMonthAndDayAreAValidDate_CleanupDateValueSetToStringRepresentationOfCleanupDate()
        {
            // Arrange
            var expectedCleanupDate = new DateTime(2000, 1, 1);
            _underTest = new FeatureCleanupDateAttribute(expectedCleanupDate.Year, expectedCleanupDate.Month, expectedCleanupDate.Day);

            // Act
            var cleanupDateValue = _underTest.CleanupDateValue;

            // Assert
            cleanupDateValue.Should().Be($"{expectedCleanupDate.Year}-{expectedCleanupDate.Month}-{expectedCleanupDate.Day}");
        }

        public void Constructor_SpecifiedYearMonthAndDayAreAValidDate_CleanupDateSetToDate()
        {
            // Arrange
            var expectedCleanupDate = new DateTime(2000, 1, 1);
            _underTest = new FeatureCleanupDateAttribute(expectedCleanupDate.Year, expectedCleanupDate.Month, expectedCleanupDate.Day);

            // Act
            var cleanupDate = _underTest.CleanupDate;

            // Assert
            cleanupDate.Should().BeSameDateAs(expectedCleanupDate.Date);
        }

        public void Constructor_SpecifiedYearMonthAndDayAreNotAValidDate_CleanupDateValueSetToStringRepresentationOfCleanupDate()
        {
            // Arrange
            _underTest = new FeatureCleanupDateAttribute(2000, 01, 99);

            // Act
            var cleanupDateValue = _underTest.CleanupDateValue;

            // Assert
            cleanupDateValue.Should().Be($"{2000}-{01}-{99}");
        }

        public void Constructor_SpecifiedYearMonthAndDayAreNotAValidDate_CleanupDateSetToNull()
        {
            // Arrange
            _underTest = new FeatureCleanupDateAttribute(2000, 01, 99);

            // Act
            var cleanupDate = _underTest.CleanupDate;

            // Assert
            cleanupDate.Should().BeNull();
        }
    }
}