using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TFeatureManagement.Tests;

[TestClass]
public class SessionManagerExecutorTests
{
    private SessionManagerExecutor<Feature, ISessionManager<Feature>> _underTest;

    private ISessionManager<Feature> _sessionManager;
    private IFeatureEnumParser<Feature> _featureEnumParser;

    [TestInitialize]
    public void Setup()
    {
        _sessionManager = Substitute.For<ISessionManager<Feature>>();
        _featureEnumParser = Substitute.For<IFeatureEnumParser<Feature>>();

        _underTest = new SessionManagerExecutor<Feature, ISessionManager<Feature>>(_sessionManager, _featureEnumParser);
    }

    [TestMethod]
    public void Constructor_SessionManagerIsNull_ThrowsArgumentNullException()
    {
        // Arrange and Act
        Action action = () => _underTest = new SessionManagerExecutor<Feature, ISessionManager<Feature>>(null, _featureEnumParser);

        // Assert
        action.Should().Throw<ArgumentNullException>().Where(ex => ex.ParamName.Equals("sessionManager", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Constructor_FeatureEnumParserIsNull_ThrowsArgumentNullException()
    {
        // Arrange and Act
        Action action = () => _underTest = new SessionManagerExecutor<Feature, ISessionManager<Feature>>(_sessionManager, null);

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
        _featureEnumParser.Received().TryParse(featureName, true, out _);
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
        await _underTest.GetAsync(featureName);

        // Assert
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        _sessionManager.DidNotReceive().GetAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [TestMethod]
    public async Task GetAsync_FeatureNameInFeatureEnum_CallsSessionManagerGetAsyncCorrectly()
    {
        // Arrange
        var feature = Feature.Test2;
        var featureName = feature.ToString();

        _featureEnumParser.TryParse(Arg.Any<string>(), Arg.Any<bool>(), out Arg.Any<Feature>()).Returns(x => { x[2] = feature; return true; });

        // Act
        await _underTest.GetAsync(featureName);

        // Assert
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        _sessionManager.Received().GetAsync(feature, Arg.Any<CancellationToken>());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [TestMethod]
    public async Task GetAsync_FeatureNameInFeatureEnum_ReturnsFalseIfSessionManagerGetAsyncResultIsFalse()
    {
        // Arrange
        var feature = Feature.Test2;
        var featureName = feature.ToString();

        _featureEnumParser.TryParse(Arg.Any<string>(), Arg.Any<bool>(), out Arg.Any<Feature>()).Returns(true);

        _sessionManager.GetAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>()).Returns(false);

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

        _featureEnumParser.TryParse(Arg.Any<string>(), Arg.Any<bool>(), out Arg.Any<Feature>()).Returns(true);

        _sessionManager.GetAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>()).Returns(true);

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

        _featureEnumParser.TryParse(Arg.Any<string>(), Arg.Any<bool>(), out Arg.Any<Feature>()).Returns(true);

        _sessionManager.GetAsync(Arg.Any<Feature>(), Arg.Any<CancellationToken>()).Returns((bool?)null);

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
        _featureEnumParser.Received().TryParse(featureName, true, out _);
    }

    [TestMethod]
    public async Task SetAsync_FeatureNameNotInFeatureEnum_DoesNotCallSessionManagerSetAsync()
    {
        // Arrange
        var feature = Feature.Test2;
        var featureName = feature.ToString();

        _featureEnumParser.TryParse(Arg.Any<string>(), Arg.Any<bool>(), out Arg.Any<Feature>()).Returns(false);

        // Act
        await _underTest.SetAsync(featureName, false);

        // Assert
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        _sessionManager.DidNotReceive().SetAsync(Arg.Any<Feature>(), Arg.Any<bool>(), Arg.Any<CancellationToken>());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task SetAsync_FeatureNameInFeatureEnum_CallSessionManagerSetAsyncCorrectly(bool isEnabled)
    {
        // Arrange
        var feature = Feature.Test2;
        var featureName = feature.ToString();

        _featureEnumParser.TryParse(Arg.Any<string>(), Arg.Any<bool>(), out Arg.Any<Feature>()).Returns(x => { x[2] = feature; return true; });

        // Act
        await _underTest.SetAsync(featureName, isEnabled);

        // Assert
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        _sessionManager.Received().SetAsync(feature, isEnabled, Arg.Any<CancellationToken>());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }
}