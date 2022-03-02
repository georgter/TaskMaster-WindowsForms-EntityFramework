using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskMaster.Model;

namespace TaskMaster
{
    public partial class Form1 : Form
    {
        private tmDBContext tmContext;
        public Form1()
        {
            // Отображаем в выпадаешем списке статусы которые записаны в базе 
            InitializeComponent();
            tmContext = new tmDBContext();
            var statuses = tmContext.Statuses.ToList();
            foreach(var s in statuses)
            {
                cboStatus.Items.Add(s);
            }
            refreshData();
        }
        /// <summary>
        /// Обновления данных в поле списка задачь
        /// </summary>
        private void refreshData()
        {
            //Источник привязки рекамендуемый 
            BindingSource bi = new BindingSource();

            var query = from t in tmContext.Tasks
                        orderby t.DueDate
                        select new { t.Id, TaskName = t.Name, StatusName = t.Status.Name, t.DueDate };

            bi.DataSource = query.ToList();

            dataGridView1.DataSource = bi;
            dataGridView1.Refresh();
        }

        /// <summary>
        /// Записываем данные в базу по нажатию кнопки Create
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdCreateTask_Click(object sender, EventArgs e)
        {
            if(cboStatus.SelectedItem != null && txtTask.Text !=String.Empty)
            {
                var newTask = new Model.Task
                {
                    Name = txtTask.Text,
                    StatusId = (cboStatus.SelectedItem as Model.Status).Id,
                    DueDate = dateTimePicker1.Value

                };
                tmContext.Tasks.Add(newTask);
                tmContext.SaveChanges();
                refreshData();
            }
            else
            {
                MessageBox.Show("Please make sure all data been entered");
            }
        }
        /// <summary>
        /// Удаления данных из таблицы 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdDeleteTask_Click(object sender, EventArgs e)
        {
            var t = tmContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);
            tmContext.Tasks.Remove(t);
            tmContext.SaveChanges();
            refreshData();
        }

        private void CmdUpdateTask_Click(object sender, EventArgs e)
        {
            if(cmdUpdateTask.Text == "Update")
            {
                txtTask.Text = dataGridView1.SelectedCells[1].Value.ToString();
                dateTimePicker1.Value =(DateTime)dataGridView1.SelectedCells[3].Value;
                foreach (Status s in cboStatus.Items)
                {
                    if(s.Name == dataGridView1.SelectedCells[2].Value.ToString())
                    {
                        cboStatus.SelectedItem = s;
                    }
                }
                cmdUpdateTask.Text = "Save";
            }
            else if(cmdUpdateTask.Text == "Save")
            {
                var t = tmContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);
                t.Name = txtTask.Text;
                t.StatusId = (cboStatus.SelectedItem as Status).Id;
                t.DueDate = dateTimePicker1.Value;

                tmContext.SaveChanges();
                refreshData();

                cmdUpdateTask.Text = "Update";
                txtTask.Text = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                cboStatus.Text = "Please Select...";
            }
            

        }

        private void CmdCancel_Click(object sender, EventArgs e)
        {
            cmdUpdateTask.Text = "Update";
            txtTask.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            cboStatus.Text = "Please Select...";
        }
    }
}
