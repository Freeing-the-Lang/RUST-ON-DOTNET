using System;
using System.Reflection;
using System.Reflection.Emit;

namespace RustOnDotnet
{
    public class ILEmit
    {
        public void EmitHello()
        {
            var asm = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("RustDotnetGen"),
                AssemblyBuilderAccess.RunAndCollect);

            var mod = asm.DefineDynamicModule("MainMod");
            var type = mod.DefineType("RustMain");

            var method = type.DefineMethod(
                "main",
                MethodAttributes.Public | MethodAttributes.Static);

            var il = method.GetILGenerator();

            var writeLine = typeof(Console).GetMethod(
                "WriteLine",
                new[] { typeof(string) }
            )!; // FIX — null-forgiving operator

            il.Emit(OpCodes.Ldstr, "Rust-on-Dotnet IL: OK");
            il.Emit(OpCodes.Call, writeLine);
            il.Emit(OpCodes.Ret);

            var finalType = type.CreateType();
            finalType?.GetMethod("main")?.Invoke(null, null);
        }
    }
}

