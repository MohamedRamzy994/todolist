namespace ToDoList
{
    public partial class Splash : Form
    {
        int count = 1;
        public Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
         
           timer1.Start();
          

        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            count++;
            if (count<=5)
            {
                this.Show();

            }
            else
            {
                Home home = new Home();
                home.Show();
                this.Hide();
                count = 0;
                timer1.Stop(); 
            }

        }
    }
}