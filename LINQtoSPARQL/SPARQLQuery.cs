using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DynamicSPARQLSpace;

namespace LINQtoSPARQLSpace
{
    public class SPARQLQuery<T> : ISPARQLQueryable, ISPARQLQueryable<T>
    {
        protected SPARQLQueryProvider provider;
        protected Expression expression;

        public SPARQLQuery(DynamicSPARQL dyno)
            : this(new SPARQLQueryProvider(dyno))
        {
        }

        public SPARQLQuery(SPARQLQueryProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            this.provider = provider;
            this.expression = Expression.Constant(this);
        }

        public SPARQLQuery(SPARQLQueryProvider provider, Expression expression)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (!typeof(ISPARQLQueryable).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }
            this.provider = provider;
            this.expression = expression;
        }

        ISPARQLQueryProvider ISPARQLQueryable.Provider
        {
            get { return this.provider; }
        }


        Expression ISPARQLQueryable.Expression
        {
            get { return this.expression; }
        }

        public override string ToString()
        {
            return provider.GetQueryText(expression);
        }
    }


    internal class SPARQLQueryInternal<T> : SPARQLQuery<T>, ISPARQLUnionQueryable<T>, ISPARQLMatchedQueryable<T>, ISPARQLQueryable
    {
        public SPARQLQueryInternal(SPARQLQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }
    }



    internal class SPARQLQueryAsQueryable<T> : SPARQLQuery<T>, IEnumerable<T>, IQueryable<T>
    {
        public SPARQLQueryAsQueryable(SPARQLQueryProvider provider, Expression expression)
            : base(provider, expression)
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this.provider.ExecuteEnumerable<T>(this.expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.provider.Execute(this.expression)).GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get { return expression; }
        }

        public IQueryProvider Provider
        {
            get { return provider; }
        }
    }
}
