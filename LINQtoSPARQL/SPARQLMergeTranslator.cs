using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DynamicSPARQLSpace;
using HelperExtensionsLibrary.Strings;
using System.Reflection;
using System.Collections.ObjectModel;

namespace LINQtoSPARQLSpace
{
    /// <summary>
    /// Incapsulates functionality for marging of two expressions. Not synchronized;
    /// </summary>
    internal class SPARQLMergeTranslator
    {
        private static readonly string[] ClauseMethods = { "Delete", "Insert" };
        const string WhereClauseMethodName = "";
       // private string currClauseMethod = null;//where clause
        private Expression ExprFrom { get; set; }
        private Expression ExprTo { get; set; }
        private Expression TailExpression { get; set; }

        /// <summary>
        /// Merges expressions
        /// </summary>
        /// <param name="exprTo">source expression</param>
        /// <param name="exprFrom">target expression</param>
        /// <returns>Merged expression</returns>
        public Expression Merge(Expression exprTo, Expression exprFrom)
        {
            ExprFrom = exprFrom;
            ExprTo = exprTo;
            TailExpression = null;

            while (ExprFrom != null)
            {
                exprTo = MergeInternal(ExprFrom, ExprTo);

                if (TailExpression != null)
                {
                    exprTo = AddTail(ExprTo, TailExpression, exprTo);
                }
                ExprTo = exprTo;
                TailExpression = null;
            }

            return exprTo;
        }
        /// <summary>
        /// Merges expressions
        /// </summary>
        /// <param name="exprFrom">source expression</param>
        /// <param name="exprTo">target expression</param>
        /// <returns>Merged expression</returns>
        private Expression MergeInternal(Expression exprFrom, Expression exprTo)
        {
            bool found;
            var mcExprFrom = exprFrom as MethodCallExpression;

            if (mcExprFrom == null)
            {
                var nexprTo = FindMergeTargetExpression(exprTo, WhereClauseMethodName, out found);
                this.ExprFrom = null;
                return nexprTo;
            }

            var nextMethodCall = mcExprFrom.Arguments[0] as MethodCallExpression;

            if (ClauseMethods.Contains(mcExprFrom.Method.Name))
            {
                this.ExprFrom = nextMethodCall;
                exprTo = FindMergeTargetExpression(exprTo, mcExprFrom.Method.Name, out found);
                if (found)
                    return exprTo;
            }
            else
            {
                exprTo = MergeInternal(nextMethodCall, exprTo);
            }

            var result = mcExprFrom.Update(null, new[] { exprTo }.Concat(mcExprFrom.Arguments.Skip(1)));
            return result;
        }
        /// <summary>
        /// Finds merge target expression
        /// </summary>
        /// <param name="exprTo">Merge target source expression </param>
        /// <param name="methodName">Name of clause method</param>
        /// <param name="found">returns whethe clause method was found or not</param>
        /// <returns>Merge target expression</returns>
        private Expression FindMergeTargetExpression(Expression exprTo, string methodName, out bool found)
        {
            var result = FindClauseExpression(methodName, exprTo);

            if (result == null)
            {
                found = false;
                return exprTo;
            }

            TailExpression = result;
            found = true;
            return result;
        }
        /// <summary>
        /// Adds tail expression to target one
        /// </summary>
        /// <param name="exprFrom">source expression</param>
        /// <param name="exprTill">mark expression in source expression</param>
        /// <param name="exprTo">target expression</param>
        /// <returns>Expression with tail</returns>
        private Expression AddTail(Expression exprFrom, Expression exprTill, Expression exprTo)
        {
            if (exprFrom == exprTill)
                return exprTo;

            var mcExprFrom = exprFrom as MethodCallExpression;
            var result = AddTail(mcExprFrom.Arguments[0], exprTill, exprTo);

            return mcExprFrom.Update(null, new[] { result }.Concat(mcExprFrom.Arguments.Skip(1)));
        }



        /// <summary>
        /// Finds clause expression 
        /// </summary>
        /// <param name="methodName">Method to find</param>
        /// <param name="expr">Expression containing clause method</param>
        /// <returns>Clause expression</returns>
        private Expression FindClauseExpression(string methodName, Expression expr)
        {
            if (expr == null)
                return null;

            var mcExp = expr as MethodCallExpression;
            while (mcExp != null)
            {
                if (!ClauseMethods.Contains(mcExp.Method.Name))
                {
                    mcExp = mcExp.Arguments[0] as MethodCallExpression;
                    continue;
                }
                
                if (mcExp.Method.Name == methodName)
                {
                    return expr;
                }
                return FindClauseExpression(methodName, mcExp.Arguments[0]);
            }

            return methodName == WhereClauseMethodName ? expr : null;
        }
    }

}
