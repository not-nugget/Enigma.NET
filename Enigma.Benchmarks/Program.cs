// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;

using Enigma.Machine;

if (args is not ["-nobench"])
{
    BenchmarkSwitcher
        .FromAssembly(typeof(Program).Assembly)
        .RunAllJoined(null, args);
    return;
}

// var fields = typeof(Alphabet)
//     .GetFields(BindingFlags.Public | BindingFlags.Static)
//     .Select(f => (f, (Alphabet)f.GetValue(null)!));
// foreach (var (f, a) in fields)
// {
//     Console.WriteLine($"{f.DeclaringType!.Name}.{f.Name}: {a.Upper} {a.Lower} / {a.Value}");
// }

// var pegboard = new Plugboard();
//
// pegboard.Plug(Alphabet.D, Alphabet.U);
// pegboard.Plug(Alphabet.A, Alphabet.N);
// pegboard.Plug(Alphabet.F, Alphabet.Q);
// pegboard.Plug(Alphabet.G, Alphabet.Z);
//
// var a = Alphabet.A;
// pegboard.Process(ref a);
//
// pegboard.Unplug(Alphabet.U);
// pegboard.Unplug(Alphabet.A);
//
// a = Alphabet.A;
// pegboard.Process(ref a);
//
// _ = a;

var rotor = Rotor.Default;
rotor = new Rotor([], 0);
_ = rotor;