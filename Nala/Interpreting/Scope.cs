using System;
using System.Collections.Generic;
using System.Text;

namespace NathanWiles.Nala.Interpreting
{
    public class Scope
    {
        public Scope ParentScope { get; }
        private Dictionary<string, object> variables;
        public bool ConditionChainExecuted;

        public Scope(Scope ParentScope)
        {
            this.ParentScope = ParentScope;

            variables = new Dictionary<string, object>();
            ConditionChainExecuted = false;
        }

        public bool ContainsIdentifier(string identifier)
        {
            bool parentContains = ParentScope == null ? false : ParentScope.ContainsIdentifier(identifier);
            bool contains = variables.ContainsKey(identifier);

            return contains || parentContains;
        }

        public void AddVariable(string identifier, object initialValue)
        {
            variables.Add(identifier, initialValue);
        }

        public object GetValue(string identifier)
        {
            if (variables.ContainsKey(identifier)) return variables[identifier];
            else if (ParentScope != null) return ParentScope.GetValue(identifier);
            else throw new NotImplementedException();
        }

        public void SetValue(string identifier, object value)
        {
            if (variables.ContainsKey(identifier)) variables[identifier] = value;
            else if (ParentScope != null && ParentScope.ContainsIdentifier(identifier))
            {
                ParentScope.SetValue(identifier, value);
            }
        }
    }
}
