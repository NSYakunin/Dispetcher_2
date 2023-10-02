using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dispetcher2.Class
{
    public class Employee : IComparable<Employee>, IEquatable<Employee>
    {
        public string Login { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string TabNum { get; set; }
        public bool ITR { get; set; }
        // Переопредщеление HashCode для того, чтобы объект был тот же, при равенстве строк Login
        public override int GetHashCode()
        {
            if (Login == null) return String.Empty.GetHashCode();
            return Login.GetHashCode();
        }
        public bool Equals(Employee other)
        {
            if (other == null) return false;
            else return this.Login.Equals(other.Login);
        }
        public int CompareTo(Employee other)
        {
            if (other == null) return 0;
            else return this.Login.CompareTo(other.Login);
        }
    }
}
