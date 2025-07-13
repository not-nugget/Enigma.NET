// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;

using Enigma.Machine;
using Enigma.Machine.Rotors;

if (args is not ["-nobench"])
{
    BenchmarkSwitcher
        .FromAssembly(typeof(Program).Assembly)
        .RunAllJoined(null, args);
    return;
}

Rotor.Parse("ABCDEFGHIJKLMNOPQRSTUVWXYZA21");