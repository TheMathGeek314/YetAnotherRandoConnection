using ItemChanger;
using ItemChanger.Modules;

namespace YetAnotherRandoConnection
{
    public class YARCModule : Module
    {
        public override void Initialize() { }
        public override void Unload() { }
        public delegate void ItemObtained(string item);
        public static event ItemObtained OnItemObtained;
        public static YARCModule Instance => ItemChangerMod.Modules.GetOrAdd<YARCModule>();
        public int Bombs = 0;
        public int Orbs = 0;
        public void RecordItem(string item)
        {
            OnItemObtained?.Invoke(item);
        }
    }
}