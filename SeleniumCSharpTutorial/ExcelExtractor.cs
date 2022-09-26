using ExcelDataReader;
using SeleniumCSharpTutorial.Objects;
using System.Data;

namespace SeleniumCSharpTutorial
{
    public class ExcelExtractor
    {
        // I decided to put all Excel Reader related code here to keep the project organized.
        public static List<TestCase> GetTestDataFromExcel()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open("../../../becu.xls"/* Change this path if you want to open another Excel file */, FileMode.Open, FileAccess.Read))
            {
                using IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                });
                var dataTable = dataSet.Tables[2];

                List<TestCase> testCases = new List<TestCase>();

                foreach (DataRow row in dataTable.Rows)
                {
                    // TODO: Validate row values in case they are null
                    string[] caseVals = { row[1] + "", row[2] + "", row[3] + "", row[4] + "" };
                    testCases.Add(new TestCase() { CaseId = int.Parse(row[0] + ""), CaseValues = caseVals });
                }
                return testCases;
            }
        }
    }
}
