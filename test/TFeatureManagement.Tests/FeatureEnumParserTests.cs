using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TFeatureManagement.Tests
{
    [TestClass]
    public class FeatureEnumParserTests
    {
        private FeatureEnumParser<Feature> _underTest;

        [TestInitialize]
        public void Setup()
        {
            _underTest = new FeatureEnumParser<Feature>();
        }

        [TestMethod]
        [DataRow("NotInFeatureEnum", false)]
        [DataRow("NotInFeatureEnum", true)]
        [DataRow("8", false)]
        [DataRow("8", true)]
        public void TryParse_FeatureNameNotInFeatureEnum_ReturnsFalseAndDefaultValue(string featureName, bool ignoreCase)
        {
            // Arrange and Act
            var result = _underTest.TryParse(featureName, ignoreCase, out Feature feature);

            // Assert
            result.Should().BeFalse();
            feature.Should().Be(default(Feature));
        }

        [TestMethod]
        public void TryParse_FeatureNameInFeatureEnumButNotMatchingCaseAndIgnoreCaseIsFalse_ReturnsFalseAndDefaultValue()
        {
            // Arrange
            var featureName = nameof(Feature.Test2).ToLower();

            // Act
            var result = _underTest.TryParse(featureName, false, out Feature feature);

            // Assert
            result.Should().BeFalse();
            feature.Should().Be(default(Feature));
        }

        [TestMethod]
        public void TryParse_FeatureNameInFeatureEnumButNotMatchingCaseAndIgnoreCaseIsTrue_ReturnsTrueAndEquivalentValue()
        {
            // Arrange
            var featureName = nameof(Feature.Test2).ToLower();

            // Act
            var result = _underTest.TryParse(featureName, true, out Feature feature);

            // Assert
            result.Should().BeTrue();
            feature.Should().Be(Feature.Test2);
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void TryParse_FeatureNameInFeatureEnumAndMatchingCase_ReturnsTrueAndEquivalentValue(bool ignoreCase)
        {
            // Arrange
            var featureName = nameof(Feature.Test2);

            // Act
            var result = _underTest.TryParse(featureName, ignoreCase, out Feature feature);

            // Assert
            result.Should().BeTrue();
            feature.Should().Be(Feature.Test2);
        }
    }
}