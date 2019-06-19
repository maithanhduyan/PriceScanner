using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Syn2DB
{
    public partial class KHACHHANG : Form
    {
        private FbConnection loadConnection;
        private FbConnection migrateConnection;
        private string defaultRow = " FIRST 100 ";
        private List<string> fiedTypes = null;
        private static readonly log4net.ILog log
        = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Table Structure Query
        private string tablePropertiesQuery =
            "SELECT r.RDB$FIELD_NAME AS field_name,"
            + "CASE f.RDB$FIELD_TYPE"
            + " WHEN 261 THEN 'BLOB'"
            + " WHEN 14 THEN 'CHAR'"
            + " WHEN 40 THEN 'CSTRING'"
            + " WHEN 11 THEN 'D_FLOAT'"
            + " WHEN 27 THEN 'DOUBLE'"
            + " WHEN 10 THEN 'FLOAT'"
            + " WHEN 16 THEN 'INT64'"
            + " WHEN 8 THEN 'INTEGER'"
            + " WHEN 9 THEN 'QUAD' "
            + " WHEN 7 THEN 'SMALLINT'"
            + " WHEN 12 THEN 'DATE'"
            + " WHEN 13 THEN 'TIME'"
            + " WHEN 35 THEN 'TIMESTAMP'"
            + " WHEN 37 THEN 'VARCHAR'"
            + " ELSE 'UNKNOWN'"
            + " END AS field_type"
            + " FROM RDB$RELATION_FIELDS r"
            + " LEFT JOIN RDB$FIELDS f ON r.RDB$FIELD_SOURCE = f.RDB$FIELD_NAME"
            + " LEFT JOIN RDB$COLLATIONS coll ON f.RDB$COLLATION_ID = coll.RDB$COLLATION_ID"
            + " LEFT JOIN RDB$CHARACTER_SETS cset ON f.RDB$CHARACTER_SET_ID = cset.RDB$CHARACTER_SET_ID"
            + " WHERE r.RDB$RELATION_NAME= ";
        #endregion

        #region List Table in Database Query
        private readonly string tableListQuery =
              " SELECT DISTINCT RDB$RELATION_NAME "
            + " FROM RDB$RELATION_FIELDS "
            + " WHERE RDB$SYSTEM_FLAG=0; ";

        #endregion
        public KHACHHANG()
        {
            InitializeComponent();

            ///<summary>Initial DataGridView</summary>
            initializeDataGridView();

            this.fiedTypes = new List<string>();

            // Reading data connection
            loadConnection = new FbConnection("User=SYSDBA;" + "Password=masterkey;" + "Database=C:\\phanmembanhang\\DATA1.FDB;" + "DataSource=45.119.81.21;" + "Port=3050;" + "Dialect=3;" + "Charset=UTF8;" + "Connection Timeout=30");
            loadConnection.Open();

            // Insert data connection
            migrateConnection = new FbConnection("User=SYSDBA;" + "Password=masterkey;" + "Database=C:\\phanmembanhang\\DATA.FDB;" + "DataSource=45.119.81.21;" + "Port=3050;" + "Dialect=3;" + "Charset=UTF8;");
            migrateConnection.Open();
        }

        private void initializeDataGridView()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void KHACHHANG_Load(object sender, EventArgs e)
        {
            LoadListTable();

            comboBoxRows.SelectedIndex = 0;
        }

        /// <summary>
        /// Load data on DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadKhachHang();

            LoadDataGridViewColumnHeader();

            LoadStructureTable();
        }

        /// <summary>
        /// Load data for comboBoxColumn
        /// </summary>
        private void LoadDataGridViewColumnHeader()
        {
            
            comboBoxColumn.Items.Clear();
            if (dataGridView1.RowCount > 0)
            {
                comboBoxColumn.Items.Add("");
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    comboBoxColumn.Items.Add(dataGridView1.Columns[i].HeaderText);
                }
            }
        }
        private void LoadKhachHang()
        {

            string tableName = "TEST1";
            if (comboBoxTable.Text.Length > 0 || comboBoxTable.Text != "")
            {
                tableName = comboBoxTable.Text.ToString();
            }
            else
            {
                MessageBox.Show("Choose a Table");
                return;
            }
            if (comboBoxRows.Text.Trim().Equals("All"))
            {
                defaultRow = "";
            }
            else
            {
                defaultRow = comboBoxRows.Text.Trim();
            }
            try
            {
                //SELECT FIRST 10 SKIP 20 * FROM TDONHANG 
                String sqlCommand = "SELECT " + defaultRow + " * FROM " + tableName + QueryCondition();
                FbCommand commandRead = new FbCommand(sqlCommand, loadConnection);
                commandRead.CommandType = CommandType.Text;

                using (FbDataAdapter sda = new FbDataAdapter(commandRead))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not load data.");
                Console.WriteLine("Can not load data.");
                Console.WriteLine("Ex: " + ex.Message);
            }
            finally
            {
                ///<summary>
                /// Format DateTime for DataGridView
                /// </summary>
                dataGridView1.Columns["TIMECREATED"].DefaultCellStyle.Format = StringUtil.DateTimeFormat;
                dataGridView1.Columns["TIMEMODIFIED"].DefaultCellStyle.Format = StringUtil.DateTimeFormat;
                toolStripStatusLabel1.Text = "Count: " + dataGridView1.RowCount;
            }
        }

        private void LoadListTable()
        {
            try
            {
                String sqlCommand = tableListQuery;
                FbCommand commandRead = new FbCommand(sqlCommand, loadConnection);
                commandRead.CommandType = CommandType.Text;

                FbDataReader dtReader = commandRead.ExecuteReader();
                while (dtReader.Read())
                {
                    if (dtReader.HasRows)
                    {
                        //Console.WriteLine(dtReader[0]);
                        comboBoxTable.Items.Add(dtReader[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Load Table List Fail. ", ex);
                MessageBox.Show("Can not load data.");
            }
        }

        private void LoadStructureTable()
        {
            try
            {
                String sqlCommand = tablePropertiesQuery + "'" + comboBoxTable.Text.Trim() + "'";

                Console.WriteLine(sqlCommand);

                FbCommand commandRead = new FbCommand(sqlCommand, loadConnection);
                commandRead.CommandType = CommandType.Text;

                FbDataReader dtReader = commandRead.ExecuteReader();
                fiedTypes.Clear();

                int count = 0;
                while (dtReader.Read())
                {

                    if (dtReader.HasRows)
                    {
                        //Console.Write(dtReader[0].ToString().Trim() + "-");
                        //Console.WriteLine(dtReader[1].ToString().Trim());
                        fiedTypes.Add(dtReader[1].ToString().Trim());

                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Load Table List Fail. ", ex);
                MessageBox.Show("Can not load data.");
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            toolStripStatusLabel2.Text = "Index: " + e.RowIndex;
        }

        private void BtnMigrate_Click(object sender, EventArgs e)
        {
            MigrateData();
        }

        private void MigrateData()
        {
            string tableName = "TEST1";
            int effectCount = 0;
            int failCount = 0;
            if (comboBoxTable.Text.Length > 0 || comboBoxTable.Text != "")
            {
                tableName = comboBoxTable.Text.ToString().Trim();
            }
            else
            {
                MessageBox.Show("Choose a Table");
                return;
            }

            string addColumn = "";
            string addValue = "";
            string query = "";
            if (dataGridView1.RowCount > 0)
            {

                query = "INSERT INTO ";
                query += " " + tableName + " ( ";
                //
                string lastComma = ", ";
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {

                    if (i == (dataGridView1.ColumnCount - 1))
                    {
                        lastComma = "";
                    }
                    addColumn += "" + dataGridView1.Columns[i].HeaderText + lastComma;
                }

                addColumn += " ) ";
                addColumn += " VALUES (";
                query += addColumn;

                //
                FbCommand fbCommand = null;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    DataGridViewRow row = dataGridView1.Rows[i];
                    // ADD VALUES
                    addValue = query;
                    lastComma = ",";
                    for (int colIndex = 0; colIndex < dataGridView1.ColumnCount; colIndex++)
                    {
                        if (colIndex == (dataGridView1.ColumnCount - 1)) { lastComma = ""; }
                        
                        addValue += "" + ConvertIfNull(row.Cells[colIndex].FormattedValue.ToString(), fiedTypes[colIndex]) + lastComma;

                    }


                    addValue += " );"; // End of query

                    Console.WriteLine(addValue);

                    try
                    {
                        if(checkBoxConnectDB.Checked)
                        {

                            fbCommand = new FbCommand(addValue , migrateConnection);
                            fbCommand.CommandType = CommandType.Text;
                            int affectedRows = fbCommand.ExecuteNonQuery();
                            if(affectedRows > 0)
                            {
                                Console.WriteLine(affectedRows + "  rows inserted!");
                            }

                            effectCount++;
                        }
                        

                    }
                    catch (Exception ex)
                    {
                        // Error log
                        log.Error("Error on " + tableName + ": ", ex);
                        Console.WriteLine("Error on " + tableName + ": " + ex.Message);
                        failCount++;
                        continue;
                    }
                    finally
                    {
                        toolStripStatusLabel3.Text = "Success: " + effectCount + " Fail: " + failCount;
                    }

                }

            }

        }

        private void ComboBoxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine(comboBoxTable.Text);
        }

        private string ConvertIfNull(string input, string dataType)
        {
            string result = "''";

            switch (dataType)
            {
                case "VARCHAR":
                    result = "'" + input + "'";
                    break;
                case "INTEGER":
                    if (input.Equals("") || input == null)
                    {
                        result = "NULL";
                    }
                    else
                    {
                        result = "" + input + "";
                    }

                    break;
                case "DECIMAL":
                    if (input.Equals("") || input == null)
                    {
                        result = "NULL";
                    }
                    else
                    {
                        result = "" + input + "";
                    }
                    break;
                case "TIMESTAMP":
                    if (input.Equals("") || input == null)
                    {
                        result = "NULL";
                    }
                    else
                    {
                        // Convert.ToDateTime(input,null).ToString(StringUtil.DateTimeFormat) 
                        result = "'" + input + "'";
                    }
                    break;
                case "INT64":
                    if (input.Equals("") || input == null)
                    {
                        result = "NULL";
                    }
                    else
                    {
                        result = "'" + input + "'";
                    }
                    break;
                case "BLOB":
                    if (input.Equals("") || input == null)
                    {
                        result = "''";
                    }
                    else if (input.Equals("System.Drawing.Bitmap"))
                    {
                        result = "''";
                    }
                    else
                    {
                        result = "'no image'";
                    }
                    break;
                default: result = "NULL"; break;
            }

            return result;
        }

        private string QueryCondition()
        {
            string result = "";
            if(!comboBoxColumn.Text.Equals("") && !comboBoxCondition.Text.Equals("") && !textBoxWhere.Text.Equals(""))
            {
                if(comboBoxCondition.Text.Equals("LIKE"))
                {
                    result = " WHERE " + comboBoxColumn.Text.Trim() + " " + comboBoxCondition.Text.Trim() + " %" + textBoxWhere.Text +"%";
                }
                else
                {
                    result = " WHERE " + comboBoxColumn.Text.Trim() + " " + comboBoxCondition.Text.Trim() + " " + textBoxWhere.Text;
                }
                
            }
            Console.WriteLine(result);

            return result;
        }
        private void CheckBoxConnectDB_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void KHACHHANG_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Closing 2 connection
            loadConnection.Close();
            migrateConnection.Close();
        }


    }
}
public class Table
{
    private string name;
    private string[] fieldNames;
    private string[] fieldTypes;
    public Table()
    {

    }
    public Table(string name, string[] fieldNames, string[] fieldTypes)
    {
        this.name = name;
        this.fieldNames = fieldNames;
        this.fieldTypes = fieldTypes;
    }
    public string Name { get { return this.name; } set { this.name = value; } }

    public string[] FieldNames { get { return this.fieldNames; } set { this.fieldNames = value; } }

    public string[] FieldTypes { get { return this.fieldTypes; } set { this.fieldTypes = value; } }

}