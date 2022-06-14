using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Model
{
    public class AddTaskModel
    {

        public int Id { get; set; }
        public string? TaskTitle { get; set; }
        public DateTime TaskStartDate { get; set; }
        public DateTime TaskEndDate { get; set; }
        public string? TaskNotes { get; set; }
        public bool ShowAlarm { get; set; }






    }
}
