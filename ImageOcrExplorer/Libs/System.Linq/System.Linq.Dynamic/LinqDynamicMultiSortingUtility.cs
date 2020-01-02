using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic
{
    public static class LinqDynamicMultiSortingUtility
    {
        /// <summary>
        /// sort multi column: column_1 asc, column_2 desc, ...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sortString"></param>
        /// <returns></returns>
        public static IEnumerable<T> SortMultiple<T>(this IEnumerable<T> data, string sortString)
        {
            // Prepare the sorting string into a list of Tuples
            var sortExpressions = new List<Tuple<string, string>>() { };
            try
            {
                string[] terms = sortString.Split(',').Select(x => x.Trim()).ToArray();
                for (int i = 0; i < terms.Length; i++)
                {
                    string[] items = terms[i].Trim().Split(' ');
                    var fieldName = items[0].Trim();
                    var sortOrder = (items.Length > 1) ? items[1].Trim().ToLower() : "asc";
                    if ((sortOrder != "asc") && (sortOrder != "desc"))
                    {
                        //throw new ArgumentException("Invalid sorting order");
                        sortOrder = "asc";
                    }
                    sortExpressions.Add(new Tuple<string, string>(fieldName, sortOrder));
                } 
            }
            catch 
            {
                // var msg = "There is an error in your sorting string. Please correct it and try again - " + e.Message;
                // ShowMessage(msg);
            }

            if (sortExpressions.Count > 0)
                return data.SortMultiple<T>(sortExpressions);
            return data;
        }

        /// <summary>
        /// 1. The sortExpressions is a list of Tuples, the first item of the 
        ///    tuples is the field name,
        ///    the second item of the tuples is the sorting order (asc/desc) case sensitive.
        /// 2. If the field name (case sensitive) provided for sorting does not exist 
        ///    in the object,
        ///    exception is thrown
        /// 3. If a property name shows up more than once in the "sortExpressions", 
        ///    only the first takes effect.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="sortExpressions"></param>
        /// <returns></returns>
        public static IEnumerable<T> SortMultiple<T>(this IEnumerable<T> data,
          List<Tuple<string, string>> sortExpressions)
        {
            // No sorting needed
            if ((sortExpressions == null) || (sortExpressions.Count <= 0))
            {
                return data;
            }

            // Let us sort it
            IEnumerable<T> query = from item in data select item;
            IOrderedEnumerable<T> orderedQuery = null;

            for (int i = 0; i < sortExpressions.Count; i++)
            {
                // We need to keep the loop index, not sure why it is altered by the Linq.
                var index = i;
                Func<T, object> expression = item => item.GetType()
                                .GetProperty(sortExpressions[index].Item1)
                                .GetValue(item, null);

                if (sortExpressions[index].Item2 == "asc")
                {
                    orderedQuery = (index == 0) ? query.OrderBy(expression)
                      : orderedQuery.ThenBy(expression);
                }
                else
                {
                    orderedQuery = (index == 0) ? query.OrderByDescending(expression)
                             : orderedQuery.ThenByDescending(expression);
                }
            }

            query = orderedQuery;

            return query;
        }
    }
}
