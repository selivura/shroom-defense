using System;

namespace ShroomDefense.Gameplay
{
    /// <summary>
    /// Flat => Добавить плоское количество к стату<br></br>
    /// PercentAdd => Увеличить стат на x%, но проценты не умножаются, а складываются. 
    /// (тоесть если база 10 и мы прибавим два раза по 5%, то получим стат 11)
    /// PercentMult => Увеличить стат на x%, но проценты умножаются.
    /// </summary>
    public enum StatModType
    {
        Flat,
        PercentAdd,
        PercentMult,
    }
    [Serializable]
    public class StatModifier
    {
        public float Value;
        public StatModType Type;
        public int Order;
        [NonSerialized] public object Source;

        public StatModifier()
        {

        }
        public StatModifier(StatModifier copyFrom)
        {
            Value = copyFrom.Value;
            Type = copyFrom.Type;
            Order = copyFrom.Order;
            Source = copyFrom.Source;
        }
        public StatModifier(float value, StatModType type)
        {
            Value = value;
            Type = type;
        }
        public StatModifier(float value, StatModType type, int order)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = null;
        }
        public StatModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }
        public new string ToString()
        {
            if (Value > 0)
            {
                switch (Type)
                {
                    case StatModType.Flat:
                        return Value.ToString();
                    case StatModType.PercentAdd:
                        return Value + "%";
                    case StatModType.PercentMult:
                        return "x" + Value + "%";
                    default:
                        break;
                }
            }
            return "";
        }

        /// <summary>
        /// Использовать, чтобы добавить источник к стат модифаеру, 
        /// на случай если стат модифаер задается из инспектора, 
        /// где источник нельзя указать
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public StatModifier WithSource(object source)
        {
            Source = source;
            return this;
        }

        public StatModifier WithValue(float value)
        {
            Value = value;
            return this;
        }

        public StatModifier WithOrder(int order)
        {
            Order = order;
            return this;
        }

        public StatModifier WithType(StatModType type)
        {
            Type = type;
            return this;
        }
    }

}
