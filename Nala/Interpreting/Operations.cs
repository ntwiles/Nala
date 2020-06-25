using System;
using System.Collections.Generic;

using NathanWiles.Nala.Parsing;
using NathanWiles.Nala.Parsing.Nodes;
using NathanWiles.Nala.Errors;

namespace NathanWiles.Nala.Interpreting
{
    public static class Operations
    {
        // Assignment Operations
        public static void DoAssign(Scope scope, AssignmentNode assign, object value)
        {
            string identifier = assign.identifier.name;
            object currentVal = scope.GetValue(identifier);

            if (assign.isArray)
            {
                if (currentVal.GetType().GetElementType() != value.GetType())
                {
                    new RuntimeError("Cannot assign value of type \"" + value.GetType() +
                        "\" to variable \"" + identifier + "\" of type \"" + currentVal.GetType() + "\"").Report();
                    return;
                }
            }
            else
            {
                if (currentVal.GetType() != value.GetType())
                {
                    new RuntimeError("Cannot assign value of type \"" + value.GetType() +
                        "\" to variable \"" + identifier + "\" of type \"" + currentVal.GetType() + "\"").Report();
                    return;
                }
            }

            switch (assign.@operator.symbol)
            {
                case "=": doDefaultAssign(scope, assign, value); break;
                case "+=": doPlusEqualsAssign(scope, assign, value); break;
                case "*=": doTimesEqualsAssign(scope, assign, value); break;
                case "/=": doDivideByEqualsAssign(scope, assign, value); break;
                default: throw new NotImplementedException();
            }
        }

        private static void doDefaultAssign(Scope scope, AssignmentNode assign, object resolved)
        {
            string identifier = assign.identifier.name;
            object currentVal = scope.GetValue(identifier);

            if (assign.isArray)
            {
                int index = ((IntNode)assign.indexArgument).value;

                switch (currentVal)
                {
                    case int[] i:
                        {
                            int[] array = (int[])currentVal;
                            array[index] = (int)resolved; 
                            break;
                        }
                    case string[] s:
                        {
                            string[] array = (string[])currentVal;
                            array[index] = (string)resolved;
                            break;
                        }
                    case bool[] b:
                        {
                            bool[] array = (bool[])currentVal;
                            array[index] = (bool)resolved;
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
            else
            {
                scope.SetValue(identifier,resolved);
            }
        }

        private static void doPlusEqualsAssign(Scope scope, AssignmentNode assign, object resolved)
        {
            string identifier = assign.identifier.name;
            object currentVal = scope.GetValue(identifier);

            if (assign.isArray)
            {
                int index = ((IntNode)assign.indexArgument).value;

                switch (currentVal)
                {
                    case int[] i:
                        {
                            int[] array = (int[])currentVal;
                            array[index] = array[index] + (int)resolved;
                            break;
                        }
                    case string[] s:
                        {
                            string[] array = (string[])currentVal;
                            array[index] = array[index] + (string)resolved;
                            break;
                        }
                    case bool[] b:
                        {
                            new RuntimeError("Cannot use operator \"+=\" on bool values.").Report(); return;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
            else
            {
                switch (currentVal)
                {
                    case int i: scope.SetValue(identifier,(int)currentVal + (int)resolved); break;
                    case string s: scope.SetValue(identifier,(string)currentVal + (string)resolved); break;
                    case bool b: new RuntimeError("Cannot use operator \"+=\" on bool values.").Report(); return;
                }
            }
        }

        private static void doTimesEqualsAssign(Scope scope, AssignmentNode assign, object resolved)
        {
            throw new NotImplementedException();
        }

        private static void doDivideByEqualsAssign(Scope scope, AssignmentNode assign, object resolved)
        {
            throw new NotImplementedException();
        }

        // Arithmatic Operations

        public static object DoAdd(object leftOperand, object rightOperand)
        {
            if (leftOperand is int) return (int)leftOperand + (int)rightOperand;
            if (leftOperand is string) return (string)leftOperand + (string)rightOperand;

            return null;
        }

        public static object DoSubtract(object leftOperand, object rightOperand)
        {
            if (leftOperand is int) return (int)leftOperand - (int)rightOperand;
            if (leftOperand is string)
            {
                new RuntimeError("Cannot use \"-\" operator between two strings.").Report();
            }

            return null;
        }

        public static object DoMultiply(object leftOperand, object rightOperand)
        {
            if (leftOperand is int) return (int)leftOperand * (int)rightOperand;
            if (leftOperand is string)
            {
                new RuntimeError("Cannot use \"*\" operator between two strings.").Report();
            }

            return null;
        }

        public static object DoDivide(object leftOperand, object rightOperand)
        {
            if (leftOperand is int) return (int)leftOperand / (int)rightOperand;
            if (leftOperand is string)
            {
                new RuntimeError("Cannot use \"/\" operator between two strings.").Report();
            }

            return null;
        }

        // Comparison Operations

        public static bool DoComparison(string symbol, object leftOperand, object rightOperand)
        {
            switch (symbol)
            {
                case "==": return doEqualsComparison(leftOperand, rightOperand);
                case "!=": return doNotEqualsComparison(leftOperand, rightOperand);
                case  ">": return doGreaterThanComparison(leftOperand, rightOperand);
                case  "<": return doLessThanComparison(leftOperand, rightOperand);
                case ">=": return doGreaterThanOrEqualToComparison(leftOperand, rightOperand);
                case "<=": return (float)leftOperand <= (float)rightOperand;
                default:
                    {
                        new RuntimeError("Cannot perform comparisons using \"" + symbol + "\" operator.").Report();
                        return false;
                    }
            }
        }
        
        private static bool doEqualsComparison(object leftOperand, object rightOperand)
        {
            if (leftOperand.GetType() != rightOperand.GetType()!)
            {
                new RuntimeError("Cannot compare between types \"" + leftOperand.GetType() + "\" and \"" + rightOperand.GetType() + "\".").Report();
                return false;
            }

            switch (leftOperand)
            {
                case bool b : return (bool)leftOperand == (bool)rightOperand;
                case int i: return (int)leftOperand == (int)rightOperand;
                case string s: return (string)leftOperand == (string)rightOperand;
            }

            return leftOperand == rightOperand;
        }

        private static bool doNotEqualsComparison(object leftOperand, object rightOperand)
        {
            if (leftOperand.GetType() != rightOperand.GetType()!)
            {
                new RuntimeError("Cannot compare between types \"" + leftOperand.GetType() + "\" and \"" + rightOperand.GetType() + "\".").Report();
                return false;
            }

            switch (leftOperand)
            {
                case bool b: return (bool)leftOperand != (bool)rightOperand;
                case int i: return (int)leftOperand != (int)rightOperand;
                case string s: return (string)leftOperand != (string)rightOperand;
            }

            return leftOperand == rightOperand;
        }

        private static bool doLessThanComparison(object leftOperand, object rightOperand)
        {
            if (leftOperand.GetType() != rightOperand.GetType()!)
            {
                new RuntimeError("Cannot compare between types \"" + leftOperand.GetType() + "\" and \"" + rightOperand.GetType() + "\".").Report();
                return false;
            }

            switch (leftOperand)
            {
                case bool b: new RuntimeError("Cannot use operator < between boolean values.").Report(); return false;
                case int i: return (int)leftOperand < (int)rightOperand;
                case string s:  new RuntimeError("Cannot use operator < between string values.").Report(); return false;
            }

            return false;
        }

        private static bool doGreaterThanComparison(object leftOperand, object rightOperand)
        {
            if (leftOperand.GetType() != rightOperand.GetType()!)
            {
                new RuntimeError("Cannot compare between types \"" + leftOperand.GetType() + "\" and \"" + rightOperand.GetType() + "\".").Report();
                return false;
            }

            switch (leftOperand)
            {
                case bool b: new RuntimeError("Cannot use operator > between boolean values.").Report(); return false;
                case int i: return (int)leftOperand > (int)rightOperand;
                case string s: new RuntimeError("Cannot use operator > between string values.").Report(); return false;
            }

            return false;
        }

        private static bool doGreaterThanOrEqualToComparison(object leftOperand, object rightOperand)
        {
            if (leftOperand.GetType() != rightOperand.GetType()!)
            {
                new RuntimeError("Cannot compare between types \"" + leftOperand.GetType() + "\" and \"" + rightOperand.GetType() + "\".").Report();
                return false;
            }

            switch (leftOperand)
            {
                case bool b: new RuntimeError("Cannot use operator >= between boolean values.").Report(); return false;
                case int i: return (int)leftOperand >= (int)rightOperand;
                case string s:  new RuntimeError("Cannot use operator >= between string values.").Report(); return false;
            }

            return false;
        }
    }
}
