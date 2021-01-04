using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomValues
{
    public enum ComparisonOperator
    {
        EqualTo,
        NotEqualTo,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo
    }

    public enum LogicalOperator
    {
        And,
        Or
    }

    [System.Serializable]
    public class CustomValueConditional
    {
        public CustomValueData value1;
        public ComparisonOperator comparisonOperator;
        public CustomValueData value2;

        public CustomValueConditional(CustomValueData newValue1, CustomValueData newValue2, ComparisonOperator newComparisonOperator)
        {
            value1 = newValue1;
            value2 = newValue2;
            comparisonOperator = newComparisonOperator;
        }

        public bool Validate()
        {
            if(value1 == null || value2 == null)
            {
                Debug.LogError("Custom value is null!");
                return false;
            }

            if(value1.type != value2.type)
            {
                Debug.LogError("Custom value type mismatch! Used " + value1.type.ToString() + " and " + value1.type.ToString());
                return false;
            }

            if(value1.type == CustomValueData.ValueType.Bool)
            {
                if(comparisonOperator != ComparisonOperator.EqualTo && comparisonOperator != ComparisonOperator.NotEqualTo)
                {
                    Debug.LogError("Invalid conditional used for Bool type! Used: " + comparisonOperator.ToString());
                    return false;
                }
            }
            else if(value1.type == CustomValueData.ValueType.String)
            {
                if (comparisonOperator != ComparisonOperator.EqualTo && comparisonOperator != ComparisonOperator.NotEqualTo)
                {
                    Debug.LogError("Invalid conditional used for String type! Used: " + comparisonOperator.ToString());
                    return false;
                }
            }

            return true;
        }

        public bool Evaluate()
        {
            if(Validate())
            {
                if (value1.type == CustomValueData.ValueType.Bool)
                {
                    if (comparisonOperator == ComparisonOperator.EqualTo)
                    {
                        return GetBool(value1) == GetBool(value2);
                    }
                    else
                    {
                        return GetBool(value1) != GetBool(value2);
                    }
                }
                else if(value1.type == CustomValueData.ValueType.Int)
                {
                    if(comparisonOperator == ComparisonOperator.EqualTo)
                    {
                        return GetInt(value1) == GetInt(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.NotEqualTo)
                    {
                        return GetInt(value1) != GetInt(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.GreaterThan)
                    {
                        return GetInt(value1) > GetInt(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.LessThan)
                    {
                        return GetInt(value1) < GetInt(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.GreaterThanOrEqualTo)
                    {
                        return GetInt(value1) >= GetInt(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.LessThanOrEqualTo)
                    {
                        return GetInt(value1) <= GetInt(value2);
                    }
                }
                else if (value1.type == CustomValueData.ValueType.Float)
                {
                    if (comparisonOperator == ComparisonOperator.EqualTo)
                    {
                        return GetFloat(value1) == GetFloat(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.NotEqualTo)
                    {
                        return GetFloat(value1) != GetFloat(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.GreaterThan)
                    {
                        return GetFloat(value1) > GetFloat(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.LessThan)
                    {
                        return GetFloat(value1) < GetFloat(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.GreaterThanOrEqualTo)
                    {
                        return GetFloat(value1) >= GetFloat(value2);
                    }
                    else if (comparisonOperator == ComparisonOperator.LessThanOrEqualTo)
                    {
                        return GetFloat(value1) <= GetFloat(value2);
                    }
                }
                else if (value1.type == CustomValueData.ValueType.String)
                {
                    if (comparisonOperator == ComparisonOperator.EqualTo)
                    {
                        return GetString(value1) == GetString(value2);
                    }
                    else
                    {
                        return GetString(value1) != GetString(value2);
                    }
                }
            }

            return false;
        }
    }

    private static Dictionary<CustomValueData, bool> _bools = new Dictionary<CustomValueData, bool>();
    private static Dictionary<CustomValueData, int> _ints = new Dictionary<CustomValueData, int>();
    private static Dictionary<CustomValueData, float> _floats = new Dictionary<CustomValueData, float>();
    private static Dictionary<CustomValueData, string> _strings = new Dictionary<CustomValueData, string>();
    
    //public static SecureEvent valuesUpdatedEvent = new SecureEvent();

    public static bool GetBool(CustomValueData customValue)
    {
        if(customValue.type != CustomValueData.ValueType.Bool)
        {
            Debug.LogError("Incorrect custom value type requested! Is a " + customValue.type.ToString() + ", expected Bool");
            return false;
        }

        if(!_bools.ContainsKey(customValue))
        {
            _bools.Add(customValue, false);
        }

        return _bools[customValue];
    }

    public static int GetInt(CustomValueData customValue)
    {
        if (customValue.type != CustomValueData.ValueType.Int)
        {
            Debug.LogError("Incorrect custom value type requested! Is a " + customValue.type.ToString() + ", expected Int");
            return 0;
        }

        if (!_ints.ContainsKey(customValue))
        {
            _ints.Add(customValue, 0);
        }

        return _ints[customValue];
    }

    public static float GetFloat(CustomValueData customValue)
    {
        if (customValue.type != CustomValueData.ValueType.Float)
        {
            Debug.LogError("Incorrect custom value type requested! Is a " + customValue.type.ToString() + ", expected Float");
            return 0f;
        }

        if (!_floats.ContainsKey(customValue))
        {
            _floats.Add(customValue, 0f);
        }

        return _floats[customValue];
    }

    public static string GetString(CustomValueData customValue)
    {
        if (customValue.type != CustomValueData.ValueType.String)
        {
            Debug.LogError("Incorrect custom value type requested! Is a " + customValue.type.ToString() + ", expected String");
            return "";
        }

        if (!_strings.ContainsKey(customValue))
        {
            _strings.Add(customValue, "");
        }

        return _strings[customValue];
    }

    public static void SetBool(CustomValueData customValue, bool boolValue)
    {
        if (customValue.type != CustomValueData.ValueType.Bool)
        {
            Debug.LogError("Incorrect custom value type requested! Is a " + customValue.type.ToString() + ", expected Bool");
            return;
        }

        if (_bools.ContainsKey(customValue))
        {
            _bools[customValue] = boolValue;
        }
        else
        {
            _bools.Add(customValue, boolValue);
        }

        //valuesUpdatedEvent.Invoke();
    }

    public static void SetInt(CustomValueData customValue, int intValue)
    {
        if (customValue.type != CustomValueData.ValueType.Int)
        {
            Debug.LogError("Incorrect custom value type requested! Is a " + customValue.type.ToString() + ", expected Int");
            return;
        }

        if (_ints.ContainsKey(customValue))
        {
            _ints[customValue] = intValue;
        }
        else
        {
            _ints.Add(customValue, intValue);
        }

        //valuesUpdatedEvent.Invoke();
    }

    public static void SetFloat(CustomValueData customValue, float floatValue)
    {
        if (customValue.type != CustomValueData.ValueType.Float)
        {
            Debug.LogError("Incorrect custom value type requested! Is a " + customValue.type.ToString() + ", expected Float");
            return;
        }

        if (_floats.ContainsKey(customValue))
        {
            _floats[customValue] = floatValue;
        }
        else
        {
            _floats.Add(customValue, floatValue);
        }

        //valuesUpdatedEvent.Invoke();
    }

    public static void SetString(CustomValueData customValue, string stringValue)
    {
        if (customValue.type != CustomValueData.ValueType.String)
        {
            Debug.LogError("Incorrect custom value type requested! Is a " + customValue.type.ToString() + ", expected String");
            return;
        }

        if (_strings.ContainsKey(customValue))
        {
            _strings[customValue] = stringValue;
        }
        else
        {
            _strings.Add(customValue, stringValue);
        }

        //valuesUpdatedEvent.Invoke();
    }

    public static bool EvaluateConditional(CustomValueData customValue1, CustomValueData customValue2, ComparisonOperator comparisonOperator)
    {
        return EvaluateConditional(new CustomValueConditional(customValue1, customValue2, comparisonOperator));
    }

    public static bool EvaluateConditional(CustomValueConditional conditional)
    {
        return conditional.Evaluate();
    }

    public static bool EvaluateConditionals(List<CustomValueConditional> conditionals, LogicalOperator logicalOperator)
    {
        if(logicalOperator == LogicalOperator.And)
        {
            foreach(CustomValueConditional customValueConditional in conditionals)
            {
                if(!customValueConditional.Evaluate())
                {
                    return false;
                }
            }

            return true;
        }
        else if(logicalOperator == LogicalOperator.Or)
        {
            foreach(CustomValueConditional customValueConditional in conditionals)
            {
                if(customValueConditional.Evaluate())
                {
                    return true;
                }
            }

            return false;
        }

        return false;
    }
}
