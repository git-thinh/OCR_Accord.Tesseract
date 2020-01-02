using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq.Dynamic
{


    /*
    c# dictionary How to add multiple values for single key ?
Dictionary<string, List<string>> dictionary = new Dictionary<string,List<string>>();
       
var dictionary = new Dictionary<string, List<int>>();
 
if (!dictionary.ContainsKey("foo"))
    dictionary.Add("foo", new List<int>());
 
dictionary["foo"].Add(42);
dictionary["foo"].AddRange(oneHundredInts);
       
dictionary.Add("key", new List<string>());
       
dictionary["key"].Add("string to your list");
       
/// <summary>
/// Represents a collection of keys and values. Multiple values can have the same key.
/// </summary>
/// <typeparam name="TKey">Type of the keys.</typeparam>
/// <typeparam name="TValue">Type of the values.</typeparam>
/// 
    foreach(string key in keys) {
    if(!dictionary.ContainsKey(key)) {
        //add
        dictionary.Add(key, new List<string>());
    }
    dictionary[key].Add("theString");
}
       
System.Collections.Specialized.NameValueCollection myCollection
    = new System.Collections.Specialized.NameValueCollection();
 
  myCollection.Add(“Arcane”, “http://arcanecode.com”);
  myCollection.Add(“PWOP”, “http://dotnetrocks.com”);
  myCollection.Add(“PWOP”, “http://dnrtv.com”);
  myCollection.Add(“PWOP”, “http://www.hanselminutes.com”);
  myCollection.Add(“TWIT”, “http://www.twit.tv”);
  myCollection.Add(“TWIT”, “http://www.twit.tv/SN”);
       
List<string> list;
if (dictionary.ContainsKey(key)) {
  list = dictionary[key];
} else {
  list = new List<string>();
  dictionary.Add(ley, list);
}
list.Add(value);
       
void Add(string key, string val)
{
    List<string> list;
 
    if (!dictionary.TryGetValue(someKey, out list))
    {
       values = new List<string>();
       dictionary.Add(key, list);
    }
 
    list.Add(val);
}
       
var myData = new[]{new {a=1,b="frog"}, new {a=1,b="cat"}, new {a=2,b="giraffe"}};
ILookup<int,string> lookup = myData.ToLookup(x => x.a, x => x.b);
IEnumerable<string> allOnes = lookup[1]; //enumerable of 2 items, frog and cat
    */

    /// <summary>
    /// Represents a collection of keys and values. DictionaryMultiValues Multiple values can have the same key.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the values.</typeparam>
    public class DictionaryListValues<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {

        public DictionaryListValues()
            : base()
        {
        }

        public DictionaryListValues(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Adds an element with the specified key and value into the MultiMap.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            List<TValue> valueList;

            if (TryGetValue(key, out valueList))
            {
                valueList.Add(value);
            }
            else
            {
                valueList = new List<TValue>() { };
                if (value != null)
                    valueList.Add(value);

                Add(key, valueList);
            }
        }

        public int AddList(TKey key, TValue[] items)
        {
            List<TValue> valueList = new List<TValue>() { };

            if (TryGetValue(key, out valueList))
            {
                valueList.AddRange(items);
            }
            else
            {
                valueList = new List<TValue>() { };
                if (items.Length > 0)
                    valueList.AddRange(items);

                Add(key, valueList);
            }

            return valueList.Count;
        }

        /// <summary>
        /// Removes first occurence of an element with a specified key and value.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <param name="value">The value of the element to remove.</param>
        /// <returns>true if the an element is removed; false if the key or the value were not found.</returns>
        public bool Remove(TKey key, TValue value)
        {
            List<TValue> valueList;

            if (TryGetValue(key, out valueList))
            {
                if (valueList.Remove(value))
                {
                    if (valueList.Count == 0)
                    {
                        Remove(key);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes all occurences of elements with a specified key and value.
        /// </summary>
        /// <param name="key">The key of the elements to remove.</param>
        /// <param name="value">The value of the elements to remove.</param>
        /// <returns>Number of elements removed.</returns>
        public int RemoveAll(TKey key, TValue value)
        {
            List<TValue> valueList;
            int n = 0;

            if (TryGetValue(key, out valueList))
            {
                while (valueList.Remove(value))
                {
                    n++;
                }
                if (valueList.Count == 0)
                {
                    Remove(key);
                }
            }
            return n;
        }

        /// <summary>
        /// Gets the total number of values contained in the MultiMap.
        /// </summary>
        public int CountAll
        {
            get
            {
                int n = 0;

                foreach (List<TValue> valueList in Values)
                {
                    n += valueList.Count;
                }
                return n;
            }
        }

        /// <summary>
        /// Determines whether the MultiMap contains an element with a specific key / value pair.
        /// </summary>
        /// <param name="key">Key of the element to search for.</param>
        /// <param name="value">Value of the element to search for.</param>
        /// <returns>true if the element was found; otherwise false.</returns>
        public bool Contains(TKey key, TValue value)
        {
            List<TValue> valueList;

            if (TryGetValue(key, out valueList))
            {
                return valueList.Contains(value);
            }
            return false;
        }

        /// <summary>
        /// Determines whether the MultiMap contains an element with a specific value.
        /// </summary>
        /// <param name="value">Value of the element to search for.</param>
        /// <returns>true if the element was found; otherwise false.</returns>
        public bool Contains(TValue value)
        {
            foreach (List<TValue> valueList in Values)
            {
                if (valueList.Contains(value))
                {
                    return true;
                }
            }
            return false;
        }

    }

}