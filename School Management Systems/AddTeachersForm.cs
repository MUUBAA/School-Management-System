using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_Systems
{
    public partial class AddTeachersForm : UserControl
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mubaseer\Documents\SchoolDB.mdf;Integrated Security=True;Connect Timeout=30");
        public AddTeachersForm()
        {
            InitializeComponent();

            teacherDisplayData();
        }
        public void teacherDisplayData()
        {

            AddTeachersData addTD = new AddTeachersData();

            teacher_gridData.DataSource = addTD.teacherData();
        }
        private void teacher_add_Click(object sender, EventArgs e)
        {

            if (teacher_id.Text == ""
                 || teacher_name.Text == ""
                 || teacher_gender.Text == ""
                 || teacher_address.Text == ""
                 || teacher_status.Text == ""
                 || teacher_image.Image == null
                 || imagePath == null)
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        String checkTeacherID = "SELECT COUNT(*) FROM teachers WHERE teacher_id = @teacherID";

                        using (SqlCommand checkTID = new SqlCommand(checkTeacherID, connect))
                        {

                            checkTID.Parameters.AddWithValue("teacherID", teacher_id.Text.Trim());
                            int count = (int)checkTID.ExecuteScalar();

                            if (count >= 1)
                            {
                                MessageBox.Show("Teacher ID :" + teacher_id.Text.Trim(), "is already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime today = DateTime.Today;
                                String insertData = "INSERT INTO teachers " +
                                        "(teacher_id, teacher_name, teacher_gender, teacher_address, " +
                                        "teacher_image, teacher_status, date_insert)" +
                                        "VALUES(@teacherID, @teacherName, @teacherGender, @teacherAddress, " +
                                        "@teacherImage, @teacherStatus, @dateInsert) ";

                                string path = Path.Combine(@"C:\Users\Mubaseer\source\repos\School Management Systems\School Management Systems\Teacher_Directory\", teacher_id.Text.Trim() + ".jpg");

                                string directoryPath = Path.GetDirectoryName(path);

                                if (!Directory.Exists(directoryPath))
                                {

                                    Directory.CreateDirectory(directoryPath);
                                }
                                File.Copy(imagePath, path, true);

                                using (SqlCommand cmd = new SqlCommand(insertData, connect))
                                {
                                    cmd.Parameters.AddWithValue("@teacherID", teacher_id.Text.Trim());
                                    cmd.Parameters.AddWithValue("@teacherName", teacher_name.Text.Trim());
                                    cmd.Parameters.AddWithValue("@teacherGender", teacher_gender.Text.Trim());
                                    cmd.Parameters.AddWithValue("@teacherAddress", teacher_address.Text.Trim());
                                    cmd.Parameters.AddWithValue("@teacherImage", path);
                                    cmd.Parameters.AddWithValue("@teacherStatus", teacher_status.Text.Trim());
                                    cmd.Parameters.AddWithValue("@dateInsert", today.ToString());

                                    cmd.ExecuteNonQuery();

                                    teacherDisplayData();

                                    MessageBox.Show("Added successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    clearFields();


                                }

                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting Database: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    finally
                    {
                        connect.Close();
                    }
                }

            }
        }

        public void clearFields()
        {
            teacher_id.Text = "";
            teacher_name.Text = "";
            teacher_gender.SelectedIndex = -1;
            teacher_address.Text = "";
            teacher_status.Text = "";
            teacher_image.Image = null;
            imagePath = "";

        }

        private string imagePath;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg;,*.png;)|*.jpg;*.png;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                imagePath = open.FileName;
                teacher_image.ImageLocation = imagePath;
            }
        }

        private void teacher_update_Click(object sender, EventArgs e)
        {
            if (teacher_id.Text == ""
              || teacher_name.Text == ""
              || teacher_gender.Text == ""
              || teacher_address.Text == ""
              || teacher_status.Text == ""
              || teacher_image.Image == null
              || imagePath == null)
            {
                MessageBox.Show("Please select item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        DialogResult check = MessageBox.Show("Are you sure want to update Teacher ID: " + teacher_id.Text.Trim() + "?", "Confirmation Message",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (check == DialogResult.Yes)
                        {
                            DateTime today = DateTime.Today;

                            String UpdateData = "UPDATE teachers SET " +
                                "teacher_name = @teacherName, teacher_gender = @teacherGender " +
                                ", teacher_address = @teacherAddress " +
                                ",teacher_status = @teacherStatus " +
                                ",date_update = @dateUpdate WHERE teacher_id = @teacherID";

                            using (SqlCommand cmd = new SqlCommand(UpdateData, connect))
                            {
                                cmd.Parameters.AddWithValue("teacherName", teacher_name.Text.Trim());
                                cmd.Parameters.AddWithValue("teacherGender", teacher_gender.Text.Trim());
                                cmd.Parameters.AddWithValue("teacherAddress", teacher_address.Text.Trim());
                                cmd.Parameters.AddWithValue("teacherStatus", teacher_status.Text.Trim());
                                cmd.Parameters.AddWithValue("dateUpdate", today.ToString());
                                cmd.Parameters.AddWithValue("teacherID", teacher_id.Text.Trim());

                                cmd.ExecuteNonQuery();

                                teacherDisplayData();

                                MessageBox.Show("Updated successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                clearFields();



                            }

                        }
                        else
                        {

                            MessageBox.Show("Cancelled.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            clearFields();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting Database: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        private void teacher_gridData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = teacher_gridData.Rows[e.RowIndex];
                teacher_id.Text = row.Cells[1].Value.ToString();
                teacher_name.Text = row.Cells[2].Value.ToString();
                teacher_gender.Text = row.Cells[3].Value.ToString();
                teacher_address.Text = row.Cells[4].Value.ToString();

                imagePath = row.Cells[5].Value.ToString();

                string imageData = row.Cells[5].Value.ToString();

                if (imageData != null && imageData.Length > 0)
                {
                    teacher_image.Image = Image.FromFile(imageData);
                }
                else
                {
                    teacher_image.Image = null;
                }

                teacher_status.Text = row.Cells[6].ToString();
            }
        }
        private void teacher_clear_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void teacher_delete_Click(object sender, EventArgs e)
        {
            if (teacher_id.Text == "")
            {
                MessageBox.Show("Please select item first", "Error Message"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    DialogResult check = MessageBox.Show("Are you sure you want to Delete Teacher ID: "
                        + teacher_id.Text + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (check == DialogResult.Yes)
                    {
                        try
                        {
                            connect.Open();

                            DateTime today = DateTime.Today
                                ;
                            string deleteData = "UPDATE teachers SET date_delete = @dateDelete " +
                                "WHERE teacher_id = @teacherID";

                            using (SqlCommand cmd = new SqlCommand(deleteData, connect))
                            {
                                cmd.Parameters.AddWithValue("@dateDelete", today.ToString());
                                cmd.Parameters.AddWithValue("@teacherID", teacher_id.Text.Trim());

                                cmd.ExecuteNonQuery();

                                teacherDisplayData();

                                MessageBox.Show("Deleted successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                clearFields();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error  connecting Database: " + ex, "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cancelled.", "Information Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
