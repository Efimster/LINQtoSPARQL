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
        private static readonly string[] GroupingMethods = {"Optional", "Group", "Either", "Or", "Minus", "Exists", "NotExists", "Graph"};
        private static readonly string[] ClauseMethods = { "Delete","Insert" };
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
        /// <summary>
        /// Distinct projection
        /// </summary>
        public bool Distinct { get; private set; }
        /// <summary>
        /// Delete clause
        /// </summary>
        public Group DeleteClause { get; private set; }
        /// <summary>
        /// Insert clause
        /// </summary>
        public Group InsertClause { get; private set; }
        /// <summary>
        /// FROM clause
        /// </summary>
        public From FromClause { get; private set; }
        /// <summary>
        /// FROM NAMED clause
        /// </summary>
        public IEnumerable<FromNamed> FromNamedClause { get; private set; }
        /// <summary>
        /// WITH clause
        /// </summary>
        public With WithClause { get; private set; }
        /// <summary>
        /// USING clause
        /// </summary>
        public Using UsingClause { get; private set; }
        /// <summary>
        /// USING NAMED clause
        /// </summary>
        public IList<UsingNamed> UsingNamedClause { get; private set; }


        private MethodCallExpression NextMethodCall { get; set; }


        internal void Translate(Expression expression)
        {
            
            WhereClause = null;
            SelectClause = null;
            DeleteClause = null;
            InsertClause = null;
            WithClause = null;
            UsingClause = null;
            UsingNamedClause = new List<UsingNamed>(2);
            FromClause = null;
            FromNamedClause = new List<FromNamed>(2);
            Prefixes = new List<Prefix>(5);
            Distinct = false;
            this.Visit(expression);
          

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

            NextMethodCall = m;

            while (NextMethodCall != null) 
            {
                Groups = new LinkedList<IList<IWhereItem>>();
                Groups.AddLast(new Group() { Items = new List<IWhereItem>() });
                VisitSPARQL(NextMethodCall);

                if (NextMethodCall==null)
                {
                    WhereClause = Groups.Last.Value.Count>0 ? (Group)Groups.Last.Value : null;

                    if (SelectClause == null)
                        SelectClause = GetSelectByTypeArgument(((MethodCallExpression)m).Method.GetGenericArguments()[0]);

                    if (Distinct)
                        SelectClause = "DISTINCT " + SelectClause;

                    continue;

                }

                if (NextMethodCall.Method.Name == "Delete")
                    DeleteClause = Groups.Last.Value.Count > 0 ? (Group)Groups.Last.Value : null;
                else if (NextMethodCall.Method.Name == "Insert")
                    InsertClause = Groups.Last.Value.Count > 0 ? (Group)Groups.Last.Value : null;
                
                NextMethodCall = NextMethodCall.Arguments[0] as MethodCallExpression;
            }
                

           

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
            
           

            IWhereItem ret = null;
            string name = m.Method.Name;

            int nextItemLevel = level;
            var nextMethodCall = m.Arguments[0] as MethodCallExpression;

            if (name == "End")
            {
                nextItemLevel++;
            }
            else if (level > 0 && GroupingMethods.Any(n=>n==name))
            {
                nextItemLevel--;
            }

            if (!ClauseMethods.Any(n => n == name))
            {
                NextMethodCall = nextMethodCall;
                if (NextMethodCall != null)
                {
                    VisitSPARQL(NextMethodCall,
                        (name == "Or" && nextMethodCall.Method.Name != "End")
                         ? nextItemLevel + 1 : nextItemLevel);
                }
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
                                    if (nextMethodCall.Method.Name != "End")
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
                case "Offset":      OffsetClause = VisitOffset(m);break;
                case "Prefix":      VisitPrefix(m); break;
                //case "Bind":
                case "As":          ret = VisitBindAs(nextMethodCall,m); break;
                case "Distinct":    VisitDistinct(m); break;
                case "From":        FromClause = VisitFrom(m); break;
                case "FromNamed":   VisitFromNamed(m); break;
                case "Using":       UsingClause = VisitUsing(m); break;
                case "UsingNamed":  VisitUsingNamed(m); break;
                case "With":        WithClause = VisitWith(m); break;
                case "Graph":       ret = VisitGraph(m); break;
            }

            if (ret == null)
                return;
            
            list.Add(ret);

            if (level <= nextItemLevel)
                return;

            Group group;

            while ((group = ret as Group)!=null)
            {
                Groups.AddLast(group);
                ret = group.Count > 0 ? group[0] : null;
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
            prev.Property[lastPropIdx] = string.Concat(prev.Property[lastPropIdx], ", ", ((ConstantExpression)m.Arguments[1]).Value.ToString());

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
            prev.Property.Add(string.Concat((string)((ConstantExpression)m.Arguments[1]).Value, " ", ((ConstantExpression)m.Arguments[2]).Value.ToString()));

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
                    SPARQL.Triple((string)((ConstantExpression)m.Arguments[1]).Value,
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
                    SPARQL.Triple((string)((ConstantExpression)m.Arguments[1]).Value,
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
                    SPARQL.Triple((string)((ConstantExpression)m.Arguments[1]).Value,
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

            Prefixes.Add(SPARQL.Prefix(prefix,iri));
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
        /// <summary>
        /// Makes triple from expression arguments
        /// </summary>
        /// <param name="m">method call arguments</param>
        /// <returns></returns>
        private Triple MakeTripleFromArguments(MethodCallExpression m)
        {
            return SPARQL.Triple((string)((ConstantExpression)m.Arguments[1]).Value,
               new List<string>() { string.Concat((string)((ConstantExpression)m.Arguments[2]).Value, " ", ((ConstantExpression)m.Arguments[3]).Value.ToString()) });
        }

        private IWhereItem VisitBindAs(MethodCallExpression bind, MethodCallExpression asValue)
        {
            return SPARQL.Bind(string.Concat((string)((ConstantExpression)bind.Arguments[1]).Value,
                " AS ",
                (string)((ConstantExpression)asValue.Arguments[1]).Value));
        }

        private void VisitDistinct(MethodCallExpression m)
        {
            Distinct = true;
        }
        /// <summary>
        /// Evaluates From expression
        /// </summary>
        /// <param name="m">method call expression</param>
        private From VisitFrom(MethodCallExpression m)
        {
            string iri = (string)((ConstantExpression)m.Arguments[1]).Value;
            return new From(iri);
        }
        /// <summary>
        /// Evaluates FromNamed expression
        /// </summary>
        /// <param name="m">method call expression</param>
        private void VisitFromNamed(MethodCallExpression m)
        {
            //fix in next version! one iri can be specified per expresion only! see UsingNamed expression
            //string iri = (string)((ConstantExpression)m.Arguments[1]).Value;
            FromNamedClause = FromNamedClause.Concat(FromNamed.Parse(((ConstantExpression)m.Arguments[1]).Value));
        }
        /// <summary>
        /// Evaluates Using expression
        /// </summary>
        /// <param name="m">method call expression</param>
        private Using VisitUsing(MethodCallExpression m)
        {
            string iri = (string)((ConstantExpression)m.Arguments[1]).Value;
            return new Using(iri);
        }
        /// <summary>
        /// Evaluates UsingNamed expression
        /// </summary>
        /// <param name="m">method call expression</param>
        private void VisitUsingNamed(MethodCallExpression m)
        {
            string iri = (string)((ConstantExpression)m.Arguments[1]).Value;
            UsingNamedClause.Add(new UsingNamed(iri));
        }
        /// <summary>
        /// Evaluates With expression
        /// </summary>
        /// <param name="m">method call expression</param>
        private With VisitWith(MethodCallExpression m)
        {
            string iri = (string)((ConstantExpression)m.Arguments[1]).Value;
            return new With(iri);
        }
        /// <summary>
        /// Evaluates Graph expression
        /// </summary>
        /// <param name="m">method call expression</param>
        /// <returns>SPARQL Where clause item</returns>
        private IWhereItem VisitGraph(MethodCallExpression m)
        {
            string iri = (string)((ConstantExpression)m.Arguments[1]).Value;
            return new Graph(iri);
        }



    }

}
