using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URISerializer
{
    public class SchemeResolver : IEnumerable<KeyValuePair<string, Type>>
    {
        private readonly List<Tuple<string, Type>> _map;

        public SchemeResolver()
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
}
