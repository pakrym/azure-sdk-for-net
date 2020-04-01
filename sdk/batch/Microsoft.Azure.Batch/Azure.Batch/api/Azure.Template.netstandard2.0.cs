namespace Azure.Batch
{
    public partial class TemplateClient
    {
        protected TemplateClient() { }
        public TemplateClient(System.Uri endpoint) { }
        public TemplateClient(System.Uri endpoint, Azure.Batch.TemplateClientOptions options) { }
        public virtual Azure.Response<Azure.Batch.Models.Model> Operation(Azure.Batch.Models.Model body, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
        public virtual System.Threading.Tasks.Task<Azure.Response<Azure.Batch.Models.Model>> OperationAsync(Azure.Batch.Models.Model body, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) { throw null; }
    }
    public partial class TemplateClientOptions : Azure.Core.ClientOptions
    {
        public TemplateClientOptions(Azure.Batch.TemplateClientOptions.ServiceVersion version = Azure.Batch.TemplateClientOptions.ServiceVersion.V1) { }
        public enum ServiceVersion
        {
            V1 = 1,
        }
    }
}
namespace Azure.Batch.Models
{
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly partial struct DaysOfWeek : System.IEquatable<Azure.Batch.Models.DaysOfWeek>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public DaysOfWeek(string value) { throw null; }
        public static Azure.Batch.Models.DaysOfWeek Friday { get { throw null; } }
        public static Azure.Batch.Models.DaysOfWeek Monday { get { throw null; } }
        public static Azure.Batch.Models.DaysOfWeek Saturday { get { throw null; } }
        public static Azure.Batch.Models.DaysOfWeek Sunday { get { throw null; } }
        public static Azure.Batch.Models.DaysOfWeek Thursday { get { throw null; } }
        public static Azure.Batch.Models.DaysOfWeek Tuesday { get { throw null; } }
        public static Azure.Batch.Models.DaysOfWeek Wednesday { get { throw null; } }
        public bool Equals(Azure.Batch.Models.DaysOfWeek other) { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool Equals(object obj) { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Azure.Batch.Models.DaysOfWeek left, Azure.Batch.Models.DaysOfWeek right) { throw null; }
        public static implicit operator Azure.Batch.Models.DaysOfWeek (string value) { throw null; }
        public static bool operator !=(Azure.Batch.Models.DaysOfWeek left, Azure.Batch.Models.DaysOfWeek right) { throw null; }
        public override string ToString() { throw null; }
    }
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly partial struct Fruit : System.IEquatable<Azure.Batch.Models.Fruit>
    {
        private readonly object _dummy;
        private readonly int _dummyPrimitive;
        public Fruit(string value) { throw null; }
        public static Azure.Batch.Models.Fruit Apple { get { throw null; } }
        public static Azure.Batch.Models.Fruit Pear { get { throw null; } }
        public bool Equals(Azure.Batch.Models.Fruit other) { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool Equals(object obj) { throw null; }
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public override int GetHashCode() { throw null; }
        public static bool operator ==(Azure.Batch.Models.Fruit left, Azure.Batch.Models.Fruit right) { throw null; }
        public static implicit operator Azure.Batch.Models.Fruit (string value) { throw null; }
        public static bool operator !=(Azure.Batch.Models.Fruit left, Azure.Batch.Models.Fruit right) { throw null; }
        public override string ToString() { throw null; }
    }
    public partial class Model
    {
        public Model(Azure.Batch.Models.Fruit fruit, Azure.Batch.Models.DaysOfWeek daysOfWeek) { }
        public Azure.Batch.Models.DaysOfWeek DaysOfWeek { get { throw null; } }
        public Azure.Batch.Models.Fruit Fruit { get { throw null; } }
        public string ModelProperty { get { throw null; } set { } }
    }
}
