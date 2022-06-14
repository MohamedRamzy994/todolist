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
    public partial class EditTask : Form
    {
        DataTable mainDataTable;
        DataGridView dataGridView;
        int Id;

        public EditTask()
        {
            InitializeComponent();
        }
        public EditTask(int Id , DataTable mainDataTable , DataGridView dataGridView)
        {
            this.mainDataTable= mainDataTable;
            this.dataGridView =dataGridView;
            this.Id = Id;   
            
            DBLAccess dBLAccess = new DBLAccess();
            InitializeComponent();

            DataTable dt = new DataTable();
            dt.Load(dBLAccess.ExecuteReaderSelectById("stSelectTaskById",Id));

           DataRow datarow = dt.Rows[0];

            AddTaskModel addTaskModel = new AddTaskModel
            {
                Id =Id,
                TaskTitle = datarow[1].ToString(),
                TaskStartDate = Convert.ToDateTime(datarow[2]),
                TaskEndDate = Convert.ToDateTime(datarow[3]),
                TaskNotes = datarow[5].ToString()
            };

            txtTaskTitle.Text = addTaskModel.TaskTitle;
            txtTaskNotes.Text = addTaskModel.TaskNotes;
            dateStartDate.Value = addTaskModel.TaskStartDate.Date;
            dateEndDate.Value = addTaskModel.TaskEndDate.Date;
            dateStartTime.Format = DateTimePickerFormat.Time;
            dateStartTime.Value = addTaskModel.TaskStartDate;
            dateEndTime.Format = DateTimePickerFormat.Time;
            dateEndTime.Value = addTaskModel.TaskEndDate;


           
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {



        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBLAccess dBLAccess = new DBLAccess();
            AddTaskModel addTaskModel = new AddTaskModel
            {
               Id=Id,  
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
                //SpeechSynthesizer speechSynthesizerObj = new SpeechSynthesizer();

                //for (int i = 0; i < 10; i++)
                //{
                //    speechSynthesizerObj.SpeakAsync(addTaskModel.TaskTitle.Trim());

                //}


                dBLAccess.ExecuteNonQueryUpdate(addTaskModel);
                 mainDataTable.Rows.Clear();
                mainDataTable.Load(dBLAccess.ExecuteReader("stSelectAllTasks"));
                dataGridView.DataSource = mainDataTable;
                ClearInputs();
                this.Hide();
               

            }
            else
            {
                dBLAccess.ExecuteNonQueryUpdate(addTaskModel);
                mainDataTable.Rows.Clear();

                mainDataTable.Load(dBLAccess.ExecuteReader("stSelectAllTasks"));
                dataGridView.DataSource = mainDataTable;
             
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
