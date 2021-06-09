using System.Diagnostics.CodeAnalysis;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PRUNner.Backend.BasePlanner
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ProvidedConsumables : ReactiveObject
    {
        [Reactive] public bool DW { get; set; } = true;
        [Reactive] public bool RAT { get; set; } = true;
        
        [Reactive] public bool OVE { get; set; } = true;
        
        [Reactive] public bool EXO { get; set; } = true;
        [Reactive] public bool PT { get; set; } = true;
        
        [Reactive] public bool MED { get; set; } = true;
        [Reactive] public bool HMS { get; set; } = true;
        [Reactive] public bool SCN { get; set; } = true;
        
        [Reactive] public bool FIM { get; set; } = true;
        [Reactive] public bool HSS { get; set; } = true;
        [Reactive] public bool PDA { get; set; } = true;
        
        [Reactive] public bool MEA { get; set; } = true;
        [Reactive] public bool LC { get; set; } = true;
        [Reactive] public bool WS { get; set; } = true;
        
        [Reactive] public bool COF { get; set; }
        [Reactive] public bool PWO { get; set; }
        
        [Reactive] public bool KOM { get; set; }
        [Reactive] public bool REP { get; set; }
        
        [Reactive] public bool ALE { get; set; }
        [Reactive] public bool SC { get; set; }
        
        [Reactive] public bool GIN { get; set; }
        [Reactive] public bool VG { get; set; }
        
        [Reactive] public bool WIN { get; set; }
        [Reactive] public bool NST { get; set; }
    }
}