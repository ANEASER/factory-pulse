namespace FactoryPulse_Core.Entities;
using System.ComponentModel.DataAnnotations;

public class WorkOrder
{
    [Key]
    public int OrderID { get; set; }
    public int MachineID { get; set; }
    public string IssueType { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }

    // Navigation property
    public Machine? Machine { get; set; }
}
