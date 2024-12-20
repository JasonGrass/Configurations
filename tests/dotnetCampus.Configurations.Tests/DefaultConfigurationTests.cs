using dotnetCampus.Configurations.Serializers;
using dotnetCampus.Configurations.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace dotnetCampus.Configurations.Tests
{
    [TestClass]
    public class DefaultConfigurationTests
    {
        [ContractTestCase]
        public void FromFile()
        {
            "从文件获取一个默认的配置，可以得到配置。".Test(() =>
            {
                // Arrange
                var coin = TestUtil.GetTempFile("configs.sim.coin");

                // Act
                var configs = DefaultConfiguration.FromFile(coin.FullName, new CoinConfigurationSerializer());

                // Assert
                var value = configs["Foo"];
                Assert.IsFalse(value.HasValue);
            });
        }
    }
}
