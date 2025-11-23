// File: il_emit.cs
// IL emitter skeleton (not generating real IL yet)

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace RustOnDotnet
{
    public class ILGeneratorPrototype
    {
        public void EmitHello()
        {
            var asm = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("RustDotnetGen"),
                AssemblyBuilderAccess.RunAndCollect);

            var module = asm.DefineDynamicModule("MainMod");
            var type = module.DefineType("RustMain");

            var method = type.DefineMethod(
                "main",
                MethodAttributes.Public | MethodAttributes.Static);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "Rust-on-Dotnet IL running!");
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }));
            il.Emit(OpCodes.Ret);

            var finalType = type.CreateType();

            // Invoke
            finalType.GetMethod("main")?.Invoke(null, null);
        }
    }
}
