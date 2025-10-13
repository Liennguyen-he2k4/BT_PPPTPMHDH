using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BTLT_tuan_8
{
    public class Form1 : Form
    {
        // ====== Khai báo Control ======
        TextBox txtMaSV, txtTenSV, txtQueQuan, txtMaLop;
        ComboBox cbGioiTinh;
        DateTimePicker dtpNgaySinh;
        Button btnThem, btnSua, btnXoa, btnHienThi;
        ListView lsvDanhSach;

        // ====== Kết nối CSDL ======
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
AttachDbFilename=|DataDirectory|\DBConnect.mdf;
Integrated Security=True";

        SqlConnection sqlCon = null;

        // ====== Hàm khởi tạo Form ======
        public Form1()
        {
            this.Text = "LAB 5 - Quản lý Sinh Viên";
            this.Size = new Size(850, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Gọi hàm tạo giao diện
            TaoGiaoDien();
        }

        // ====== 1️⃣ TẠO GIAO DIỆN ======
        private void TaoGiaoDien()
        {
            Label lblTitle = new Label()
            {
                Text = "QUẢN LÝ SINH VIÊN (LAB 5 - Winform & Database)",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(180, 10)
            };
            this.Controls.Add(lblTitle);

            int xLabel = 30, xInput = 150, y = 60, step = 35;

            // Mã SV
            this.Controls.Add(new Label() { Text = "Mã sinh viên:", Location = new Point(xLabel, y + 5), AutoSize = true });
            txtMaSV = new TextBox() { Location = new Point(xInput, y), Width = 200 }; y += step;

            // Tên SV
            this.Controls.Add(new Label() { Text = "Tên sinh viên:", Location = new Point(xLabel, y + 5), AutoSize = true });
            txtTenSV = new TextBox() { Location = new Point(xInput, y), Width = 200 }; y += step;

            // Giới tính
            this.Controls.Add(new Label() { Text = "Giới tính:", Location = new Point(xLabel, y + 5), AutoSize = true });
            cbGioiTinh = new ComboBox() { Location = new Point(xInput, y), Width = 200 };
            cbGioiTinh.Items.Add("Nam");
            cbGioiTinh.Items.Add("Nữ"); y += step;

            // Ngày sinh
            this.Controls.Add(new Label() { Text = "Ngày sinh:", Location = new Point(xLabel, y + 5), AutoSize = true });
            dtpNgaySinh = new DateTimePicker() { Location = new Point(xInput, y), Width = 200, Format = DateTimePickerFormat.Short }; y += step;

            // Quê quán
            this.Controls.Add(new Label() { Text = "Quê quán:", Location = new Point(xLabel, y + 5), AutoSize = true });
            txtQueQuan = new TextBox() { Location = new Point(xInput, y), Width = 200 }; y += step;

            // Mã lớp
            this.Controls.Add(new Label() { Text = "Mã lớp:", Location = new Point(xLabel, y + 5), AutoSize = true });
            txtMaLop = new TextBox() { Location = new Point(xInput, y), Width = 200 }; y += step;

            // Nút Thêm / Sửa / Xóa
            btnThem = new Button() { Text = "Thêm (Không Parameter)", Location = new Point(30, y + 20), Width = 180 };
            btnThem.Click += BtnThem_Click;

            btnSua = new Button() { Text = "Sửa (Có Parameter)", Location = new Point(230, y + 20), Width = 180 };
            btnSua.Click += BtnSua_Click;

            btnXoa = new Button() { Text = "Xóa (Có Parameter)", Location = new Point(430, y + 20), Width = 180 };
            btnXoa.Click += BtnXoa_Click;

            btnHienThi = new Button() { Text = "Hiển thị danh sách", Location = new Point(630, y + 20), Width = 180 };
            btnHienThi.Click += BtnHienThi_Click;

            this.Controls.AddRange(new Control[] { txtMaSV, txtTenSV, cbGioiTinh, dtpNgaySinh, txtQueQuan, txtMaLop, btnThem, btnSua, btnXoa, btnHienThi });

            // ListView
            lsvDanhSach = new ListView()
            {
                Location = new Point(30, y + 70),
                Size = new Size(770, 250),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            lsvDanhSach.Columns.Add("MaSV", 80);
            lsvDanhSach.Columns.Add("TenSV", 150);
            lsvDanhSach.Columns.Add("GioiTinh", 70);
            lsvDanhSach.Columns.Add("NgaySinh", 100);
            lsvDanhSach.Columns.Add("QueQuan", 150);
            lsvDanhSach.Columns.Add("MaLop", 80);

            this.Controls.Add(lsvDanhSach);
        }

        // ====== 2️⃣ MỞ / ĐÓNG KẾT NỐI ======
        private void MoKetNoi()
        {
            if (sqlCon == null)
                sqlCon = new SqlConnection(strCon);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
        }

        private void DongKetNoi()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                sqlCon.Close();
        }

        // ====== 3️⃣ HIỂN THỊ DANH SÁCH ======
        private void HienThiDanhSach()
        {
            try
            {
                MoKetNoi();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SinhVien", sqlCon);
                SqlDataReader reader = cmd.ExecuteReader();

                lsvDanhSach.Items.Clear();

                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["MaSV"].ToString());
                    item.SubItems.Add(reader["TenSV"].ToString());
                    item.SubItems.Add(reader["GioiTinh"].ToString());
                    item.SubItems.Add(Convert.ToDateTime(reader["NgaySinh"]).ToString("dd/MM/yyyy"));
                    item.SubItems.Add(reader["QueQuan"].ToString());
                    item.SubItems.Add(reader["MaLop"].ToString());
                    lsvDanhSach.Items.Add(item);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi hiển thị: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }

        // ====== 4️⃣ THÊM SINH VIÊN (Không Parameter) ======
        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                MoKetNoi();
                string sql = "INSERT INTO SinhVien VALUES ('" + txtMaSV.Text + "', N'" + txtTenSV.Text +
                    "', N'" + cbGioiTinh.Text + "', '" + dtpNgaySinh.Value.ToString("yyyy/MM/dd") +
                    "', N'" + txtQueQuan.Text + "', '" + txtMaLop.Text + "')";

                SqlCommand cmd = new SqlCommand(sql, sqlCon);
                int kq = cmd.ExecuteNonQuery();

                if (kq > 0)
                {
                    MessageBox.Show("✅ Thêm sinh viên thành công!");
                    HienThiDanhSach();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi thêm: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }

        // ====== 5️⃣ SỬA SINH VIÊN (Có Parameter) ======
        private void BtnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSV.Text))
                {
                    MessageBox.Show("⚠️ Vui lòng nhập Mã SV để sửa!", "Thông báo");
                    return;
                }

                MoKetNoi();

                // Câu lệnh UPDATE dùng parameter để an toàn
                string sql = @"UPDATE SinhVien 
                       SET TenSV = @TenSV,
                           GioiTinh = @GioiTinh,
                           NgaySinh = @NgaySinh,
                           QueQuan = @QueQuan,
                           MaLop = @MaLop
                       WHERE MaSV = @MaSV";

                using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
                {
                    cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenSV", txtTenSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.Text.Trim());
                    cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@QueQuan", txtQueQuan.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());

                    int kq = cmd.ExecuteNonQuery();

                    if (kq > 0)
                    {
                        MessageBox.Show("✅ Sửa thông tin sinh viên thành công!", "Thành công");
                        HienThiDanhSach();
                    }
                    else
                    {
                        MessageBox.Show("⚠️ Không tìm thấy sinh viên có mã này!", "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi sửa dữ liệu: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }


        // ====== 6️⃣ XÓA SINH VIÊN (Có Parameter) ======
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSV.Text == "")
                {
                    MessageBox.Show("⚠️ Nhập Mã SV để xóa!");
                    return;
                }

                MoKetNoi();
                SqlCommand cmd = new SqlCommand("DELETE FROM SinhVien WHERE MaSV=@MaSV", sqlCon);
                cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);

                int kq = cmd.ExecuteNonQuery();

                if (kq > 0)
                {
                    MessageBox.Show("🗑️ Xóa thành công!");
                    HienThiDanhSach();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi xóa: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }

        // ====== 7️⃣ NÚT HIỂN THỊ ======
        private void BtnHienThi_Click(object sender, EventArgs e)
        {
            HienThiDanhSach();
        }
    }
}
