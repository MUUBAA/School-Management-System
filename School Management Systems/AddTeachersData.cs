using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_Systems
{
    internal class AddTeachersData
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mubaseer\Documents\SchoolDB.mdf;Integrated Security=True;Connect Timeout=30");
        public int ID { set; get; }

        public string TeacherID { set; get; }

        public string TeacherName { set; get; }

        public string TeacherGender { set; get; }

        public string TeacherAddress { set; get; }

        public string TeacherImage { set; get; }

        public string Teacherstatus { set; get; }

        public string DateInsert { set; get; }


        public List<AddTeachersData> teacherData()
        {
            List<AddTeachersData> listData = new List<AddTeachersData>();
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();
                    String sql = "SELECT * FROM teachers WHERE date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(sql, connect))
                    {

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            AddTeachersData addTD = new AddTeachersData();
                            addTD.ID = (int)reader["id"];
                            addTD.TeacherID = reader["teacher_id"].ToString();
                            addTD.TeacherName = reader["teacher_name"].ToString();
                            addTD.TeacherGender = reader["teacher_gender"].ToString();
                            addTD.TeacherAddress = reader["teacher_address"].ToString();
                            addTD.TeacherImage = reader["teacher_image"].ToString();
                            addTD.Teacherstatus = reader["teacher_status"].ToString();
                            addTD.DateInsert = reader["date_insert"].ToString();
                            listData.Add(addTD);
                        }
                        reader.Close();



                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error Connecting Databae" + ex);
                }
                finally
                {
                    connect.Close();
                }

            }
            return listData;
        }

    }
}