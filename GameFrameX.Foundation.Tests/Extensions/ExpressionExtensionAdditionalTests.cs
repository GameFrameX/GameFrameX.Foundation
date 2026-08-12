using System;
using System.Linq.Expressions;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions
{
    /// <summary>
    /// ExpressionExtension 补充覆盖测试：多类型表达式组合、Not 链式、AndIf/OrIf null 参数组合。
    /// </summary>
    public class ExpressionExtensionAdditionalTests
    {
        // ============================================================
        // And / Or / Not 语义正确性（数值类型）
        // ============================================================

        [Fact]
        public void And_WithNumericPredicates_ShouldCombineCorrectly()
        {
            // Arrange
            Expression<Func<int, bool>> left = x => x > 0;
            Expression<Func<int, bool>> right = x => x < 100;

            // Act
            var combined = left.And(right);
            var compiled = combined.Compile();

            // Assert
            Assert.True(compiled(50));
            Assert.False(compiled(0));
            Assert.False(compiled(100));
            Assert.False(compiled(-1));
        }

        [Fact]
        public void Or_WithNumericPredicates_ShouldCombineCorrectly()
        {
            // Arrange
            Expression<Func<int, bool>> left = x => x < 0;
            Expression<Func<int, bool>> right = x => x > 100;

            // Act
            var combined = left.Or(right);
            var compiled = combined.Compile();

            // Assert
            Assert.True(compiled(-1));
            Assert.True(compiled(101));
            Assert.False(compiled(50));
        }

        [Fact]
        public void Not_ShouldInvertCompoundExpression()
        {
            // Arrange
            Expression<Func<int, bool>> expr = x => x > 10 && x < 20;

            // Act
            var notExpr = expr.Not();
            var compiled = notExpr.Compile();

            // Assert
            Assert.False(compiled(15));
            Assert.True(compiled(5));
            Assert.True(compiled(25));
        }

        // ============================================================
        // AndIf / OrIf 条件分支
        // ============================================================

        [Fact]
        public void AndIf_ConditionFalse_ShouldReturnLeftExpressionUnchanged()
        {
            // Arrange
            Expression<Func<int, bool>> left = x => x > 0;
            Expression<Func<int, bool>> right = x => x < 100;

            // Act
            var result = left.AndIf(() => false, right);
            var compiled = result.Compile();

            // Assert - only left expression applies
            Assert.True(compiled(200));
            Assert.False(compiled(-1));
        }

        [Fact]
        public void OrIf_ConditionFalse_ShouldReturnLeftExpressionUnchanged()
        {
            // Arrange
            Expression<Func<int, bool>> left = x => x > 100;
            Expression<Func<int, bool>> right = x => x < 0;

            // Act
            var result = left.OrIf(() => false, right);
            var compiled = result.Compile();

            // Assert - only left expression applies
            Assert.True(compiled(200));
            Assert.False(compiled(50));
        }

        [Fact]
        public void AndIf_ConditionTrue_ShouldCombineBoth()
        {
            // Arrange
            Expression<Func<int, bool>> left = x => x > 0;
            Expression<Func<int, bool>> right = x => x < 100;

            // Act
            var result = left.AndIf(() => true, right);
            var compiled = result.Compile();

            // Assert
            Assert.True(compiled(50));
            Assert.False(compiled(200));
        }

        [Fact]
        public void OrIf_ConditionTrue_ShouldCombineBoth()
        {
            // Arrange
            Expression<Func<int, bool>> left = x => x > 100;
            Expression<Func<int, bool>> right = x => x < 0;

            // Act
            var result = left.OrIf(() => true, right);
            var compiled = result.Compile();

            // Assert
            Assert.True(compiled(-1));
            Assert.True(compiled(200));
            Assert.False(compiled(50));
        }

        // ============================================================
        // Null 参数组合
        // ============================================================

        [Fact]
        public void AndIf_NullLeftExpression_ShouldThrowArgumentNullException()
        {
            // Arrange
            Expression<Func<int, bool>> left = null;
            Expression<Func<int, bool>> right = x => x > 0;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left.AndIf(() => true, right));
        }

        [Fact]
        public void OrIf_NullLeftExpression_ShouldThrowArgumentNullException()
        {
            // Arrange
            Expression<Func<int, bool>> left = null;
            Expression<Func<int, bool>> right = x => x > 0;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => left.OrIf(() => true, right));
        }

        // ============================================================
        // 链式组合
        // ============================================================

        [Fact]
        public void And_ThenOr_Chained_ShouldEvaluateCorrectly()
        {
            // Arrange
            Expression<Func<int, bool>> expr1 = x => x > 0;
            Expression<Func<int, bool>> expr2 = x => x < 100;
            Expression<Func<int, bool>> expr3 = x => x == 50;

            // Act - (x > 0 AND x < 100) OR (x == 50)
            var combined = expr1.And(expr2).Or(expr3);
            var compiled = combined.Compile();

            // Assert
            Assert.True(compiled(50));   // matches all
            Assert.True(compiled(99));   // matches first two
            Assert.False(compiled(0));   // matches none
            Assert.False(compiled(200)); // matches none
        }

        [Fact]
        public void MultipleNot_ShouldRestoreOriginalSemantics()
        {
            // Arrange
            Expression<Func<int, bool>> expr = x => x > 5;

            // Act - double negation
            var doubleNot = expr.Not().Not();
            var compiled = doubleNot.Compile();

            // Assert - same as original
            Assert.True(compiled(10));
            Assert.False(compiled(3));
        }
    }
}
