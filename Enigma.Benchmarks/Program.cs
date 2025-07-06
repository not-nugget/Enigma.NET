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

var rotor = new Rotor([
        (Alphabet.A, Alphabet.Z),
        (Alphabet.B, Alphabet.Y),
        (Alphabet.C, Alphabet.X),
        (Alphabet.D, Alphabet.W),
        (Alphabet.E, Alphabet.V),
        (Alphabet.F, Alphabet.U),
        (Alphabet.G, Alphabet.T),
        (Alphabet.H, Alphabet.S),
        (Alphabet.I, Alphabet.R),
        (Alphabet.J, Alphabet.Q),
        (Alphabet.K, Alphabet.P),
        (Alphabet.L, Alphabet.O),
        (Alphabet.M, Alphabet.N),
        (Alphabet.N, Alphabet.M),
        (Alphabet.O, Alphabet.L),
        (Alphabet.P, Alphabet.K),
        (Alphabet.Q, Alphabet.J),
        (Alphabet.R, Alphabet.I),
        (Alphabet.S, Alphabet.H),
        (Alphabet.T, Alphabet.G),
        (Alphabet.U, Alphabet.F),
        (Alphabet.V, Alphabet.E),
        (Alphabet.W, Alphabet.D),
        (Alphabet.X, Alphabet.C),
        (Alphabet.Y, Alphabet.B),
        (Alphabet.Z, Alphabet.A),
    ],
    0
);

rotor = new Rotor([], 0);

_ = rotor;