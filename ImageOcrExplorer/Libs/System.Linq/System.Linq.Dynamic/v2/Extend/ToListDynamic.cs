using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq.Dynamic
{
    public static class linqToDynamic
    {
        // for IEnumerable
        public static IList ToListDynamic(this IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            if (!enumerator.MoveNext())
                return null;

                //throw new Exception("?? No elements??");

            var value = enumerator.Current;
            var returnList = (IList)typeof(List<>)
                .MakeGenericType(value.GetType())
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null);

            returnList.Add(value);

            while (enumerator.MoveNext())
                returnList.Add(enumerator.Current);

            return returnList;
        }

        // for IQueryable
        public static IList ToListDynamic(this IQueryable source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var returnList = (IList)typeof(List<>)
                .MakeGenericType(source.ElementType)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null);

            foreach (var elem in source)
                returnList.Add(elem);

            return returnList;
        }

         
    }
}
