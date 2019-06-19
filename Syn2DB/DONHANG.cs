using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Syn2DB
{


    public partial class DONHANG : Form
    {
        private FbConnection loadConnection;
        private FbConnection insertConnection;
        public DONHANG()
        {
            InitializeComponent();
            // Reading data connection
            loadConnection = new FbConnection("User=SYSDBA;" + "Password=masterkey;" + "Database=C:\\phanmembanhang\\DATA1.FDB;" + "DataSource=45.119.81.21;" + "Port=3050;" + "Dialect=3;" + "Charset=UTF8;" + "Connection Timeout=30");
            loadConnection.Open();

            // Insert data connection
            insertConnection = new FbConnection("User=SYSDBA;" + "Password=masterkey;" + "Database=C:\\phanmembanhang\\DATA.FDB;" + "DataSource=45.119.81.21;" + "Port=3050;" + "Dialect=3;" + "Charset=UTF8;");
            insertConnection.Open();
        }

        private void DONHANG_Load(object sender, EventArgs e)
        {

        }


        private void LoadDonHang()
        {
            try
            {
                //SELECT FIRST 10 SKIP 20 * FROM TDONHANG 
                String sqlCommand = "SELECT * FROM TDONHANG WHERE TIMECREATED > '2018-09-15 19:10:24' ";
                FbCommand commandRead = new FbCommand(sqlCommand, loadConnection);
                commandRead.CommandType = CommandType.Text;

                using (FbDataAdapter sda = new FbDataAdapter(commandRead))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        dataGridViewDonHang.DataSource = dt;
                    }
                }

                this.dataGridViewDonHang.Columns["TIMEMODIFIED"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                this.dataGridViewDonHang.Columns["TIMECREATED"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                this.dataGridViewDonHang.Columns["NGAY"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                this.dataGridViewDonHang.Columns["GIOTHANHTOAN"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                //FbDataReader dtReader = commandRead.ExecuteReader();
                //while (dtReader.Read())
                //{
                //if (dtReader.HasRows)
                //{
                //Console.WriteLine(dtReader[0].ToString());
                //}
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not load data.");
                Console.WriteLine("Can not load data.");
                Console.WriteLine("Ex: " + ex.Message);
            }
        }

        private void BtnLoadDonHang_Click(object sender, EventArgs e)
        {
            LoadDonHang();
        }

        private void DONHANG_FormClosing(object sender, FormClosingEventArgs e)
        {
            loadConnection.Close();
            insertConnection.Close();
        }

        private void BtnInsertDonHang_Click(object sender, EventArgs e)
        {
            insertDonHang();
        }

        private void insertDonHang()
        {
            string sqlCommand = "";
            Console.WriteLine("Total: " + dataGridViewDonHang.RowCount + " rows.");
            for (int i = 0; i < dataGridViewDonHang.RowCount; i++)
            {
                DataGridViewRow row = dataGridViewDonHang.Rows[i];
                sqlCommand = "INSERT INTO TDONHANG (ID,NAME,NOTE,STATUS,USERMODIFIEDID,TIMEMODIFIED,TIMECREATED,NGAY,USERCREATEDID,GIAMTHEOTIEN,LOAI,TONGCONG,PHIVANCHUYEN,TIENGIAMGIA,TILEGIAMGIA,TIENTHUE,TILETHUE,TIENHANG,DNHACUNGCAPID,DKHOXUATID,DKHONHAPID,DNHANVIENXUATID,DNHANVIENNHAPID,DIENGIAI,GIOTHANHTOAN,KHACHDUA,TRALAI,NOCU,LANIN,LOAIGIA,USERTHANHTOANID,TIENTHANHTOAN,LOAITHANHTOAN,TDATHANGID,GIAOHANG,DATHANHTOAN,DOITRA,CONNO,DIEM,VOUCHER,DNHANVIENGIAOID,TRICHNHANVIEN,DCUAHANGID,CONLAI,THANHTOAN,DKHACHHANGID,DTAIKHOANNGANHANGID,THETRATRUOC,TRUTICHLUY,DIEMGIAM,TIENMAT,CHUYENKHOAN,THE,DVOUCHERID,DTHETRATRUOCID) VALUES (";
                // ID
                sqlCommand += " '" + row.Cells[0].Value + "', ";
                // NAME
                sqlCommand += " '" + row.Cells[1].Value + "', ";
                // NOTE
                sqlCommand += " '" + row.Cells[2].Value + "', ";
                // STATUS
                sqlCommand += " " + convertString(row.Cells[3].Value) + ", ";
                // USERMODIFIEDID
                sqlCommand += " '" + row.Cells[4].Value + "', ";
                // TIMEMODIFIED
                sqlCommand += " '" + row.Cells[5].FormattedValue + "', ";
                // TIMECREATED
                sqlCommand += " '" + row.Cells[6].FormattedValue + "', ";
                // NGAY
                sqlCommand += " '" + row.Cells[7].FormattedValue + "', ";
                // USERCREATEDID
                sqlCommand += " '" + row.Cells[8].Value + "', ";
                // GIAMTHEOTIEN - INTEGER
                sqlCommand += " " + convertString(row.Cells[9].Value) + ", ";
                // LOAI - INTEGER
                sqlCommand += " " + convertString(row.Cells[10].Value) + ", ";
                // TONGCONG - DECIMAL
                sqlCommand += " " + convertString(row.Cells[11].Value) + ", ";
                // PHIVANCHUYEN - DECIMAL
                sqlCommand += " " + convertString(row.Cells[12].Value) + ", ";
                // TIENGIAMGIA - DECIMAL
                sqlCommand += " " + convertString(row.Cells[13].Value) + ", ";
                // TILEGIAMGIA - DECIMAL
                sqlCommand += " " + convertString(row.Cells[14].Value) + ", ";
                // TIENTHUE - DECIMAL
                sqlCommand += " " + convertString(row.Cells[15].Value) + ", ";
                // TILETHUE - DECIMAL
                sqlCommand += " " + convertString(row.Cells[16].Value) + ", ";
                // TIENHANG - DECIMAL
                sqlCommand += " " + convertString(row.Cells[17].Value) + ", ";
                // DNHACUNGCAPID
                sqlCommand += " '" + row.Cells[18].Value + "', ";
                // DKHOXUATID
                sqlCommand += " '" + row.Cells[19].Value + "', ";
                // DKHONHAPID
                sqlCommand += " '" + row.Cells[20].Value + "', ";
                // DNHANVIENXUATID
                sqlCommand += " '" + row.Cells[21].Value + "', ";
                // DNHANVIENNHAPID
                sqlCommand += " '" + row.Cells[22].Value + "', ";
                // DIENGIAI
                sqlCommand += " '" + row.Cells[23].FormattedValue + "', ";
                // GIOTHANHTOAN
                sqlCommand += " '" + row.Cells[24].FormattedValue + "', ";
                // KHACHDUA - DECIMAL
                sqlCommand += " " + convertString(row.Cells[25].Value) + ", ";
                // TRALAI - DECIMAL
                sqlCommand += " " + convertString(row.Cells[26].Value) + ", ";
                // NOCU - DECIMAL
                sqlCommand += " " + convertString(row.Cells[27].Value) + ", ";
                // LANIN - INTEGER
                sqlCommand += " " + convertString(row.Cells[28].Value) + ", ";
                // LOAIGIA - INTEGER
                sqlCommand += " " + convertString(row.Cells[29].Value) + ", ";
                // USERTHANHTOANID
                sqlCommand += " '" + row.Cells[30].Value + "', ";
                // TIENTHANHTOAN  - DECIMAL
                sqlCommand += " " + convertString(row.Cells[31].Value) + ", ";
                // LOAITHANHTOAN - INTEGER
                sqlCommand += " " + convertString(row.Cells[32].Value) + ", ";
                // TDATHANGID - INTEGER
                sqlCommand += " " + convertString(row.Cells[33].Value) + ", ";
                // GIAOHANG
                sqlCommand += " '" + row.Cells[34].Value + "', ";
                // DATHANHTOAN - INTEGER
                sqlCommand += " " + convertString(row.Cells[35].Value) + ", ";
                // DOITRA - DECIMAL
                sqlCommand += " " + convertString(row.Cells[36].Value) + ", ";
                // CONNO - INTEGER
                sqlCommand += " " + convertString(row.Cells[37].Value) + ", ";
                // DIEM - DECIMAL
                sqlCommand += " " + convertString(row.Cells[38].Value) + ", ";
                // VOUCHER - DECIMAL
                sqlCommand += " " + convertString(row.Cells[39].Value) + ", ";
                // DNHANVIENGIAOID
                sqlCommand += " '" + row.Cells[40].Value + "', ";
                // TRICHNHANVIEN - DECIMAL
                sqlCommand += " " + convertString(row.Cells[41].Value) + ", ";
                // DCUAHANGID
                sqlCommand += " '" + row.Cells[42].Value + "', ";
                // CONLAI - DECIMAL
                sqlCommand += " " + convertString(row.Cells[43].Value) + ", ";
                // THANHTOAN - DECIMAL
                sqlCommand += " " + convertString(row.Cells[44].Value) + ", ";
                // DKHACHHANGID
                sqlCommand += " '" + row.Cells[45].Value + "', ";
                // DTAIKHOANNGANHANGID
                sqlCommand += " '" + row.Cells[46].Value + "', ";
                // THETRATRUOC - DECIMAL
                sqlCommand += " " + convertString(row.Cells[47].Value) + ", ";
                // TRUTICHLUY - DECIMAL
                sqlCommand += " " + convertString(row.Cells[48].Value) + ", ";
                // DIEMGIAM - DECIMAL
                sqlCommand += " " + convertString(row.Cells[49].Value) + ", ";
                // TIENMAT - DECIMAL
                sqlCommand += " " + convertString(row.Cells[50].Value) + ", ";
                // CHUYENKHOAN - DECIMAL
                sqlCommand += " " + convertString(row.Cells[51].Value) + ", ";
                // THE - DECIMAL
                sqlCommand += " " + convertString(row.Cells[52].Value) + ", ";
                // DVOUCHERID
                sqlCommand += " '" + row.Cells[53].Value + "', ";
                // DTHETRATRUOCID
                sqlCommand += " '" + row.Cells[54].Value + "' ";

                sqlCommand += " )";
                Console.WriteLine(sqlCommand);
                //break;
                try
                {
                    FbCommand fbCommand = new FbCommand(sqlCommand, insertConnection);
                    int affectedRows = fbCommand.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        Console.WriteLine(affectedRows + "  rows inserted!");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    continue;
                }
                finally
                {

                }

                // break for testing
                //break;

            }
        }


        private String convertString(Object input)
        {
            if (input.ToString().Equals(""))
            {
                return "NULL";
            }
            return input.ToString();
        }
    }
}
