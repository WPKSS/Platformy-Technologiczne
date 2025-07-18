using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PTLab7
{
    public class SearchableSortableCollection<T> : ObservableCollection<T>
    {

        private object GetNestedValue(object obj, string path)
        {

            var prop = obj.GetType().GetProperty(path);     
            
            obj = prop.GetValue(obj);

            return obj;
        }

        public void SortBy(string propertyName)
        {

            var sorted = this.OrderBy(x =>
            {
                var val = GetNestedValue(x, propertyName);

                return val as IComparable;

            }).ToList();

            this.Clear();

            foreach (var item in sorted)
            {
                this.Add(item);
            }

        }


        public void Search(string propertyName, object value)
        {

            PropertyInfo property = typeof(T).GetProperty(propertyName);


            var filtered = this.Where(x =>
            {

                var propVal = property.GetValue(x);

                return propVal != null && propVal.ToString().Equals(value.ToString(), StringComparison.OrdinalIgnoreCase);

            }).ToList();

            this.Clear();

            foreach (var item in filtered)
            {
                this.Add(item);
            }

        }

        public void Reset(IEnumerable<T> originalItems)
        {

            this.Clear();

            foreach (var item in originalItems) {

                this.Add(item);

            }
        }


    }
}
