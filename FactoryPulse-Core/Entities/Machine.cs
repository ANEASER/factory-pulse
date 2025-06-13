using System.ComponentModel.DataAnnotations;

namespace FactoryPulse_Core.Entities;

public class Machine
{
    [Key]
    public int MachineID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime LastMaintenance { get; set; }

    // Navigation properties
    public ICollection<SensorData> SensorData { get; set; } = new List<SensorData>();
    public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
}
