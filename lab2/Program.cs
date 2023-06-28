using System;
using System.Collections.Generic;

enum State
{
    S0,
    S1,
    Halt
}

class TuringMachine
{
    private HashSet<char> alphabet;
    private Dictionary<Tuple<State, char>, Tuple<char, State, int>> transitions;
    private Dictionary<int, char> tape;
    private int headIndex;
    private State currentState;

    public TuringMachine(HashSet<char> alphabet, Dictionary<Tuple<State, char>, Tuple<char, State, int>> transitions)
    {
        this.alphabet = alphabet;
        this.transitions = transitions;
    }

    public void SetTape(string input)
    {
        tape = new Dictionary<int, char>();
        for (int i = 0; i < input.Length; i++)
        {
            if (!alphabet.Contains(input[i]))
            {
                throw new ArgumentException("Invalid input character: " + input[i]);
            }
            tape[i] = input[i];
        }
    }

    public void SetHeadPosition(int position)
    {
        headIndex = position;
    }

    public void SetInitialState(State state)
    {
        currentState = state;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("Current tape: " + GetTapeString());
            Console.WriteLine("Current state: " + currentState);

            Tuple<char, State, int> transition;
            if (!transitions.TryGetValue(Tuple.Create(currentState, GetHeadSymbol()), out transition))
            {
                Console.WriteLine("No transition found for current state and tape symbol.");
                break;
            }

            tape[headIndex] = transition.Item1;
            headIndex += transition.Item3;

            currentState = transition.Item2;

            if (currentState == State.Halt)
            {
                Console.WriteLine("Machine halted.");
                break;
            }
        }

        Console.WriteLine("Result: " + GetTapeString().TrimEnd('_'));
    }

    private char GetHeadSymbol()
    {
        if (tape.ContainsKey(headIndex))
        {
            return tape[headIndex];
        }
        else
        {
            return '_'; // Empty symbol
        }
    }

    private string GetTapeString()
    {
        string tapeString = string.Empty;
        for (int i = 0; i < tape.Count; i++)
        {
            tapeString += tape[i];
        }
        return tapeString;
    }
}

class Program
{
    static void Main()
    {
        // Задаємо алфавіт
        HashSet<char> alphabet = new HashSet<char> { '0', '1', 'e', 'o' };

        // Задаємо матрицю переходів
        Dictionary<Tuple<State, char>, Tuple<char, State, int>> transitions = new Dictionary<Tuple<State, char>, Tuple<char, State, int>>
        {
            { Tuple.Create(State.S0, '0'), Tuple.Create('1', State.S1, 1) },
            { Tuple.Create(State.S0, '1'), Tuple.Create('0', State.S1, 1) },
            { Tuple.Create(State.S0, 'e'), Tuple.Create('o', State.S1, 1) },
            { Tuple.Create(State.S0, 'o'), Tuple.Create('o', State.S1, 1) },
            { Tuple.Create(State.S1, '0'), Tuple.Create('0', State.S0, -1) },
            { Tuple.Create(State.S1, '1'), Tuple.Create('1', State.S1, 1) },
            { Tuple.Create(State.S1, 'e'), Tuple.Create('e', State.Halt, 1) },
            { Tuple.Create(State.S1, 'o'), Tuple.Create('o', State.S1, 1) }
        };

        TuringMachine machine = new TuringMachine(alphabet, transitions);
        machine.SetTape("000111");
        machine.SetHeadPosition(0);
        machine.SetInitialState(State.S0);
        machine.Run();
    }
}
