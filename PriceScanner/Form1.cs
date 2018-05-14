using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace PriceScanner
{
    public partial class Form1 : Form
    {
        private FbConnection con;

        public Form1()
        {
            InitializeComponent();
            con = new FbConnection("User=SYSDBA;" + "Password=masterkey;" + "Database=D:\\phanmemkhongxoa\\DULIEU\\DATA.fdb;" + "DataSource=sieuthibeemart.com;" + "Port=3050;" + "Dialect=3;" + "Charset=UTF8;");
            con.Open();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            getMatHangByCode("8934673613712");
        }

        private void textBoxMaVach_TextChanged(object sender, EventArgs e)
        {
            getMatHangByCode(textBoxMaVach.Text.ToString());
        }

        #region Tìm Mặt Hàng theo Mã Vạch

        private void getMatHangByCode(String code)
        {
            int start = DateTimeOffset.Now.Millisecond;
            try
            {
                String sqlCommand = "SELECT MH.NAME, MH.GIABAN FROM DMATHANG MH WHERE MH.CODE = @code";
                FbCommand commandRead = new FbCommand(sqlCommand, con);
                commandRead.CommandType = CommandType.Text;
                commandRead.Parameters.Add("@code", code);
                FbDataReader dtReader = commandRead.ExecuteReader();
                while (dtReader.Read())
                {
                    if (dtReader.HasRows)
                    {
                        labelTenMatHang.Text = "" + dtReader[0].ToString();
                        labelGia.Text = "Giá: " + exchangeCurrency(dtReader[1].ToString()) + " đồng";
                        toolStripStatusLabel2.Text = "| Giá: " + convertCurrencuToWords(Int32.Parse(dtReader[1].ToString()));
                        textBoxMaVach.SelectAll();
                    }  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            int end = DateTimeOffset.Now.Millisecond;
            toolStripStatusLabel1.Text = "Thời gian tìm: " + (end - start) + " ms.";
        }

        #endregion Tìm Mặt Hàng theo Mã Vạch

        #region Chuyển đổi tiền tệ có dấu phẩy ngăn cách

        private String exchangeCurrency(String currency)
        {
            return string.Format("{0:#,#}", Convert.ToDecimal(currency));
        }

        #endregion Chuyển đổi tiền tệ có dấu phẩy ngăn cách

        #region Chuyển đổi tiền bằng chữ

        public String convertCurrencuToWords(decimal total)  //đọc đc 18 số vd: 999999999999999999
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();
                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }
                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }
                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng.";
                else
                    rs += "đồng.";
                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
            }
            catch
            {
                return "Số bạn nhập vào quá lớn";
            }
        }

        #endregion Chuyển đổi tiền bằng chữ

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
        }
    }
}