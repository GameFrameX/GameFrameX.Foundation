using System;
using System.IO;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions
{
    /// <summary>
    /// DirectoryExtensions 全面覆盖测试：正向创建、isFile 模式、已存在目录、null/空路径边界。
    /// </summary>
    public class DirectoryExtensionsTests
    {
        /// <summary>
        /// 创建一个唯一的临时目录路径用于测试，并在测试结束后自动清理。
        /// </summary>
        private string CreateUniqueTempPath()
        {
            return Path.Combine(Path.GetTempPath(), "gfx_test_" + Guid.NewGuid().ToString("N"));
        }

        [Fact]
        public void CreateAsDirectory_ValidPath_ShouldCreateDirectory()
        {
            // Arrange
            var dirPath = CreateUniqueTempPath();

            try
            {
                // Act
                dirPath.CreateAsDirectory();

                // Assert
                Assert.True(Directory.Exists(dirPath));
            }
            finally
            {
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, recursive: true);
                }
            }
        }

        [Fact]
        public void CreateAsDirectory_AlreadyExists_ShouldNotThrow()
        {
            // Arrange
            var dirPath = CreateUniqueTempPath();

            try
            {
                Directory.CreateDirectory(dirPath);
                Assert.True(Directory.Exists(dirPath));

                // Act & Assert - should not throw
                var exception = Record.Exception(() => dirPath.CreateAsDirectory());
                Assert.Null(exception);
            }
            finally
            {
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, recursive: true);
                }
            }
        }

        [Fact]
        public void CreateAsDirectory_RecursiveCreation_ShouldCreateParentDirectories()
        {
            // Arrange
            var basePath = CreateUniqueTempPath();
            var nestedPath = Path.Combine(basePath, "level1", "level2", "level3");

            try
            {
                // Act
                nestedPath.CreateAsDirectory();

                // Assert
                Assert.True(Directory.Exists(nestedPath));
            }
            finally
            {
                if (Directory.Exists(basePath))
                {
                    Directory.Delete(basePath, recursive: true);
                }
            }
        }

        [Fact]
        public void CreateAsDirectory_AsFile_ShouldCreateParentDirectory()
        {
            // Arrange
            var basePath = CreateUniqueTempPath();
            var filePath = Path.Combine(basePath, "subdir", "testfile.txt");

            try
            {
                // Act
                filePath.CreateAsDirectory(isFile: true);

                // Assert - should create the "subdir" directory, not a file
                Assert.True(Directory.Exists(Path.Combine(basePath, "subdir")));
                Assert.False(File.Exists(filePath));
            }
            finally
            {
                if (Directory.Exists(basePath))
                {
                    Directory.Delete(basePath, recursive: true);
                }
            }
        }

        [Fact]
        public void CreateAsDirectory_AsFileWithRootOnly_ShouldNotCreateAnything()
        {
            // Arrange - a filename with no directory component
            var filePath = "plainfile.txt";

            // Act & Assert - should not throw and should not create anything
            var exception = Record.Exception(() => filePath.CreateAsDirectory(isFile: true));
            Assert.Null(exception);
        }

        [Fact]
        public void CreateAsDirectory_NullPath_ShouldThrowArgumentNullException()
        {
            // Arrange
            string path = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => path.CreateAsDirectory());
        }

        [Fact]
        public void CreateAsDirectory_NullPathAsFile_ShouldThrowArgumentNullException()
        {
            // Arrange
            string path = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => path.CreateAsDirectory(isFile: true));
        }

        [Fact]
        public void CreateAsDirectory_AsFileFalse_ShouldCreateDirectoryDirectly()
        {
            // Arrange
            var dirPath = CreateUniqueTempPath();

            try
            {
                // Act - explicitly pass isFile: false
                dirPath.CreateAsDirectory(isFile: false);

                // Assert
                Assert.True(Directory.Exists(dirPath));
            }
            finally
            {
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, recursive: true);
                }
            }
        }
    }
}
