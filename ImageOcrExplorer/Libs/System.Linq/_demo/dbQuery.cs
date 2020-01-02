using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;

using System.Linq.Dynamic;
using System.Collections;
using Newtonsoft.Json;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace host
{
    public class dbQuery
    {
        public static Tuple<bool, string, int, int, IList> where_cache<T>(T[] items, string s_key, string s_select, string s_where, string s_order_by, string s_distinct, int page_number, int page_size)
        {
            int rs_total = items.Length;
            try
            {
                T[] a = new T[] { };
                if (!string.IsNullOrEmpty(s_where))
                    a = items.Where(s_where).Cast<T>().ToArray();
                else
                    a = items;

                IList ls = new List<dynamic>() { };
                IList dt = new List<dynamic>() { };
                if (!string.IsNullOrEmpty(s_distinct))
                {
                    string[] adis = s_distinct.Split(',').Select(x => "Key." + x.Trim()).ToArray();
                    string s_dis_select = string.Join(",", adis);

                    s_select = "";
                    if (string.IsNullOrEmpty(s_order_by))
                        ls = a.AsQueryable().GroupBy("new(" + s_distinct + ")").Select("new(" + s_dis_select + ")").ToListDynamic();
                    else
                    {
                        //var ls = 
                        //ls = a.SortMultiple(s_order_by).AsQueryable().GroupBy("new(" + s_distinct + ")").Select("new(" + s_dis_select + ")").ToListDynamic();
                        ls = a.AsQueryable().GroupBy("new(" + s_distinct + ")").Select("new(" + s_dis_select + ")").ToListDynamic();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(s_order_by))
                        ls = a;
                    else
                        ls = a.SortMultiple(s_order_by).ToArray();
                }

                int rs_count = ls.Count;
                if (rs_count <= page_size)
                    dt = ls;
                else
                {
                    int startRowIndex = page_size * (page_number - 1);
                    dt = ls.Cast<dynamic>().Skip(startRowIndex).Take(page_size).ToArray();
                }

                dynamic dy = dt;
                if (!string.IsNullOrEmpty(s_select))
                    dy = ls.Select("new(" + s_select + ")").ToListDynamic();

                //string json = JsonConvert.SerializeObject(dy);

                return new Tuple<bool, string, int, int, IList>(true, "", rs_total, rs_count, dy);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, int, int, IList>(false, ex.Message, rs_total, 0, null);
            }
        }

        public static Tuple<bool, string, int, int, IList> where<T>(
            T[] items, string s_key, string s_select, string s_where, string s_order_by, string s_distinct, int page_number, int page_size)
        {
            string s_tab = typeof(T).FullName;

            string key_cache = s_tab + "|" +
                s_key + "|" +
                s_select + "|" + s_where + "|" + Guid.NewGuid().ToString() +
                s_order_by + "|" + s_distinct ;


            int rs_total = items.Length;
            int rs_count = 0;
            try
            {
                IList dy;

                ObjectCache cache = MemoryCache.Default;
                if (cache[key_cache] == null)
                {
                    #region // query ...

                    T[] a = new T[] { };
                    if (!string.IsNullOrEmpty(s_where))
                        a = items.Where(s_where).Cast<T>().ToArray();
                    else
                        a = items;

                    IList ls = new List<dynamic>() { };
                    IList dt = new List<dynamic>() { };
                    if (!string.IsNullOrEmpty(s_distinct))
                    {
                        string[] adis = s_distinct.Split(',').Select(x => "Key." + x.Trim()).ToArray();
                        string s_dis_select = string.Join(",", adis);

                        s_select = "";
                        if (string.IsNullOrEmpty(s_order_by))
                            ls = a.AsQueryable().GroupBy("new(" + s_distinct + ")").Select("new(" + s_dis_select + ")").ToListDynamic();
                        else
                        {
                            //var ls = 
                            //ls = a.SortMultiple(s_order_by).AsQueryable().GroupBy("new(" + s_distinct + ")").Select("new(" + s_dis_select + ")").ToListDynamic();
                            ls = a.AsQueryable().GroupBy("new(" + s_distinct + ")").Select("new(" + s_dis_select + ")").ToListDynamic();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(s_order_by))
                            ls = a;
                        else
                            ls = a.SortMultiple(s_order_by).ToArray();
                    }

                    rs_count = ls.Count;
                    //ObjectCache cache = MemoryCache.Default;
                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.Priority = System.Runtime.Caching.CacheItemPriority.Default;
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30);
                    //Muốn tạo sự kiện thì tạo ở đây còn k thì hoy
                    //(MyCacheItemPriority == MyCachePriority.Default) ? CacheItemPriority.Default : CacheItemPriority.NotRemovable;
                    ///Globals.policy.RemovedCallback = callback; 
                    cache.Set(key_cache, ls, policy, null);

                    dbCache.dicKeys.AddDistinct(s_tab, key_cache);

                    if (rs_count <= page_size)
                        dt = ls;
                    else
                    {
                        int startRowIndex = page_size * (page_number - 1);
                        dt = ls.Cast<dynamic>().Skip(startRowIndex).Take(page_size).ToArray();
                    }

                    //IList dy = dt;
                    dy = dt;
                    if (!string.IsNullOrEmpty(s_select))
                        dy = ls.Select("new(" + s_select + ")").ToListDynamic();
                    //string json = JsonConvert.SerializeObject(dy);

                    #endregion
                     
                }
                else
                {
                    dy = cache[key_cache] as IList;
                    rs_count = dy.Count;

                    if (rs_count <= page_size)
                    { }
                    else
                    {
                        int startRowIndex = page_size * (page_number - 1);
                        dy = dy.Cast<dynamic>().Skip(startRowIndex).Take(page_size).ToArray();
                    }
                     
                    if (!string.IsNullOrEmpty(s_select))
                        dy = dy.Select("new(" + s_select + ")").ToListDynamic();
                }

                return new Tuple<bool, string, int, int, IList>(true, "", rs_total, rs_count, dy);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, int, int, IList>(false, ex.Message, rs_total, 0, null);
            }
        }




    }//end class
}
