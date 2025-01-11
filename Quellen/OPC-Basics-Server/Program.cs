using Opc.UaFx;
using Opc.UaFx.Server;

namespace OPC_Basics_Server
{
    internal class Program
    {
        // Zufallszahlengenerator

        private static Random RANDOM = new Random();

        // Ordnerknoten

        private static OpcFolderNode machine = new OpcFolderNode(new OpcName("Machine"));

        // Variablenknoten

        [OpcDataType(id: "A", namespaceIndex: 2)]
        struct A
        {
            public int X;
            public int Y;
            public int Z;
        };

        private static OpcDataVariableNode<A> test = new OpcDataVariableNode<A>(machine, "Test", new A { X = 1, Y = 2, Z = 3 });
        private static OpcDataVariableNode<bool> running = new OpcDataVariableNode<bool>(machine, "Running", true);
        private static OpcDataVariableNode<int> version = new OpcDataVariableNode<int>(machine, "Version", 1);
        private static OpcDataVariableNode<int> random = new OpcDataVariableNode<int>(machine, "Random", 0);
        private static OpcDataVariableNode<int> parameter = new OpcDataVariableNode<int>(machine, "Parameter", 0);

        // Methodenknoten

        private static OpcMethodNode methodA = new OpcMethodNode(machine, new OpcName("MethodA"), new Action(MethodA));
        private static OpcMethodNode methodB = new OpcMethodNode(machine, new OpcName("MethodB"), new Func<int>(MethodB));
        private static OpcMethodNode methodC = new OpcMethodNode(machine, new OpcName("MethodC"), new Func<int, int>(MethodC));

        // Hauptroutine

        static void Main(string[] args)
        {
            // Rückrufmethoden für Variablenknoten setzen

            parameter.WriteVariableValueCallback = WriteParameter;

            // OPC UA Server starten und Werte der Variablenknoten zyklisch aktualisieren

            using (var server = new OpcServer("opc.tcp://localhost:4840/", machine, new OpcDataTypeNode<A>()))
            {
                // OPC UA Server starten

                server.Start();

                Console.WriteLine("Server gestartet!");

                // Werte der Variablenknoten zyklisch aktualisieren

                while (true)
                {
                    random.Status.Update(OpcStatusCode.Good);
                    random.Timestamp = DateTime.UtcNow;
                    random.Value = RANDOM.Next();
                    random.ApplyChanges(server.SystemContext);

                    Thread.Sleep(1000);
                }
            }
        }

        // Rückrufmethoden für Variablenknoten

        private static OpcVariableValue<object> WriteParameter(OpcContext context, OpcVariableValue<object> value)
        {
            // Aufruf protokollieren
            Console.WriteLine($"Neuer Wert {value.Value}");
            // Wert zurückgeben (und somit setzen)
            return value;

            //return new OpcVariableValue<object>(parameter.Value, new DateTime(), new OpcStatus(OpcStatusCode.BadNotWritable));
        }

        // Methodenimplementierungen für Methodenknoten

        private static void MethodA()
        {
            // Aufruf protokollieren
            Console.WriteLine("Methode A aufgerufen!");
        }
        private static int MethodB()
        {
            // Aufruf protokollieren
            Console.WriteLine("Methode B aufgerufen!");
            // Wert zurückgeben
            return 0;
        }
        private static int MethodC(int param)
        {
            // Aufruf protokollieren
            Console.WriteLine($"Methode C mit Parameter {param} aufgerufen!");
            // Wert berechnen und zurückgeben
            return param * 2;
        }
    }
}
