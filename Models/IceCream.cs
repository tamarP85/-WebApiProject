namespace WebApiProject.Models;
public class IceCream:GenericModel{
    
    public string Name { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public int AgentId { get; set; }
}