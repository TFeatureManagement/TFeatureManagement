using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace TFeatureManagement.Tests
{
    [TestClass]
    public class SessionManagerExecutorTests
    {
        private SessionManagerExecutor<Feature, ISessionManager<Feature>> _underTest;

        private Mock<ISessionManager<Feature>> _sessionManager;
        private Mock<IFeatureEnumParser<Feature>> _featureEnumParser;

        [TestInitialize]
        public void Setup()
        {
            _sessionManager = new Mock<ISessionManager<Feature>>();
            _featureEnumParser = new Mock<IFeatureEnumParser<Feature>>();

            _underTest = new SessionManagerExecutor<Feature, ISessionManager<Feature>>(_sessionManager.Object, _featureEnumParser.Object);
        }

        [TestMethod]
        public void Constructor_SessionManagerIsNull_ThrowsArgumentNullException()
        {
            // Arrange and Act
            Action action = () => _underTest = new SessionManagerExecutor<Feature, ISessionManager<Feature>>(null, _featureEnumParser.Object);

            // Assert
            action.Should().Throw<ArgumentNullException>().Where(ex => ex.ParamName.Equals("sessionManager", StringComparison.Ordinal));
        }

        [TestMethod]
        public void Constructor_FeatureEnumParserIsNull_ThrowsArgumentNullException()
        {
            // Arrange and Act
            Action action = () => _underTest = new SessionManagerExecutor<Feature, ISessionManager<Feature>>(_sessionManager.Object, null);

            // Assert
            action.Should().Throw<ArgumentNullException>().Where(ex => ex.ParamName.Equals("featureEnumParser", StringComparison.Ordinal));
        }

        [TestMethod]
        public async Task GetAsync_FeatureNameSpecified_CallsFeatureEnumParserCorrectly()
        {
            // Arrange
            const string featureName = "NotInFeatureEnum";

            // Act
            await _underTest.GetAsync(featureName);

            // Assert
            var feature = default(Feature);
            _featureEnumParser.Verify(x => x.TryParse(featureName, true, out feature), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_FeatureNameNotInFeatureEnum_ReturnsNull()
        {
            // Arrange
            const string featureName = "NotInFeatureEnum";

            // Act
            var isEnabled = await _underTest.GetAsync(featureName);

            // Assert
            isEnabled.Should().BeNull();
        }

        [TestMethod]
        public async Task GetAsync_FeatureNameNotInFeatureEnum_DoesNotCallSessionManagerGetAsync()
        {
            // Arrange
            const string featureName = "NotInFeatureEnum";

            // Act
            var isEnabled = await _underTest.GetAsync(featureName);

            // Assert
            _sessionManager.Verify(x => x.GetAsync(It.IsAny<Feature>()), Times.Never);
        }

        [TestMethod]
        public async Task GetAsync_FeatureNameInFeatureEnum_CallsSessionManagerGetAsyncCorrectly()
        {
            // Arrange
            var feature = Feature.Test2;
            var featureName = feature.ToString();

            _featureEnumParser.Setup(x => x.TryParse(It.IsAny<string>(), It.IsAny<bool>(), out feature)).Returns(true);

            // Act
            await _underTest.GetAsync(featureName);

            // Assert
            _sessionManager.Verify(x => x.GetAsync(feature), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_FeatureNameInFeatureEnum_ReturnsFalseIfSessionManagerGetAsyncResultIsFalse()
        {
            // Arrange
            var feature = Feature.Test2;
            var featureName = feature.ToString();

            _featureEnumParser.Setup(x => x.TryParse(It.IsAny<string>(), It.IsAny<bool>(), out feature)).Returns(true);

            _sessionManager.Setup(x => x.GetAsync(It.IsAny<Feature>())).ReturnsAsync(false);

            // Act
            var isEnabled = await _underTest.GetAsync(featureName);

            // Assert
            isEnabled.Should().BeFalse();
        }

        [TestMethod]
        public async Task GetAsync_FeatureNameInFeatureEnum_ReturnsTrueIfSessionManagerGetAsyncResultIsTrue()
        {
            // Arrange
            var feature = Feature.Test2;
            var featureName = feature.ToString();

            _featureEnumParser.Setup(x => x.TryParse(It.IsAny<string>(), It.IsAny<bool>(), out feature)).Returns(true);

            _sessionManager.Setup(x => x.GetAsync(It.IsAny<Feature>())).ReturnsAsync(true);

            // Act
            var isEnabled = await _underTest.GetAsync(featureName);

            // Assert
            isEnabled.Should().BeTrue();
        }

        [TestMethod]
        public async Task GetAsync_FeatureNameInFeatureEnum_ReturnsNullIfSessionManagerGetAsyncResultIsNull()
        {
            // Arrange
            var feature = Feature.Test2;
            var featureName = feature.ToString();

            _featureEnumParser.Setup(x => x.TryParse(It.IsAny<string>(), It.IsAny<bool>(), out feature)).Returns(true);

            _sessionManager.Setup(x => x.GetAsync(It.IsAny<Feature>())).ReturnsAsync((bool?)null);

            // Act
            var isEnabled = await _underTest.GetAsync(featureName);

            // Assert
            isEnabled.Should().BeNull();
        }

        [TestMethod]
        public async Task SetAsync_FeatureNameSpecified_CallsFeatureEnumParserCorrectly()
        {
            // Arrange
            const string featureName = "NotInFeatureEnum";

            // Act
            await _underTest.SetAsync(featureName, false);

            // Assert
            var feature = default(Feature);
            _featureEnumParser.Verify(x => x.TryParse(featureName, true, out feature), Times.Once);
        }

        [TestMethod]
        public async Task SetAsync_FeatureNameNotInFeatureEnum_DoesNotCallSessionManagerSetAsync()
        {
            // Arrange
            var feature = Feature.Test2;
            var featureName = feature.ToString();

            _featureEnumParser.Setup(x => x.TryParse(It.IsAny<string>(), It.IsAny<bool>(), out feature)).Returns(false);

            // Act
            await _underTest.SetAsync(featureName, false);

            // Assert
            _sessionManager.Verify(x => x.SetAsync(It.IsAny<Feature>(), It.IsAny<bool>()), Times.Never);
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public async Task SetAsync_FeatureNameInFeatureEnum_CallSessionManagerSetAsyncCorrectly(bool isEnabled)
        {
            // Arrange
            var feature = Feature.Test2;
            var featureName = feature.ToString();

            _featureEnumParser.Setup(x => x.TryParse(It.IsAny<string>(), It.IsAny<bool>(), out feature)).Returns(true);

            // Act
            await _underTest.SetAsync(featureName, isEnabled);

            // Assert
            _sessionManager.Verify(x => x.SetAsync(feature, isEnabled), Times.Once);
        }
    }
}