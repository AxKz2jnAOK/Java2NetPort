using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Java2NetPort.JNDI
{
    public class CompoundName : List<string>, IName
    {
        private IDictionary<string, string> _syntax;
        private string _separator;
        private bool _trimBlanks;
        private NameSyntaxValues_direction _direction;

        
        public CompoundName(string name, IDictionary<string, string> syntax)
        {
            _syntax = syntax;


            if (syntax.Keys.Contains(NameSyntaxKeys.trimblanks.ToString()))
            {
                _trimBlanks = Boolean.Parse(syntax[NameSyntaxKeys.trimblanks.ToString()]);
            }
            else
            {
                _trimBlanks = false;
            }

            if (syntax.Keys.Contains(NameSyntaxKeys.separator.ToString()))
            {
                _separator = syntax[NameSyntaxKeys.separator.ToString()];                
            }
            else
            {
                _separator = "/";
            }

            if (syntax.Keys.Contains(NameSyntaxKeys.direction.ToString()))
            {
                _direction = (NameSyntaxValues_direction)Enum.Parse(typeof(NameSyntaxValues_direction), syntax[NameSyntaxKeys.direction.ToString()]);
            }

            foreach (string n in name.Split(new string[] { _separator }, StringSplitOptions.None))
            {
                if (!string.IsNullOrEmpty(n))
                {
                    this.Add(_trimBlanks ? n.Trim() : n);
                }
            }
        }

        public IEnumerable<string> GetAll()
        {
            if (_direction == NameSyntaxValues_direction.right_to_left)
            {
                IList<string> l = new List<string>(this);
                l.Reverse();
                return l;
            }
            else
                return this;
        }


        public void Insert(int index, IName name)
        {
            this.Insert(index, name.GetAll().First());
        }


        public void Add(IName name)
        {
            this.Add(name.GetAll().First());            
        }

        public int CompareTo(object obj)
        {
            if (!(obj is IName))
            {
                return -1;
            }
            else
            {
                return 0;
            }            
        }

        public bool EndsWith(IName name)
        {
            return this.Last() == name.GetAll().First();
        }


        public string Get(int position)
        {
            return this[position];
        }

        public bool IsEmpty()
        {
            return !this.Any();
        }


        public string Remove(int index)
        {
            string r = this.ElementAt(index);
            this.RemoveAt(index);
            return r;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (_direction == NameSyntaxValues_direction.flat || _direction == NameSyntaxValues_direction.left_to_right)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    sb.Append(this[i]);
                    if (i + 1 != this.Count)
                    {
                        sb.Append(_separator);
                    }
                }
            }
            else
            {
                for (int i = this.Count-1; i >= 0; i--)
                {
                    if (i != 0)
                    {
                        sb.Append(_separator);
                    }
                    sb.Append(this[i]);                    
                }
            }
            return sb.ToString();
        }
    }
}
