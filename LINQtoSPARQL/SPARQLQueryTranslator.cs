using System;
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
        private readonly string[] GroupingMethods = {"Optional", "Group", "Either", "Or", "Minus", "Exists", "NotExists"};
        /// <summary>
        /// Where clause
        /// </summary>
        public Group WhereClause { get; private set; }
        /// <summary>
        /// Select clause
        /// </summary>
        public string SelectClause { get; private set; }
        /// <summary>
        /// Order By clause
        /// </summary>
        public string OrderByClause { get; private set; }
        /// <summary>
        /// Limit clause
        /// </summary>
        public int? LimitClause { get; private set; }
        /// <summary>
        /// SPARQL prefixes 
        /// </summary>
        public IList<Prefix> Prefixes { get; private set; }
        /// <summary>
        /// Group By clause
        /// </summary>
        public string GroupByClause { get; private set; }
        /// <summary>
        /// Having clause
        /// </summary>
        public string HavingClause { get; private set; }
        /// <summary>
        /// Offset clause
        /// </summary>
        public int? OffsetClause { get; private set; }


        internal void Translate(Expression expression)
        {
            Groups = new LinkedList<IList<IWhereItem>>();
            Groups.AddLast(new Group() { Items = new List<IWhereItem>() });
            WhereClause = null;
            SelectClause = null;
            Prefixes = new List<Prefix>(5);
            this.Visit(expression);
            WhereClause = (Group)Groups.Last.Value;
            if (SelectClause != null)
                return;

            SelectClause = GetSelectByTypeArgument(((MethodCallExpression)expression).Method.GetGenericArguments()[0]);

        }
        /// <summary>
        /// Visits any method call expression
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType != typeof(LINQtoSPARQLExtensions))
                throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));


            VisitSPARQL(m); 

            return m;

        }
        /// <summary>
        /// Visit LINQtoSPARQL expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <param name="level">expression group level</param>
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
                    (name == "Or" && prevMethod.Method.Name != "End") 
                     ? prevItemLevel+1 : prevItemLevel);
            }

            var list = Groups.Last.Value;

            switch(name)
            {
                case "End":         Groups.RemoveLast();break;
                case "Group":       ret = VisitGroup(m);break;
                case "Match":       ret = VisitMatch(m);break;
                case "And_1":       ret = VisitAnd1(m, list);break;
                case "And_2":       ret = VisitAnd2(m, list);break;
                case "Optional":    ret = VisitOptional(m); break;
                case "FilterBy":    ret = VisitFilterBy(m);break;
                case "Either":      ret = VisitEither(m);break;
                case "Minus":       ret = VisitMinus(m); break;
                case "Exists":      ret = VisitExists(m); break;
                case "NotExists":   ret = VisitNotExists(m); break;
                case "Or":          
                                    if (prevMethod.Method.Name != "End")
                                        Groups.RemoveLast();
                                    list = Groups.Last.Value;
                                    Groups.RemoveLast();
                                    ret = VisitOr(m, list);
                                    break;
                case "Select":      SelectClause = VisitSelect(m);break;
                case "OrderBy":     OrderByClause = VisitOrderBy(m);break;
                case "GroupBy":     GroupByClause = VisitOrderBy(m);break;
                case "Having":      HavingClause = VisitOrderBy(m);break;
                case "Limit":       LimitClause = VisitLimit(m);break;
                case "Offset":      OffsetClause = VisitLimit(m);break;
                case "Prefix":      VisitPrefix(m); break;
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
        /// <summary>
        /// Evaluates Match expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitMatch(MethodCallExpression m)
        {
            return MakeTripleFromArguments(m);
        }
        /// <summary>
        /// Evaluates And expression with one parameter
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitAnd1(MethodCallExpression m, IList<IWhereItem> list)
        {
            var prev = (Triple)list[list.Count - 1];

            int lastPropIdx = prev.Property.Count - 1;
            prev.Property[lastPropIdx] = string.Concat(prev.Property[lastPropIdx], ", ", (string)((ConstantExpression)m.Arguments[1]).Value);

            return null;
        }
        /// <summary>
        /// Evaluates And expression with two parameters
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitAnd2(MethodCallExpression m, IList<IWhereItem> list)
        {
            var prev = (Triple)list[list.Count - 1];
            prev.Property.Add(string.Concat((string)((ConstantExpression)m.Arguments[1]).Value, " ", (string)((ConstantExpression)m.Arguments[2]).Value));

            return null;
        }
        /// <summary>
        /// Evaluates Optional expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitOptional(MethodCallExpression m)
        {
            return new Optional(MakeTripleFromArguments(m));
        }
        /// <summary>
        /// Evaluates FilterBy expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitFilterBy(MethodCallExpression m)
        {
            var filter = SPARQL.Filter((string)((ConstantExpression)m.Arguments[1]).Value);
            return filter;
        }
        /// <summary>
        /// Evaluates Either expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
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
        /// <summary>
        /// Evaluates Or expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <param name="list"></param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitOr(MethodCallExpression m, IList<IWhereItem> list)
        {
            var orGroup =new Group(
                    SPARQL.Tripple((string)((ConstantExpression)m.Arguments[1]).Value,
                        new List<string>() { string.Concat((string)((ConstantExpression)m.Arguments[2]).Value, " ", (string)((ConstantExpression)m.Arguments[3]).Value) })
                );

            return orGroup;
        }

        /// <summary>
        /// Evaluates Group expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitGroup(MethodCallExpression m)
        {
            var group = new Group(
                    SPARQL.Tripple((string)((ConstantExpression)m.Arguments[1]).Value,
                        new List<string>() { string.Concat((string)((ConstantExpression)m.Arguments[2]).Value, " ", (string)((ConstantExpression)m.Arguments[3]).Value) })
                );

            return group;
        }
        /// <summary>
        /// Evaluates Select expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>projection</returns>
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
        /// <summary>
        /// Translate properties of type to delimited string 
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>properties delimited string</returns>
        private string GetSelectByTypeArgument(Type type)
        {
            if (type == null)
                return null;

            return type.GetProperties().Select(prop => "?" + prop.Name.ToLower()).Join2String(" ");
        }
        /// <summary>
        /// Evaluates OrderBy expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>orderBy</returns>
        private string VisitOrderBy(MethodCallExpression m)
        {
            return (string)((ConstantExpression)m.Arguments[1]).Value;
        }
        /// <summary>
        /// Evaluates GroupBy expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>groupBy</returns>
        private string VisitGroupBy(MethodCallExpression m)
        {
            return (string)((ConstantExpression)m.Arguments[1]).Value;
        }
        /// <summary>
        /// Evaluates Having expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>Having</returns>
        private string VisitHaving(MethodCallExpression m)
        {
            return (string)((ConstantExpression)m.Arguments[1]).Value;
        }
        /// <summary>
        /// Evaluates Limit expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>limit number</returns>
        private int VisitLimit(MethodCallExpression m)
        {
            return (int)((ConstantExpression)m.Arguments[1]).Value;
        }
        /// <summary>
        /// Evaluates Offset expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>offset number</returns>
        private int VisitOffset(MethodCallExpression m)
        {
            return (int)((ConstantExpression)m.Arguments[1]).Value;
        }
        /// <summary>
        /// Evaluates Prefix expression
        /// </summary>
        /// <param name="m">method call expression</param>
        private void VisitPrefix(MethodCallExpression m)
        {
            string prefix = (string)((ConstantExpression)m.Arguments[1]).Value;
            string iri = (string)((ConstantExpression)m.Arguments[2]).Value;

            Prefixes.Add(new Prefix() { PREFIX = prefix, IRI = iri });
        }
        /// <summary>
        /// Evaluates Minus expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitMinus(MethodCallExpression m)
        {
            return new Minus(MakeTripleFromArguments(m));
        }
        /// <summary>
        /// Evaluates Exists expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitExists(MethodCallExpression m)
        {
            return new Exists(MakeTripleFromArguments(m));
        }
        /// <summary>
        /// Evaluates NotExists expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitNotExists(MethodCallExpression m)
        {
            return new NotExists(MakeTripleFromArguments(m));
        }

        private Triple MakeTripleFromArguments(MethodCallExpression m)
        {
            return SPARQL.Tripple((string)((ConstantExpression)m.Arguments[1]).Value,
               new List<string>() { string.Concat((string)((ConstantExpression)m.Arguments[2]).Value, " ", (string)((ConstantExpression)m.Arguments[3]).Value) });
        }


    }

}
