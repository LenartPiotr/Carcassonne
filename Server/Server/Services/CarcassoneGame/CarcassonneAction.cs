namespace Server.Services.CarcassoneGame
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CarcassonneAction: Attribute
    {
        public string Name { get; set; }
        public CarcassonneAction(string name) {
            Name = name;
        }
        public CarcassonneAction()
        {
            Name = "none";
        }
    }
}
