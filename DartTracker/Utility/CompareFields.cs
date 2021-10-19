using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DartTracker.Utility
{
    public abstract class CompareFields
    {
        public override bool Equals(object other) {
            if (GetType() != other.GetType()) return false;

            foreach (var field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!(field.GetValue(this)?.Equals(field.GetValue(other)) ?? field.GetValue(other) == null)) return false;
            }

            foreach (var prop in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!(prop.GetValue(this)?.Equals(prop.GetValue(other)) ?? prop.GetValue(other) == null)) return false;
            }

            return true;
        }
    }
}
