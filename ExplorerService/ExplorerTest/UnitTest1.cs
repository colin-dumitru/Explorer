using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBLibrary;
using System.Data.SqlClient;

namespace ExplorerTest {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void main() {
            DBWriter.LogFile = @"C:\Users\bkt\Desktop\explorer\ExplorerService\dblog.txt";
            DBWriter.Initialize();
            DBWriter.ListDrive("C:");
        }
    }
}
