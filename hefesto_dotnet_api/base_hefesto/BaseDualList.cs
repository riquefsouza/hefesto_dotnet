using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.base_hefesto
{
    public class BaseDualList<T> : IEquatable<BaseDualList<T>>
	{
		public List<T> Source { get; set; }

        public List<T> Target { get; set; }

		public BaseDualList() {
			Source = new List<T>();
			Target = new List<T>();
		}

		public BaseDualList(List<T> source, List<T> target)
		{
			this.Source = source;
			this.Target = target;
		}

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseDualList<T>);
        }

        public bool Equals(BaseDualList<T> other)
        {
            if (other == null)
                return false;

            if (this.GetType() != other.GetType())
                return false;

            if (this.Source != other.Source && (this.Source == null || !this.Source.Equals(other.Source)))
                return false;
            if (this.Target != other.Target && (this.Target == null || !this.Target.Equals(other.Target)))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Source, Target);
        }

    }
}
