using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web;
using System.Collections;

namespace URISerializer
{

    public class SchemeMap : IEnumerable<KeyValuePair<string, Type>>
    {
        private readonly List<Tuple<string, Type>> _map;

        public SchemeMap()
        {
            this._map = new List<Tuple<string, Type>>();
        }

        public void Add(string scheme, Type type)
        {
            Tuple<string, Type> item = new Tuple<string, Type>(scheme, type);
            if (this._map.Contains(item))
            {
                throw new ArgumentException(string.Format("Cannot add {0} : {1} to scheme map. Mapping already exists.", scheme, type.Name));
            }
            _map.Add(item);
        }

        public Type this[string scheme]
        {
            get
            {
                var item = this._map.Find(x => x.Item1.Equals(scheme, StringComparison.InvariantCultureIgnoreCase));
                return item.Item2;
            }
        }

        public string this[Type type]
        {
            get
            {
                var item = this._map.Find(x => x.Item2 == type);
                return item.Item1;
            }
        }


        public IEnumerator<KeyValuePair<string, Type>> GetEnumerator()
        {
            foreach (Tuple<string, Type> item in this._map)
            {
                yield return new KeyValuePair<string, Type>(item.Item1, item.Item2);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    public class URISerializer
    {
        private readonly SchemeMap _map;

        public URISerializer(SchemeMap map)
        {
            this._map = map;
        }

        public Uri Serialize<T>(T obj)
        {
            return null;
        }

        public T Deserialize<T>(Uri uri)
        {
            string scheme = uri.Scheme;
            string resource = uri.GetLeftPart(UriPartial.Path);
            NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);

            Type type = this._map[scheme];
            if (type == null)
            {
                throw new Exception(string.Format("Scheme: {0} is not registered"));
            }

            /*
                1. look for a constructor marked with Attribute
                2. look for a public default constructor (a constructor that doesn't take any arguments)
                3. then check if the class has a single public constructor with arguments 
                4. and finally check for a non-public default constructor. 

                If the class has multiple public constructors with arguments an error will be thrown. 
                This can be fixed by marking one of the constructors with an attribute.
            */

 




            return (T)obj;
        }
    }
}
