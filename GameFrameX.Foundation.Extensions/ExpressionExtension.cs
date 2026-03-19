// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.Linq.Expressions;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供对 <see cref="Expression" /> 类型的扩展方法，用于组合和操作表达式树。
/// </summary>
/// <remarks>
/// Provides extension methods for the <see cref="Expression" /> type, used for combining and manipulating expression trees.
/// </remarks>
public static class ExpressionExtension
{
    /// <summary>
    /// 将两个表达式进行逻辑与运算，使用短路求值。
    /// </summary>
    /// <remarks>
    /// Performs a logical AND operation on two expressions using short-circuit evaluation.
    /// </remarks>
    /// <typeparam name="T">表达式的参数类型 / The parameter type of the expression.</typeparam>
    /// <param name="leftExpression">第一个表达式，作为逻辑与运算的左操作数 / The first expression, serving as the left operand of the logical AND operation.</param>
    /// <param name="rightExpression">第二个表达式，作为逻辑与运算的右操作数 / The second expression, serving as the right operand of the logical AND operation.</param>
    /// <returns>一个新的表达式，表示两个输入表达式的逻辑与运算结果 / A new expression representing the logical AND result of the two input expressions.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="leftExpression"/> 或 <paramref name="rightExpression"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="leftExpression"/> or <paramref name="rightExpression"/> is <c>null</c>.</exception>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
    {
        ArgumentNullException.ThrowIfNull(leftExpression, nameof(leftExpression));
        ArgumentNullException.ThrowIfNull(rightExpression, nameof(rightExpression));
        var newParameter = Expression.Parameter(typeof(T), nameof(And));
        var visitor = new CustomExpressionVisitor(newParameter);
        var left = visitor.Visit(leftExpression.Body);
        var right = visitor.Visit(rightExpression.Body);
        var body = Expression.AndAlso(left, right); // 使用AndAlso替代And以支持短路求值
        return Expression.Lambda<Func<T, bool>>(body, newParameter);
    }

    /// <summary>
    /// 根据条件将两个表达式进行逻辑与运算，使用短路求值。
    /// 当条件为 false 时，仅返回左表达式；当条件为 true 时，返回两个表达式的逻辑与运算结果。
    /// </summary>
    /// <remarks>
    /// Performs a logical AND operation on two expressions based on a condition, using short-circuit evaluation.
    /// When the condition is false, only the left expression is returned; when the condition is true, the logical AND result of both expressions is returned.
    /// </remarks>
    /// <typeparam name="T">表达式的参数类型 / The parameter type of the expression.</typeparam>
    /// <param name="leftExpression">第一个表达式，作为逻辑与运算的左操作数 / The first expression, serving as the left operand of the logical AND operation.</param>
    /// <param name="condition">决定是否执行逻辑与运算的条件委托 / The condition delegate that determines whether to perform the logical AND operation.</param>
    /// <param name="rightExpression">第二个表达式，作为逻辑与运算的右操作数 / The second expression, serving as the right operand of the logical AND operation.</param>
    /// <returns>当条件为 <c>true</c> 时返回两个表达式的逻辑与运算结果，否则返回左表达式 / Returns the logical AND result of both expressions when the condition is <c>true</c>; otherwise, returns the left expression.</returns>
    /// <exception cref="ArgumentNullException">当任何参数为 <c>null</c> 时抛出 / Thrown when any parameter is <c>null</c>.</exception>
    public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> leftExpression, Func<bool> condition, Expression<Func<T, bool>> rightExpression)
    {
        ArgumentNullException.ThrowIfNull(leftExpression, nameof(leftExpression));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));
        ArgumentNullException.ThrowIfNull(rightExpression, nameof(rightExpression));

        if (!condition())
        {
            return leftExpression;
        }

        var newParameter = Expression.Parameter(typeof(T), nameof(AndIf));
        var visitor = new CustomExpressionVisitor(newParameter);
        var left = visitor.Visit(leftExpression.Body);
        var right = visitor.Visit(rightExpression.Body);
        var body = Expression.AndAlso(left, right); // 使用AndAlso替代And以支持短路求值
        return Expression.Lambda<Func<T, bool>>(body, newParameter);
    }


    /// <summary>
    /// 将两个表达式进行逻辑或运算，使用短路求值。
    /// </summary>
    /// <remarks>
    /// Performs a logical OR operation on two expressions using short-circuit evaluation.
    /// </remarks>
    /// <typeparam name="T">表达式的参数类型 / The parameter type of the expression.</typeparam>
    /// <param name="leftExpression">第一个表达式，作为逻辑或运算的左操作数 / The first expression, serving as the left operand of the logical OR operation.</param>
    /// <param name="rightExpression">第二个表达式，作为逻辑或运算的右操作数 / The second expression, serving as the right operand of the logical OR operation.</param>
    /// <returns>一个新的表达式，表示两个输入表达式的逻辑或运算结果 / A new expression representing the logical OR result of the two input expressions.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="leftExpression"/> 或 <paramref name="rightExpression"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="leftExpression"/> or <paramref name="rightExpression"/> is <c>null</c>.</exception>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
    {
        ArgumentNullException.ThrowIfNull(leftExpression, nameof(leftExpression));
        ArgumentNullException.ThrowIfNull(rightExpression, nameof(rightExpression));

        var newParameter = Expression.Parameter(typeof(T), nameof(Or));
        var visitor = new CustomExpressionVisitor(newParameter);
        var left = visitor.Visit(leftExpression.Body);
        var right = visitor.Visit(rightExpression.Body);
        var body = Expression.OrElse(left, right); // 使用OrElse替代Or以支持短路求值
        return Expression.Lambda<Func<T, bool>>(body, newParameter);
    }

    /// <summary>
    /// 根据条件将两个表达式进行逻辑或运算，使用短路求值。
    /// 当条件为 false 时，仅返回左表达式；当条件为 true 时，返回两个表达式的逻辑或运算结果。
    /// </summary>
    /// <remarks>
    /// Performs a logical OR operation on two expressions based on a condition, using short-circuit evaluation.
    /// When the condition is false, only the left expression is returned; when the condition is true, the logical OR result of both expressions is returned.
    /// </remarks>
    /// <typeparam name="T">表达式的参数类型 / The parameter type of the expression.</typeparam>
    /// <param name="leftExpression">第一个表达式，作为逻辑或运算的左操作数 / The first expression, serving as the left operand of the logical OR operation.</param>
    /// <param name="condition">决定是否执行逻辑或运算的条件委托 / The condition delegate that determines whether to perform the logical OR operation.</param>
    /// <param name="rightExpression">第二个表达式，作为逻辑或运算的右操作数 / The second expression, serving as the right operand of the logical OR operation.</param>
    /// <returns>当条件为 <c>true</c> 时返回两个表达式的逻辑或运算结果，否则返回左表达式 / Returns the logical OR result of both expressions when the condition is <c>true</c>; otherwise, returns the left expression.</returns>
    /// <exception cref="ArgumentNullException">当任何参数为 <c>null</c> 时抛出 / Thrown when any parameter is <c>null</c>.</exception>
    public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> leftExpression, Func<bool> condition, Expression<Func<T, bool>> rightExpression)
    {
        ArgumentNullException.ThrowIfNull(leftExpression, nameof(leftExpression));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));
        ArgumentNullException.ThrowIfNull(rightExpression, nameof(rightExpression));

        if (!condition())
        {
            return leftExpression;
        }

        var newParameter = Expression.Parameter(typeof(T), nameof(OrIf));
        var visitor = new CustomExpressionVisitor(newParameter);
        var left = visitor.Visit(leftExpression.Body);
        var right = visitor.Visit(rightExpression.Body);
        var body = Expression.OrElse(left, right); // 使用OrElse替代Or以支持短路求值
        return Expression.Lambda<Func<T, bool>>(body, newParameter);
    }

    /// <summary>
    /// 对表达式进行逻辑非运算，对表达式的结果取反。
    /// </summary>
    /// <remarks>
    /// Performs a logical NOT operation on an expression, negating the result of the expression.
    /// If the input expression is x => x > 5, the output expression is x => !(x > 5), equivalent to x => x &lt;= 5.
    /// </remarks>
    /// <typeparam name="T">表达式的参数类型 / The parameter type of the expression.</typeparam>
    /// <param name="expr">要进行逻辑非运算的表达式 / The expression to perform the logical NOT operation on.</param>
    /// <returns>一个新的表达式，表示输入表达式的逻辑非运算结果 / A new expression representing the logical NOT result of the input expression.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="expr"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="expr"/> is <c>null</c>.</exception>
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
    {
        ArgumentNullException.ThrowIfNull(expr, nameof(expr));

        var newParameter = expr.Parameters[0];
        var body = Expression.Not(expr.Body);
        return Expression.Lambda<Func<T, bool>>(body, newParameter);
    }

    /// <summary>
    /// 表达式访问器的自定义实现。
    /// </summary>
    /// <remarks>
    /// Custom implementation of the expression visitor.
    /// </remarks>
    internal sealed class CustomExpressionVisitor : ExpressionVisitor
    {
        private ParameterExpression _targetParameter;

        /// <summary>
        /// 初始化 <see cref="CustomExpressionVisitor" /> 类的新实例。
        /// </summary>
        /// <remarks>
        /// Initializes a new instance of the <see cref="CustomExpressionVisitor" /> class.
        /// </remarks>
        /// <param name="param">访问器中的参数表达式，不能为 <c>null</c> / The parameter expression in the visitor, cannot be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">当 <paramref name="param"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="param"/> is <c>null</c>.</exception>
        public CustomExpressionVisitor(ParameterExpression param)
        {
            ArgumentNullException.ThrowIfNull(param, nameof(param));
            Parameter = param;
        }

        /// <summary>
        /// 获取或设置访问器中的参数表达式。
        /// </summary>
        /// <remarks>
        /// Gets or sets the parameter expression in the visitor.
        /// </remarks>
        /// <value>参数表达式 / The parameter expression.</value>
        public ParameterExpression Parameter { get; }

        /// <summary>
        /// 访问 Lambda 表达式，正确处理参数替换。
        /// </summary>
        /// <remarks>
        /// Visits the lambda expression, correctly handling parameter substitution.
        /// </remarks>
        /// <typeparam name="T">Lambda 表达式的委托类型 / The delegate type of the lambda expression.</typeparam>
        /// <param name="node">要访问的 Lambda 表达式 / The lambda expression to visit.</param>
        /// <returns>返回访问后的 Lambda 表达式 / Returns the visited lambda expression.</returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            ArgumentNullException.ThrowIfNull(node, nameof(node));

            // 记录要替换的目标参数（第一个参数）
            var originalTarget = _targetParameter;
            _targetParameter = node.Parameters.Count > 0 ? node.Parameters[0] : null;

            try
            {
                // 如果Lambda表达式只有一个参数，则替换它
                if (node.Parameters.Count == 1)
                {
                    var newParameters = new[] { Parameter };
                    var newBody = Visit(node.Body);
                    return Expression.Lambda<T>(newBody, newParameters);
                }

                // 如果有多个参数，只替换第一个参数
                if (node.Parameters.Count > 1)
                {
                    var newParameters = new ParameterExpression[node.Parameters.Count];
                    newParameters[0] = Parameter;
                    for (int i = 1; i < node.Parameters.Count; i++)
                    {
                        newParameters[i] = node.Parameters[i];
                    }

                    var newBody = Visit(node.Body);
                    return Expression.Lambda<T>(newBody, newParameters);
                }

                return base.VisitLambda(node);
            }
            finally
            {
                // 恢复原来的目标参数
                _targetParameter = originalTarget;
            }
        }

        /// <summary>
        /// 访问参数表达式。
        /// </summary>
        /// <remarks>
        /// Visits the parameter expression.
        /// </remarks>
        /// <param name="node">要访问的参数表达式 / The parameter expression to visit.</param>
        /// <returns>返回访问后的表达式 / Returns the visited expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            ArgumentNullException.ThrowIfNull(node, nameof(node));

            // 如果在Lambda上下文中，只替换目标参数
            if (_targetParameter != null)
            {
                return ReferenceEquals(node, _targetParameter) ? Parameter : node;
            }

            // 如果不在Lambda上下文中，总是返回构造函数中的参数（保持原有行为）
            return Parameter;
        }
    }
}