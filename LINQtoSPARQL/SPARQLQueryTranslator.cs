﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DynamicSPARQLSpace;
using HelperExtensionsLibrary.Strings;
using System.Reflection;

namespace LINQtoSPARQLSpace
{
    internal class SPARQLQueryTranslator : ExpressionVisitor
    {
        private LinkedList<IList<IWhereItem>> Groups {get; set;}
        private readonly string[] GroupingMethods = {"Optional", "Group", "Either", "OR"};

        public Group WhereClause { get; private set; }
        public string SelectClause { get; private set; }
        public string OrderByClause { get; private set; }
        public int LimitClause { get; private set; }
        public string Prefixes { get; private set; }
        public string GroupByClause { get; private set; }
        public string HavingClause { get; private set; }
        public string OffsetClause { get; private set; }

        internal SPARQLQueryTranslator()
        {
        }

        internal void Translate(Expression expression)
        {
            Groups = new LinkedList<IList<IWhereItem>>();
            Groups.AddLast(new Group() { Items = new List<IWhereItem>() });
            WhereClause = null;
            SelectClause = null;
            this.Visit(expression);
            WhereClause = (Group)Groups.Last.Value;
            if (SelectClause != null)
                return;

            SelectClause = GetSelectByTypeArgument(((MethodCallExpression)expression).Method.GetGenericArguments()[0]);

        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Cast")
            {
                this.Visit(m.Arguments[0]);
                return m;
            }

            if (m.Method.DeclaringType != typeof(LINQtoSPARQLExtensions))
                throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));


            VisitSPARQL(m); 

            return m;

        }

        private void VisitSPARQL(MethodCallExpression m, int level = 0)
        {

            if (m.Arguments == null || m.Arguments.Count == 0)
                return;
            
            var prevMethod = m.Arguments[0] as MethodCallExpression;

            IWhereItem ret = null;
            string name = m.Method.Name;

            int prevItemLevel = level;

            if (name == "End")
            {
                prevItemLevel++;
            }
            else if (level > 0 && GroupingMethods.Any(n=>n==name))
            {
                prevItemLevel--;
            }

            if (prevMethod != null)
            {
                VisitSPARQL(prevMethod,  
                    (name == "OR" && prevMethod.Method.Name != "End") 
                     ? prevItemLevel+1 : prevItemLevel);
            }

            var list = Groups.Last.Value;
            
            if (name == "End")
            {
                Groups.RemoveLast();
                ret = null;
            }
            
            else if (name == "Group")
            {
                ret = VisitGroup(m);
            }
                       
            //"Match" or "And" with three parameters
            else if (name == "Match")
            {
                ret = VisitMatch(m);
            }


            //"And" with one parameter 
            else if (name == "And_1")
            {
                ret = VisitAnd1(m, list);
            }
            //"And" with two parameters
            else if (name == "And_2")
            {
                ret = VisitAnd2(m, list);
            }

            else if (name == "Optional")
            {
                ret = VisitOptional(m); 
                
            }

            else if (name == "FilterBy")
            {
                 ret = VisitFilterBy(m);
            }

            else if (name == "Either")
            {
                ret = VisitEither(m);
            }
            
            else if (name == "OR")
            {
                if (prevMethod.Method.Name != "End")
                    Groups.RemoveLast();
           
                list = Groups.Last.Value;
                Groups.RemoveLast();
               
                ret = VisitOR(m, list);
            }

            else if (name == "Select")
            {
                SelectClause = VisitSelect(m);
            }

            else if (name == "OrderBy")
            {
                OrderByClause = VisitOrderBy(m);
            }

            else if (name == "Limit")
            {
                LimitClause = VisitLimit(m);
            }

            if (ret == null)
                return;
            
            list.Add(ret);

            if (level <= prevItemLevel)
                return;

            Group group;

            while ((group = ret as Group)!=null)
            {
                Groups.AddLast(group);
                ret = group[0];
            }
        }

        private IWhereItem VisitMatch(MethodCallExpression m)
        {
            var tripple = SPARQL.Tripple((string)((ConstantExpression)m.Arguments[1]).Value,
                new List<string>() { string.Concat((string)((ConstantExpression)m.Arguments[2]).Value," ", (string)((ConstantExpression)m.Arguments[3]).Value) });
            return tripple;
        }
        /// <summary>
        /// Evaluates And with one parameter
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>expression</returns>
        private IWhereItem VisitAnd1(MethodCallExpression m, IList<IWhereItem> list)
        {
            var prev = (Triple)list[list.Count - 1];

            int lastPropIdx = prev.Property.Count - 1;
            prev.Property[lastPropIdx] = string.Concat(prev.Property[lastPropIdx], ", ", (string)((ConstantExpression)m.Arguments[1]).Value);

            return null;
        }
        /// <summary>
        /// Evaluates And with two parameters
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>expression</returns>
        private IWhereItem VisitAnd2(MethodCallExpression m, IList<IWhereItem> list)
        {
            var prev = (Triple)list[list.Count - 1];
            prev.Property.Add(string.Concat((string)((ConstantExpression)m.Arguments[1]).Value, " ", (string)((ConstantExpression)m.Arguments[2]).Value));

            return null;
        }

        private IWhereItem VisitOptional(MethodCallExpression m)
        {
            var tripple = SPARQL.Tripple((string)((ConstantExpression)m.Arguments[1]).Value,
                new List<string>() { string.Concat((string)((ConstantExpression)m.Arguments[2]).Value," ", (string)((ConstantExpression)m.Arguments[3]).Value) });
                         
            return new Optional(tripple);
        }

        private IWhereItem VisitFilterBy(MethodCallExpression m)
        {
            var filter = SPARQL.Filter((string)((ConstantExpression)m.Arguments[1]).Value);
            return filter;
        }

        private IWhereItem VisitEither(MethodCallExpression m)
        {
            var union = new Union(
                new Group(
                    SPARQL.Tripple((string)((ConstantExpression)m.Arguments[1]).Value,
                        new List<string>() { string.Concat((string)((ConstantExpression)m.Arguments[2]).Value, " ", (string)((ConstantExpression)m.Arguments[3]).Value) })
                ),
                null
            );
            
            return union;
        }

        private IWhereItem VisitOR(MethodCallExpression m, IList<IWhereItem> list)
        {
            var orGroup =new Group(
                    SPARQL.Tripple((string)((ConstantExpression)m.Arguments[1]).Value,
                        new List<string>() { string.Concat((string)((ConstantExpression)m.Arguments[2]).Value, " ", (string)((ConstantExpression)m.Arguments[3]).Value) })
                );

            return orGroup;
        }


        private IWhereItem VisitGroup(MethodCallExpression m)
        {
            var group = new Group(
                    SPARQL.Tripple((string)((ConstantExpression)m.Arguments[1]).Value,
                        new List<string>() { string.Concat((string)((ConstantExpression)m.Arguments[2]).Value, " ", (string)((ConstantExpression)m.Arguments[3]).Value) })
                );

            return group;
        }

        private string VisitSelect(MethodCallExpression m)
        {
            var arg = ((ConstantExpression)m.Arguments[1]).Value;
           

            string str = arg as string;

            if (str != null)
                return str;
            
            IList<string> list = arg as IList<string>;
            if (list!=null && list.Count > 0)
            {
                return list.Join2String(" ");
            }

            return GetSelectByTypeArgument(arg as Type);
        }

        private string GetSelectByTypeArgument(Type type)
        {
            if (type == null)
                return null;

            return type.GetProperties().Select(prop => "?" + prop.Name.ToLower()).Join2String(" ");
        }

        private string VisitOrderBy(MethodCallExpression m)
        {
            return (string)((ConstantExpression)m.Arguments[1]).Value;
        }

        private int VisitLimit(MethodCallExpression m)
        {
            return (int)((ConstantExpression)m.Arguments[1]).Value;
        }


    }

}
