using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToDoList.Model;
using System.Speech.Synthesis;

namespace ToDoList
{
    public partial class Home : Form
    {
        int count = 0;
         DBLAccess access = new  DBLAccess();
        DataTable dt = new DataTable();
        SpeechSynthesizer speechSynthesizerObj = new SpeechSynthesizer();
        public Home()
        {
            InitializeComponent();
        }

        public void Home_Load(object sender, EventArgs e)
        {
            notifyIcon1.ShowBalloonTip(2000);
           
            dt.Load(access.ExecuteReader("stSelectAllTasks"));
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[1].AutoSizeMode=DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            label3.Text+= " ("+ dt.Select("StateValue= 'pending'").Length + ")";

     
            label2.Text += " (" + dt.Select("StateValue= 'completed'").Length + ")";
         

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLocalTime().ToString();
         


        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
             this.ShowInTaskbar = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           panel2.Visible = !panel2.Visible;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTask addTask = new(dt,dataGridView1);
            addTask.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime TaskStartDate = new DateTime(dateTimePicker1.Value.Year,
             dateTimePicker1.Value.Month, dateTimePicker1.Value.Day,
             dateTimePicker1Time.Value.Hour,
             dateTimePicker1Time.Value.Minute, dateTimePicker1Time.Value.Second);

            DateTime TaskEndDate = new DateTime(dateTimePicker2.Value.Year,
            dateTimePicker2.Value.Month, dateTimePicker2.Value.Day,
            dateTimePicker2Time.Value.Hour,
            dateTimePicker2Time.Value.Minute, dateTimePicker2Time.Value.Second);

            dt.DefaultView.RowFilter = "TaskStartDate = #" + TaskStartDate + "# OR TaskEndDate = #"+TaskEndDate+"#";
            dataGridView1.DataSource = dt;

         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dt.DefaultView.RowFilter = String.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
            int result= access.DeleteExecuteNonQuery(Id);
            if (result == 1)
            {
                MessageBox.Show($"Task you selected with {Id} is Deleted Successfuly !","Danger Alert",MessageBoxButtons.OK,MessageBoxIcon.Error);
                DataTable dt = new DataTable();
                dt.Load(access.ExecuteReader("stSelectAllTasks"));
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else
            {
                MessageBox.Show($"Task you selected with {Id} is Can't be Deleted  !", "Danger Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Load(access.ExecuteReader("stSelectAllTasks"));
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            DataGridViewRow dataGridViewRow= dataGridView1.Rows[rowIndex] ;

            EditTask editTask = new EditTask(Convert.ToInt32(dataGridViewRow.Cells[0].Value), dt,dataGridView1);
            editTask .ShowDialog();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            foreach (DataRow item in dt.Rows)
            {
                if (DateTime.Now >= Convert.ToDateTime(item.ItemArray[2])
                    && DateTime.Now >= Convert.ToDateTime(item.ItemArray[3]))
                {



                    speechSynthesizerObj.SpeakAsync(item.ItemArray[1]?.ToString() +"timeout you need to review your tasks now ");
                    notifyIcon1.ShowBalloonTip(2000);


                }

            }


        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
        }
    }
}
