using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rose.NowInstaller.Core.Instructions
{

    public class InstructionAttribute : Attribute
    {
        public InstructionAttribute(int instructionId)
        {
            InstructionId = instructionId;
        }

        public int InstructionId { get; private set; }
    }

    public static class InstructionsMapping
    {

        public const int NumericInstructionId = 0xf;

        public const int AddRegistryValue = 0xff;
        public const int DeleteRegistryValue = 0xf1;

        public const int CustomInstruction= 0xf2;

        public const int StartDownload = 0xf3;
        public const int PauseDownload = 0xf3;


        public const int ChangeInstallStatus = 0xf4;

        public const int RunProcess = 0xf5;

        public const int Delay = 0xf6;
        public const int Delegate = 0xf7;

        private static readonly IDictionary<int, Type> mapping = Initialize();


        private static IDictionary<int, Type> Initialize()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes().Where(type => type.GetCustomAttribute<InstructionAttribute>() != null);
            var res = new Dictionary<int, Type>();
            foreach (var type in types)
            {
                var a = type.GetCustomAttribute<InstructionAttribute>();
                res.Add(a.InstructionId, type);
            }
            return res;
        }

        public static bool AddCustom(int id, Type type)
        {
            if (mapping.ContainsKey(id))
                return false;
            mapping.Add(id, type);
            return true;
        }

        public static Type GetTypeForId(int id)
        {
            try
            {
                return mapping[id];
            }
            catch 
            {
                return null;
            }
        }

        public static int GetIdForType(Type type)
        {
            foreach (var kv in mapping.Where(kv => kv.Value == type))
            {
                return kv.Key;
            }
            return -1;
        }

        public static bool IsExistId(int id)
        {
            return GetTypeForId(id) != null;
        }

        public static bool IsExistType(Type type)
        {
            return GetIdForType(type) != -1;
        }
    }
}
