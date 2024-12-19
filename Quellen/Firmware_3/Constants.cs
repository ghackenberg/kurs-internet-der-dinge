namespace Firmware
{
    internal class Constants
    {
        // Hostname oder IP-Adresse des Thingsboard-MQTT-Servers
        public static string THINGSBOARD_HOST = "192.168.0.102";
        // Portnummer des Thingsboard-MQTT-Brokers
        public static int THINGSBOARD_PORT = 1883;
        // Geräteschlüssel für die Authentifizierung
        public static string THINGSBOARD_TOKEN = "s833bD55HhzFbCjwxvLq";

        // Hostname oder IP-Adresse des Modbus-Servers
        public static string LOGO_HOST = "192.168.0.55";
        // Portnummer des Modbus-Servers
        public static int LOGO_PORT = 502;
    }
}
