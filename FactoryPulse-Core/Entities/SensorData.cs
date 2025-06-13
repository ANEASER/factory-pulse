using System.ComponentModel.DataAnnotations;

namespace FactoryPulse_Core.Entities;

public class SensorData
{
    [Key]
    public int DataID { get; set; }
    public int MachineID { get; set; }
    public float Temperature { get; set; }
    public float Vibration { get; set; }
    public float Pressure { get; set; }
    public float Humidity { get; set; }
    public float RPM { get; set; }
    public DateTime Timestamp { get; set; }
    // Navigation property
    public Machine? Machine { get; set; }
}
