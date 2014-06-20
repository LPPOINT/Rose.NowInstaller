using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MonoTorrent.BEncoding;
using Rose.NowInstaller.Core.Instructions.InstructionsLibrary;

namespace Rose.NowInstaller.Core.Instructions
{
    public class InstructionsList : List<Instruction>
    {


        public delegate void InstructionDelegate(IInstructionExecutionContext context);

        public InstructionsList()
        {
        }

        public InstructionsList(int capacity) : base(capacity)
        {
        }

        public InstructionsList([NotNull] IEnumerable<Instruction> collection) : base(collection)
        {
        }

        public static InstructionsList FromBEncode(BEncodedList list)
        {
            var instructions = new InstructionsList();
            foreach (var item in list)
            {
                instructions.Add(Instruction.FromBEncoded(item));
            }
            return instructions;
        }

        public BEncodedList ToBEncode()
        {
            var list = new BEncodedList();
            foreach (var item in this)
            {
                list.Add(item.ToBEncoded());
            }
            return list;
        }

        public void Add(Instruction instruction, InstructionTrigger trigger)
        {
            instruction.Trigger = trigger;
            Add(instruction);
        }

        public void AddAfterLast(Instruction instruction)
        {
            var last = this.LastOrDefault();
            if(last == null)
                Add(instruction);
            else
            {
                instruction.Trigger = last.AfterExecution;
                Add(instruction);
            }
        }

        public void Add(InstructionTrigger trigger, InstructionDelegate instructionDelegate)
        {

        }

        public void CastToMapping()
        {
            for (int i = 0; i < this.Count; i++)
            {
                var item = this[i];
                if (!item.IsMapped && InstructionsMapping.IsExistId(item.Id))
                {
                    this[i] = Activator.CreateInstance(InstructionsMapping.GetTypeForId(item.Id), item.Data) as Instruction;
                }
            }
        }
    }
}
