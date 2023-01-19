using System;
using System.Collections.Generic;
namespace Command.RealWorld
{
    /// <summary>
    /// Patrón de diseño Command
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Crear usuario y dejar que calcule
            User user = new User();
            // El usuario pulsa los botones de la calculadora
            user.Compute('+', 100);
            user.Compute('-', 50);
            user.Compute('*', 10);
            user.Compute('/', 2);
            // Deshacer 4 comandos
            user.Undo(4);
            // Rehacer 3 comandos
            user.Redo(3);
            // Esperar al usuario
            Console.ReadKey();
        }
    }
    /// <summary>
    /// La clase abstracta 'Command'
    /// </summary>
    public abstract class Command
    {
        public abstract void Execute();
        public abstract void UnExecute();
    }
    /// <summary>
    /// La Clase 'ConcreteCommand'
    /// </summary>
    /// Y una clase de comando "CalculatorCommand" que es una clase concreta que
    /// implementa un comando para realizar una operación en la calculadora.
    public class CalculatorCommand : Command
    {
        char @operator;
        int operand;
        Calculator calculator;
        // Constructor
        public CalculatorCommand(Calculator calculator,
            char @operator, int operand)
        {
            this.calculator = calculator;
            this.@operator = @operator;
            this.operand = operand;
        }
        // Obtener el operador
        public char Operator
        {
            set { @operator = value; }
        }
        // Obtener el operando
        public int Operand
        {
            set { operand = value; }
        }
        // Ejecutar nuevo comando
        public override void Execute()
        {
            calculator.Operation(@operator, operand);
        }
        // Anular la ejecución de la última orden
        public override void UnExecute()
        {
            calculator.Operation(Undo(@operator), operand);
        }
        // Devuelve el operador opuesto al operador dado
        private char Undo(char @operator)
        {
            switch (@operator)
            {
                case '+': return '-';
                case '-': return '+';
                case '*': return '/';
                case '/': return '*';
                default:
                    throw new
             ArgumentException("@operator");
            }
        }
    }
    /// <summary>
    /// La clase 'Receiver'
    /// </summary>
    /// También se utiliza una clase "Calculator" como receptor que
    /// realiza las operaciones aritméticas.
    public class Calculator
    {
        int curr = 0;
        public void Operation(char @operator, int operand)
        {
            switch (@operator)
            {
                case '+': curr += operand; break;
                case '-': curr -= operand; break;
                case '*': curr *= operand; break;
                case '/': curr /= operand; break;
            }
            Console.WriteLine(
                "Valor actual = {0,3} (en {1} {2})",
                curr, @operator, operand);
        }
    }
    /// <summary>
    /// La clase 'Invoker'
    /// </summary>
    /// Se tiene una clase de usuario llamada User que tiene una calculadora
    /// y una lista de comandos. El usuario puede realizar operaciones aritméticas
    /// básicas en la calculadora mediante la ejecución de comandos, y también puede
    /// deshacer y rehacer comandos.
    public class User
    {
        // Inicializadores
        Calculator calculator = new Calculator();
        List<Command> commands = new List<Command>();
        int current = 0;
        public void Redo(int levels)
        {
            Console.WriteLine("\n---- Rehacer {0} niveles ", levels);
            // Realizar operaciones de rehacer
            for (int i = 0; i < levels; i++)
            {
                if (current < commands.Count - 1)
                {
                    Command command = commands[current++];
                    command.Execute();
                }
            }
        }
        public void Undo(int levels)
        {
            Console.WriteLine("\n---- Deshacer {0} niveles ", levels);

            // Realizar operaciones de deshacer
            for (int i = 0; i < levels; i++)
            {
                if (current > 0)
                {
                    Command command = commands[--current] as Command;
                    command.UnExecute();
                }
            }
        }
        public void Compute(char @operator, int operand)
        {
            // Crear operación de comando y ejecutarla
            Command command = new CalculatorCommand(calculator, @operator, operand);
            command.Execute();
            // Añadir comando a la lista de deshacer
            commands.Add(command);
            current++;
        }
    }
}