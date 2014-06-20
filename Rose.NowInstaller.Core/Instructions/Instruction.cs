using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MonoTorrent.BEncoding;
using NLog;

namespace Rose.NowInstaller.Core.Instructions
{
    public  class Instruction : ITriggerable
    {


        public static Instruction FromBEncoded(BEncodedValue value, Type instructionType)
        {
            try
            {
                if (value is BEncodedNumber)
                {
                    return Activator.CreateInstance(instructionType, InstructionData.Empty) as Instruction;
                }
                if (value is BEncodedList)
                {
                    var asList = value as BEncodedList;

                    if (asList.Count != 2 || !(asList.First() is BEncodedNumber))
                        throw new InstructionDecodingException("Неверный формат инструкции");

                    var id = (int)(asList.First() as BEncodedNumber).Number;
                    var data = InstructionData.FromBEncoded(asList.Last());

                    return Activator.CreateInstance(instructionType,  data) as Instruction;

                }
            }
            catch (Exception e)
            {
                throw new InstructionDecodingException("Не удалось раскодировать инструкцию", e);
            }

            return null;
        }

        public static T FromBEncoded<T>(BEncodedValue value) where T : Instruction
        {
            return FromBEncoded(value, typeof (T)) as T;
        }

        public Type MappingType
        {
            get { return InstructionsMapping.GetTypeForId(Id); }
        }

        public bool IsMapped
        {
            get { return GetType() == InstructionsMapping.GetTypeForId(Id); }
        }

        public InstructionTrigger Trigger { get; set; }

        internal virtual bool IsAsync
        {
            get { return false; }
        }

        public InstructionTrigger AfterExecution { get; private set; }

        private static int DetectBEncodedInstructionId(BEncodedValue value)
        {
            if (value is BEncodedList)
            {
                var asList = value as BEncodedList;
                var first = asList.First();

                if (first is BEncodedNumber)
                {
                    return (int)(first as BEncodedNumber).Number;
                }
            }

            return -1;
        }

        public static Instruction FromBEncoded(BEncodedValue value)
        {

            var id = DetectBEncodedInstructionId(value);
            if (id == -1)
                return null;
            var type = InstructionsMapping.GetTypeForId(id);

            return FromBEncoded(value, type);
        }

        protected Instruction(int id, InstructionData data)
        {
            Data = data;
            Id = id;
            AfterExecution = new InstructionTrigger();
            Wrap();
        }

        public Instruction(InstructionData data)
        {
            Data = data;
            AfterExecution = new InstructionTrigger();
            Id = 0;
        }


        public bool IsInstructionPropertiesExists
        {
            get
            {
                var res =
                    GetType()
                        .GetProperties()
                        .Where(info => info.CanWrite && info.GetCustomAttribute<InstructionPropertyAttribute>() != null)
                        .Where(info => InstructionData.IsCorrectType(info.PropertyType));
                return res.Any();

            }
        }

        public int Id { get; private set; }
        public InstructionData Data { get; private set; }

        public BEncodedValue ToBEncoded(bool useProps = false)
        {

            if (IsInstructionPropertiesExists && useProps)
            {
                
            }

            var result = new BEncodedList
                         {
                             new BEncodedNumber(Id),
                             Data.ToBEncoded()
                         };

            return result;

        }

        private void Set(PropertyInfo property, object value)
        {
            property.SetValue(this, value);
        }

        public void Wrap()
        {

            if(GetType() == typeof(Instruction))
                return;

            var allProps = GetType().GetProperties();
            var withAttr = allProps.Where(info => info.GetCustomAttribute<InstructionPropertyAttribute>() != null);
            var writeable = withAttr.Where(info => info.CanWrite);

            if (writeable.Count() == 1  && writeable.First().PropertyType == Data.ValueType)
            {
                var p = writeable.First();
                var a = p.GetCustomAttribute<InstructionPropertyAttribute>();

                if (a.IsUnnamed || a.Key == p.Name)
                {
                    Set(p, Data.Value);
                }
            }

            else if (Data.ValueEType == InstructionDataValueType.Dictionary)
            {

                var asDictionary = Data.AsDictionary();

                foreach (var propertyInfo in writeable)
                {
                    var a = propertyInfo.GetCustomAttribute<InstructionPropertyAttribute>();
                    var key = a == null ? propertyInfo.Name : a.Key;

                    foreach (var kv in asDictionary)
                    {
                        if (kv.Key == key)
                        {
                            Set(propertyInfo, kv.Value);
                        }
                    }

                }
            }
        }

        public virtual void Execute(IInstructionExecutionContext context)
        {
            LogManager.GetCurrentClassLogger().Info("Выполнена инструкция {0} ({1})", GetType().Name, Id);
            AfterExecution.Invoke();
        }
    }

    public abstract class AsyncInstruction : Instruction
    {
        protected AsyncInstruction(int id, InstructionData data) : base(id, data)
        {
        }

        protected AsyncInstruction(InstructionData data) : base(data)
        {
        }

        internal override bool IsAsync
        {
            get { return true; }
        }

        public sealed override void Execute(IInstructionExecutionContext context)
        {
            Task.Run(() => ExecuteAsync(context));
            base.Execute(context);
        }

        public abstract Task ExecuteAsync(IInstructionExecutionContext context);
    }
}
