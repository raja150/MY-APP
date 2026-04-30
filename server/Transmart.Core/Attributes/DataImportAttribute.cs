namespace TranSmart.Core.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Property)]
	public class DataImportAttribute : System.Attribute
    {
        public DataImportAttribute()
        {

        }
        public string Name { set; get; }
        public int Order { set; get; }
        public bool Required { set; get; } = true;
        public bool ForError { set; get; }

        public string GetName() => Name;
        public int GetOrder() =>   Order; 
        public bool GetRequired() =>  Required;
        public bool GetForError() =>  ForError;
    }
}
