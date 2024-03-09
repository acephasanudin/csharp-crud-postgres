using Npgsql;
using System.Data;

namespace ConnectPostgres
{
    public partial class Form1 : Form
    {
        private string connstring = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4};",
            "localhost", 5432, "postgres",
            "postgres", "Demo");
        private NpgsqlConnection conn;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;
        private int rowIndex = -1;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            Select();

        }

        private void Select()
        {
            try
            {
                conn.Open();
                sql = @"select * from st_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                dgvData.DataSource = null;
                dgvData.DataSource = dt;
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                rowIndex = e.RowIndex;
                txtFirstName.Text = dgvData.Rows[e.RowIndex].Cells["_firstName"].Value.ToString();
                txtMidName.Text = dgvData.Rows[e.RowIndex].Cells["_midName"].Value.ToString();
                txtLastName.Text = dgvData.Rows[e.RowIndex].Cells["_lastName"].Value.ToString();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            rowIndex = -1;
            txtFirstName.Text = txtMidName.Text = txtLastName.Text = null;
            txtFirstName.Select();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (rowIndex < 0)
            {
                MessageBox.Show("Please choose student to update");
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (rowIndex < 0)
            {
                MessageBox.Show("Please choose student to delete");
            }
            try
            {
                conn.Open();
                sql = @"select * from st_delete(:_id)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id", int.Parse(dgvData.Rows[rowIndex].Cells["id"].Value.ToString()));
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Delete Student Successfully");
                    rowIndex = -1;
                    conn.Close();
                    Select();
                }
            }
            catch (Exception ex) {
                conn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (rowIndex < 0)
            {
                try
                {
                    conn.Open();
                    sql = @"select * from st_insert(:_firstName, :_midName, :_lastName)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_firstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("_midName", txtMidName.Text);
                    cmd.Parameters.AddWithValue("_lastName", txtLastName.Text);
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("Insert new student successfully");
                        Select();
                    }
                    else {
                        MessageBox.Show("Insert student failed");
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show(ex.Message);
                }
            } else
            {
                try
                {
                    conn.Open();
                    sql = @"select * from st_update(:_id, :_firstName, :_midName, :_lastName)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id", int.Parse(dgvData.Rows[rowIndex].Cells["id"].Value.ToString()));
                    cmd.Parameters.AddWithValue("_firstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("_midName", txtMidName.Text);
                    cmd.Parameters.AddWithValue("_lastName", txtLastName.Text);
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("Update student successfully");
                        Select();
                    }
                    else {
                        MessageBox.Show("Update student failed");
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show(ex.Message);
                }
                result = 0;
            }
        }
    }
}
