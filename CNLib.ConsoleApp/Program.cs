

using CNLib.CNDbManager.CNMariaDB;
using System.Data;

MariaDbHelper db = new MariaDbHelper();


DataTable table = db.SelectTable("test") as DataTable;





int  a = 0; 