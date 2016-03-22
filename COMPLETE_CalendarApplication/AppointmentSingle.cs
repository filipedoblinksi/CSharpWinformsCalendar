using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
    public partial class AppointmentSingle : Form
    {
        public bool saved;
        public int _model;
        public DateTime _starting;
        public IAppointment _SelectedAppointment;
        public AppointmentSingle(IAppointment _SelectedAppointment_, int model, DateTime starting)
        {
            saved = false;
            InitializeComponent();
            _model = model;
            if (model == 0)
            {
                _SelectedAppointment = _SelectedAppointment_;

                Subject.Text = getSubject(_SelectedAppointment.DisplayableDescription);
                Subject.SelectionStart = Subject.Text.Length;
                Subject.SelectionLength = 0;
                Lokacija.Text = getLocation(_SelectedAppointment.DisplayableDescription);
                Lokacija.SelectionStart = Lokacija.Text.Length;
                Lokacija.SelectionLength = 0;
                Datum.Text = _SelectedAppointment.Start.Date.ToString("dd MMMM yyyy");
                comboBox1.SelectedIndex = Utility.ConvertTimeToRow(_SelectedAppointment.Start);
                comboBox2.SelectedIndex = (_SelectedAppointment.Length / 30) - 1;
            }
            else
            {
                _starting = starting;
                Datum.Text = starting.Date.ToString("dd MMMM yyyy");
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
            }
        }

        private void Subject_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',' || e.KeyChar == '(' || e.KeyChar == ')')
            {
                e.Handled = true;
                // Disable these characters in these textboxes, because that's how we devide string into Subject, Location etc...
            }
        }

        private void Lokacija_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',' || e.KeyChar == '(' || e.KeyChar == ')')
            {
                e.Handled = true;
                // Disable these characters in these textboxes, because that's how we devide string into Subject, Location etc...
            }
        }
        public string getSubject(string Description)
        {
            string subject = "";
            for (int i = 0; i < Description.Length; i++)
            {
                if (Description[i] != ',') subject += Description[i];
                else break;
            }
            return subject;
        }
        public string getLocation(string Description)
        {
            string lokacija = "";
            int i = 0;
            for (i = 0; i < Description.Length; i++) // Jump over the Subject
            {
                if (Description[i] == ',') break;
            } 
            i = i + 2; // Jump over ', ' -> two letters
            for (int k = i; k < Description.Length; k++)
            {
                lokacija += Description[k];                              
            }
            return lokacija;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string poruka = "";
            if (Subject.Text.Length <= 0)
                poruka += "You must define Subject!";
            if (Lokacija.Text.Length <= 0) {
                if (poruka.Length>0)
                    poruka+= "\n";
                poruka += "You must define Location!";
            }
            if (poruka.Length>0)
                MessageBox.Show(poruka, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                // Edit this Appointment info
                DateTime newStart;
                if (_model==0)
                    newStart = Utility.ConvertRowToDateTime(_SelectedAppointment.Start,comboBox1.SelectedIndex);
                else
                    newStart = Utility.ConvertRowToDateTime(_starting, comboBox1.SelectedIndex);
                int newLength=(comboBox2.SelectedIndex+1)*30;
                string newDescription=Subject.Text+", "+Lokacija.Text;
                _SelectedAppointment = new Appointment(newStart, newLength, newDescription, 0, 0);
                saved = true;
                this.Close();
            }
        }
    }
   
}
