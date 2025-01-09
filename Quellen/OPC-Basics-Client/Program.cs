using Opc.UaFx;
using Opc.UaFx.Client;

namespace OPC_Basics_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Lokale Variablen definieren

            OpcNodeInfo info;

            OpcValue value;

            OpcStatus status;

            object[] result;

            // OPC UA Client erstellen

            using (var client = new OpcClient("opc.tcp://localhost:4840"))
            {
                // Connect
                client.Connect();

                // Browse nodes
                info = client.BrowseNode(OpcObjectTypes.ObjectsFolder);
                Print(info);

                // Read node
                value = client.ReadNode("ns=2;s=Machine/Running");
                Console.WriteLine($"[opc-ua-client] Read node = {value}");

                // Read node attribute
                value = client.ReadNode("ns=2;s=Machine/Running", OpcAttribute.DisplayName);
                Console.WriteLine($"[opc-ua-client] Read node display name = {value}");

                // Write node
                status = client.WriteNode("ns=2;s=Machine/Parameter", 0);
                Console.WriteLine($"[opc-ua-client] Write node = {status}");

                // Write node attribute
                status = client.WriteNode("ns=2;s=Machine/Parameter", OpcAttribute.DisplayName, "Parameter");
                Console.WriteLine($"[opc-ua-client] Write node display name = {status}");

                // Read/write node
                for (var index = 0; index < 10; index++)
                {
                    value = client.ReadNode("ns=2;s=Machine/Parameter");
                    Console.WriteLine($"[opc-ua-client] Read node = {value}");

                    if (value.Status.IsGood && value.DataType == OpcDataType.Int32)
                    {
                        status = client.WriteNode("ns=2;s=Machine/Parameter", "Test");//(int) value.Value + 1);
                        Console.WriteLine($"[opc-ua-client] Write node = {status}");
                    }

                }

                // Call method
                result = client.CallMethod("ns=2;s=Machine", "ns=2;s=Machine/MethodA");
                Console.WriteLine($"[opc-ua-client] Call method");

                // Call method with output argument
                result = client.CallMethod("ns=2;s=Machine", "ns=2;s=Machine/MethodB");
                Console.WriteLine($"[opc-ua-client] Call method with output argument = {result[0]}");

                // Call method with input/output argument
                result = client.CallMethod("ns=2;s=Machine", "ns=2;s=Machine/MethodC", 10);
                Console.WriteLine($"[opc-ua-client] Call method with input/output argument = {result[0]}");
            }

            // Wait for enter key
            Console.Write("[opc-ua-client] Press enter to exit");
            Console.ReadLine();
        }

        static void Print(OpcNodeInfo parent, int level = 0)
        {
            var space = new string(' ', level);
            var id = parent.NodeId;
            var name = parent.Attribute(OpcAttribute.DisplayName).Value;

            Console.WriteLine($"[opc-ua-client]{space}{id} {name}");

            foreach (var child in parent.Children())
            {
                Print(child, level + 1);
            }
        }
    }
}
