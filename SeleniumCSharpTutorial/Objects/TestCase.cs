namespace SeleniumCSharpTutorial.Objects
{
    // I've built this object to organize test case values supplied by Excel file (becu.xls)
    public class TestCase
    {
        public int CaseId { get; set; }// This ID is for identifying which test case we're interacting with
        public string[]? CaseValues { get; set; }// This is an array of the actual test case values that will be supplied to the form

        public override int GetHashCode()
        {
            return CaseId;
        }
    }
}
