using System;

namespace nickeltin.GameData.DataObjects
{
    [Serializable]
    public class NumberReference
    {
        public bool UseConstant = true;
        public float ConstantValue;
        public NumberObject Variable;

        public NumberReference() { }

        public NumberReference(float value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public float Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator float(NumberReference reference)
        {
            return reference.Value;
        }
    }
}
