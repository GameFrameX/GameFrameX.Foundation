using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// EnvironmentHelper 单元测试：通过显式设置/清理环境变量，覆盖各 Is* 判断、Docker/K8s 检测与 GetEnvironmentName。
    /// </summary>
    public class EnvironmentHelperTests : IDisposable
    {
        private const string AspNetEnvKey = "ASPNETCORE_ENVIRONMENT";
        private const string DotnetEnvKey = "DOTNET_ENVIRONMENT";
        private const string DockerKey = "DOTNET_RUNNING_IN_CONTAINER";
        private const string K8sKey = "KUBERNETES_SERVICE_HOST";

        private readonly string _originalAspNet;
        private readonly string _originalDotnet;
        private readonly string _originalDocker;
        private readonly string _originalK8s;

        public EnvironmentHelperTests()
        {
            _originalAspNet = Environment.GetEnvironmentVariable(AspNetEnvKey);
            _originalDotnet = Environment.GetEnvironmentVariable(DotnetEnvKey);
            _originalDocker = Environment.GetEnvironmentVariable(DockerKey);
            _originalK8s = Environment.GetEnvironmentVariable(K8sKey);
        }

        public void Dispose()
        {
            RestoreEnvironmentVariable(AspNetEnvKey, _originalAspNet);
            RestoreEnvironmentVariable(DotnetEnvKey, _originalDotnet);
            RestoreEnvironmentVariable(DockerKey, _originalDocker);
            RestoreEnvironmentVariable(K8sKey, _originalK8s);
        }

        private static void RestoreEnvironmentVariable(string key, string value)
        {
            if (value == null)
            {
                Environment.SetEnvironmentVariable(key, null);
            }
            else
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }

        private static void ClearAllEnvVars()
        {
            Environment.SetEnvironmentVariable(AspNetEnvKey, null);
            Environment.SetEnvironmentVariable(DotnetEnvKey, null);
        }

        #region IsDevelopment

        [Theory]
        [InlineData("Development")]
        [InlineData("development")]
        [InlineData("DEVELOPMENT")]
        public void IsDevelopment_WhenAspNetCoreEnvMatches_ShouldReturnTrue(string envValue)
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, envValue);

            Assert.True(EnvironmentHelper.IsDevelopment());
        }

        [Fact]
        public void IsDevelopment_WhenOnlyDotnetEnvMatches_ShouldReturnTrue()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(DotnetEnvKey, "Development");

            Assert.True(EnvironmentHelper.IsDevelopment());
        }

        [Fact]
        public void IsDevelopment_WhenAspNetCoreEnvTakesPrecedenceOverDotnet_ShouldUseAspNetCore()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, "Production");
            Environment.SetEnvironmentVariable(DotnetEnvKey, "Development");

            // ASPNETCORE 优先，所以这里应返回 false
            Assert.False(EnvironmentHelper.IsDevelopment());
        }

        [Fact]
        public void IsDevelopment_WhenNoEnvSet_ShouldReturnFalse()
        {
            ClearAllEnvVars();

            Assert.False(EnvironmentHelper.IsDevelopment());
        }

        [Fact]
        public void IsDevelopment_WhenEnvIsStaging_ShouldReturnFalse()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, "Staging");

            Assert.False(EnvironmentHelper.IsDevelopment());
        }

        #endregion

        #region IsProduction

        [Theory]
        [InlineData("Production")]
        [InlineData("production")]
        [InlineData("PRODUCTION")]
        public void IsProduction_WhenEnvMatches_ShouldReturnTrue(string envValue)
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, envValue);

            Assert.True(EnvironmentHelper.IsProduction());
        }

        [Fact]
        public void IsProduction_WhenOnlyDotnetEnvMatches_ShouldReturnTrue()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(DotnetEnvKey, "Production");

            Assert.True(EnvironmentHelper.IsProduction());
        }

        [Fact]
        public void IsProduction_WhenNoEnvSet_ShouldReturnFalse()
        {
            ClearAllEnvVars();

            Assert.False(EnvironmentHelper.IsProduction());
        }

        #endregion

        #region IsStaging

        [Theory]
        [InlineData("Staging")]
        [InlineData("staging")]
        [InlineData("STAGING")]
        public void IsStaging_WhenEnvMatches_ShouldReturnTrue(string envValue)
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, envValue);

            Assert.True(EnvironmentHelper.IsStaging());
        }

        [Fact]
        public void IsStaging_WhenEnvIsDevelopment_ShouldReturnFalse()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, "Development");

            Assert.False(EnvironmentHelper.IsStaging());
        }

        [Fact]
        public void IsStaging_WhenNoEnvSet_ShouldReturnFalse()
        {
            ClearAllEnvVars();

            Assert.False(EnvironmentHelper.IsStaging());
        }

        #endregion

        #region IsEnvironment (自定义)

        [Fact]
        public void IsEnvironment_WhenCustomNameMatches_ShouldReturnTrue()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, "QA");

            Assert.True(EnvironmentHelper.IsEnvironment("QA"));
        }

        [Fact]
        public void IsEnvironment_ShouldBeCaseInsensitive()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, "qa");

            Assert.True(EnvironmentHelper.IsEnvironment("QA"));
            Assert.True(EnvironmentHelper.IsEnvironment("qa"));
            Assert.True(EnvironmentHelper.IsEnvironment("Qa"));
        }

        [Fact]
        public void IsEnvironment_WhenNoMatch_ShouldReturnFalse()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, "Production");

            Assert.False(EnvironmentHelper.IsEnvironment("Development"));
        }

        [Fact]
        public void IsEnvironment_WhenNoEnvSet_ShouldReturnFalse()
        {
            ClearAllEnvVars();

            Assert.False(EnvironmentHelper.IsEnvironment("Production"));
        }

        #endregion

        #region IsDocker

        [Fact]
        public void IsDocker_WhenDotnetRunningInContainerSet_ShouldReturnTrue()
        {
            Environment.SetEnvironmentVariable(DockerKey, "true");

            Assert.True(EnvironmentHelper.IsDocker());
        }

        [Fact]
        public void IsDocker_WhenDotnetRunningInContainerIsNotEmptyString_ShouldReturnTrue()
        {
            Environment.SetEnvironmentVariable(DockerKey, "1");

            Assert.True(EnvironmentHelper.IsDocker());
        }

        [Fact]
        public void IsDocker_WhenDotnetRunningInContainerIsEmpty_ShouldReturnFalse()
        {
            Environment.SetEnvironmentVariable(DockerKey, string.Empty);

            Assert.False(EnvironmentHelper.IsDocker());
        }

        [Fact]
        public void IsDocker_WhenDotnetRunningInContainerIsNull_ShouldReturnFalse()
        {
            Environment.SetEnvironmentVariable(DockerKey, null);

            Assert.False(EnvironmentHelper.IsDocker());
        }

        #endregion

        #region IsKubernetes

        [Fact]
        public void IsKubernetes_WhenServiceHostSet_ShouldReturnTrue()
        {
            Environment.SetEnvironmentVariable(K8sKey, "10.0.0.1");

            Assert.True(EnvironmentHelper.IsKubernetes());
        }

        [Fact]
        public void IsKubernetes_WhenServiceHostIsEmpty_ShouldReturnFalse()
        {
            Environment.SetEnvironmentVariable(K8sKey, string.Empty);

            Assert.False(EnvironmentHelper.IsKubernetes());
        }

        [Fact]
        public void IsKubernetes_WhenServiceHostIsNull_ShouldReturnFalse()
        {
            Environment.SetEnvironmentVariable(K8sKey, null);

            Assert.False(EnvironmentHelper.IsKubernetes());
        }

        #endregion

        #region GetEnvironmentName

        [Fact]
        public void GetEnvironmentName_WhenAspNetCoreSet_ShouldReturnItsValue()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, "Staging");

            Assert.Equal("Staging", EnvironmentHelper.GetEnvironmentName());
        }

        [Fact]
        public void GetEnvironmentName_WhenOnlyDotnetSet_ShouldReturnItsValue()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(DotnetEnvKey, "Development");

            Assert.Equal("Development", EnvironmentHelper.GetEnvironmentName());
        }

        [Fact]
        public void GetEnvironmentName_WhenAspNetCoreTakesPrecedence_ShouldReturnAspNetCore()
        {
            ClearAllEnvVars();
            Environment.SetEnvironmentVariable(AspNetEnvKey, "Production");
            Environment.SetEnvironmentVariable(DotnetEnvKey, "Development");

            Assert.Equal("Production", EnvironmentHelper.GetEnvironmentName());
        }

        [Fact]
        public void GetEnvironmentName_WhenNeitherSet_ShouldReturnNull()
        {
            ClearAllEnvVars();

            Assert.Null(EnvironmentHelper.GetEnvironmentName());
        }

        #endregion
    }
}
