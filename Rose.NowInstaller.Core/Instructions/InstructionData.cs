using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using MonoTorrent.BEncoding;

namespace Rose.NowInstaller.Core.Instructions
{
    public enum InstructionDataValueType
    {
        Number,
        String,
        List,
        Dictionary,
        Null
    }

    public class InstructionIncorrectDataTypeException : Exception
    {
        public InstructionIncorrectDataTypeException()
        {
        }

        public InstructionIncorrectDataTypeException(string message) : base(message)
        {
        }

        public InstructionIncorrectDataTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InstructionIncorrectDataTypeException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }


    public class InstructionData
    {



        public static InstructionData Number(long n)
        {
            return new InstructionData(n);
        }
        public static InstructionData String(string s)
        {
            return new InstructionData(s);
        }
        public static InstructionData List(List<InstructionData> l)
        {
            return new InstructionData(l);
        }
        public static InstructionData Dictionary(Dictionary<string, InstructionData> d)
        {
            return new InstructionData(d);
        }

        public static InstructionData FromBEncoded(BEncodedValue value)
        {
            try
            {
                if (value is BEncodedNumber)
                {
                    var asNumber = value as BEncodedNumber;
                    return new InstructionData(asNumber.Number);
                }
                if (value is BEncodedString)
                {
                    var asString = value as BEncodedString;
                    return new InstructionData(asString.Text);
                }
                if (value is BEncodedList)
                {
                    var asList = value as BEncodedList;
                    return new InstructionData(BEncodedListToInstructionDataList(asList));
                }
                if (value is BEncodedDictionary)
                {
                    var asDictionary = value as BEncodedDictionary;
                    return new InstructionData(BEncodedDictionaryToInstructionDataDictionary(asDictionary));
                }

                return null;
            }
            catch (Exception e)
            {
                throw new InstructionDecodingException("Не удалось раскодировать данные инструкции", e);
            }
        }

        private static IList<InstructionData> BEncodedListToInstructionDataList(IEnumerable<BEncodedValue> list)
        {
            return list.Select(FromBEncoded).ToList();
        }

        private static IDictionary<string, InstructionData> BEncodedDictionaryToInstructionDataDictionary(
            IEnumerable<KeyValuePair<BEncodedString, BEncodedValue>> dictionary)
        {
            return dictionary.ToDictionary(item => item.Key.Text, item => FromBEncoded(item.Value));
        }

        private static BEncodedValue ToBEncoded(InstructionData data)
        {
            if(data == null)
                return null;
            switch (data.ValueEType)
            {
                case InstructionDataValueType.Number:
                    return new BEncodedNumber((long) data.Value);
                case InstructionDataValueType.String:
                    return new BEncodedString((string)data.Value);
                case InstructionDataValueType.List:
                {
                    var result = data.AsList().Select(ToBEncoded).ToList();
                    return new BEncodedList(result);
                }
                case InstructionDataValueType.Dictionary:
                {
                    var result = new BEncodedDictionary();
                    foreach (var item in data.AsDictionary())
                    {
                        result.Add(new BEncodedString(item.Key), ToBEncoded(item.Value));
                    }
                    return result;
                }
                case InstructionDataValueType.Null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static readonly InstructionData Empty = new InstructionData(null);

        public InstructionData(object value)
        {

            Value = value;
        }

        public object Value { get; private set; }

        public InstructionDataValueType ValueEType
        {
            get { return GetEnumValueForType(ValueType); }
        }

        public Type ValueType
        {
            get
            {
                if (Value == null)
                    return null;
                return Value.GetType();
            }
        }

        public bool IsSingle
        {
            get
            {
                return ValueEType == InstructionDataValueType.Number || ValueEType == InstructionDataValueType.String;
            }
        }

        public long AsNumber()
        {
            return As<long>();
        }

        public string AsString()
        {
            return As<string>();
        }

        public List<InstructionData> AsList()
        {
            return As<List<InstructionData>>();
        }

        public Dictionary<string, InstructionData> AsDictionary()
        {
            return As<Dictionary<string, InstructionData>>();
        }

        public T As<T>() 
        {
            if (!IsCorrectType(typeof (T)))
                return default(T);

            try
            {
                var value = (T) Value;
                return value;
            }
            catch
            {
                return default(T);
            }

        }

        public BEncodedValue ToBEncoded()
        {
            if(Value == null)
                return null;
            return ToBEncoded(this);
        }

        private static readonly List<Type> correctTypes = new List<Type>()
                                                 {
                                                     typeof(long),
                                                     typeof(string),
                                                     typeof(Dictionary<string, InstructionData>),
                                                     typeof(List<InstructionData>)
                                                 };  

        public static bool IsCorrectType(Type type)
        {
            return correctTypes.Contains(type);
        }

        private static Type GetTypeForEnumValue(InstructionDataValueType enumValue)
        {
            switch (enumValue)
            {
                case InstructionDataValueType.Number:
                    return typeof(long);
                case InstructionDataValueType.String:
                    return typeof(string);
                case InstructionDataValueType.List:
                    return typeof(List<InstructionData>);
                case InstructionDataValueType.Dictionary:
                    return typeof(Dictionary<string, InstructionData>);
                default:
                    return null;
            }
        }

        private static InstructionDataValueType GetEnumValueForType(Type type)
        {

            if (type == null) return InstructionDataValueType.Null;

            if (type == typeof(long)) return InstructionDataValueType.Number;

            if (type == typeof(string)) return InstructionDataValueType.String;

            if (type == typeof(List<InstructionData>)) return InstructionDataValueType.List;

            if (type == typeof(Dictionary<string, InstructionData>)) return InstructionDataValueType.Dictionary;

            return InstructionDataValueType.Null;

        }
    }
}
