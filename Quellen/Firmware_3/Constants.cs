namespace Firmware
{
    internal class Constants
    {
        // Hostname oder IP-Adresse des Thingsboard-MQTT-Servers
        public static string THINGSBOARD_HOST = "192.168.137.1";
        // Portnummer des Thingsboard-MQTT-Brokers
        public static int THINGSBOARD_PORT = 1883;
        // Geräteschlüssel für die Authentifizierung
        public static string THINGSBOARD_TOKEN = "bvdoxulzQYsYUshqfiSE";

        // Hostname oder IP-Adresse des Modbus-Servers
        public static string LOGO_HOST = "192.168.137.5";
        // Portnummer des Modbus-Servers
        public static int LOGO_PORT = 502;
    }
}
