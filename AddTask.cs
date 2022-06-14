using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToDoList.Model;
using System.Speech;
using System.Speech.Synthesis;

namespace ToDoList
{
    public partial class AddTask : Form
    {
        DataTable mTable;
        DataGridView gv;
        public AddTask()
        {
            InitializeComponent();
        }
        public AddTask(DataTable maintable,DataGridView dataGridView)
        {
            this.mTable = maintable;
            this.gv = dataGridView;

            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBLAccess dBLAccess = new DBLAccess();
            AddTaskModel addTaskModel = new AddTaskModel
            {
              TaskTitle = txtTaskTitle.Text.Trim(),
              TaskNotes= txtTaskNotes.Text.Trim(),
              TaskStartDate=new DateTime(dateStartDate.Value.Year,
              dateStartDate.Value.Month, dateStartDate.Value.Day,
              dateStartTime.Value.Hour,
              dateStartTime.Value.Minute, dateStartTime.Value.Second),
              TaskEndDate = new DateTime(dateEndDate.Value.Year,
              dateEndDate.Value.Month, dateEndDate.Value.Day,
              dateEndTime.Value.Hour,
              dateEndTime.Value.Minute, dateEndTime.Value.Second),
              ShowAlarm=showAlarm.Checked

            };
            if (addTaskModel.ShowAlarm)
            {
               


                dBLAccess.ExecuteNonQuery(addTaskModel);
                mTable.Rows.Clear();
                mTable.Load(dBLAccess.ExecuteReader("stSelectAllTasks"));
                gv.DataSource = mTable;
                ClearInputs();
                this.Hide();

            }
            else
            {
                dBLAccess.ExecuteNonQuery(addTaskModel);
                mTable.Rows.Clear();
                mTable.Load(dBLAccess.ExecuteReader("stSelectAllTasks"));
                gv.DataSource = mTable;
                ClearInputs();
                this.Hide();

            }


        }

        public void ClearInputs()
        {
            txtTaskTitle.Clear();
            txtTaskNotes.Clear();
            dateStartDate.Value= DateTime.Now;
            dateEndDate.Value = DateTime.Now;
            dateStartTime.Value = DateTime.Now;
            dateEndTime.Value = DateTime.Now;
            showAlarm.Checked = false;


        }
    }
}
