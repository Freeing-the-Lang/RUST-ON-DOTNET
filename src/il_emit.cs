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
            il.Emit(OpCodes.Ldstr, "Rust-on-Dotnet IL: OK");
            il.Emit(OpCodes.Call, typeof(Console)
                .GetMethod("WriteLine", new[] { typeof(string) }));
            il.Emit(OpCodes.Ret);

            var t = type.CreateType();
            t.GetMethod("main")?.Invoke(null, null);
        }
    }
}
