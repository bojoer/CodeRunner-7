using System.IO;
using System.Xml;

using MasterServer.Constants;

namespace MasterServer.Utilities
{
    public class FileUtilities
    {
        public static void WriteFailedTestCaseToXmlFile(TestCaseOutput failedTestCase, string errorFilePath)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(errorFilePath))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("FailedTestCase");

                xmlWriter.WriteElementString("Input", failedTestCase.Input);
                xmlWriter.WriteElementString("ExpectedOutput", failedTestCase.ExpectedOutput);
                xmlWriter.WriteElementString("ReceivedOutput", failedTestCase.ReceivedOutput);
                xmlWriter.WriteElementString("ExceptionThrown", failedTestCase.ExceptionThrown);

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }

        public static string GetErrorFilePath(string codeFilePath)
        {
            string parentDir = Path.GetDirectoryName(codeFilePath);
            string codeFileName = Path.GetFileName(codeFilePath);

            return parentDir + codeFileName + "_Error.xml";
        }

        public static string GetTesterFileName(string fileName)
        {
            return fileName + "Tester.cs";
        }

        public static string GetTesterFilePath(string testerFileName)
        {
            return FileConstants.TesterFilesPath + "\\" + testerFileName;
        }
    }
}
