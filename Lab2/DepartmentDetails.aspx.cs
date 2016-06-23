using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements required for EF DB access
using Lab2.Models;
using System.Web.ModelBinding;

namespace Lab2
{
    public partial class DepartmentDetails : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetDepartment();
            }
        }
        protected void GetDepartment()
        {
            // populate teh form with existing data from the database
            int DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

            // connect to the EF DB
            using (DefaultConnection db = new DefaultConnection())
            {
                // populate a Department object instance with the DepartmentID from the URL Parameter
                Department updatedDepartment = (from Department in db.Departments
                                          where Department.DepartmentID == DepartmentID
                                          select Department).FirstOrDefault();

                // map the Department properties to the form controls
                if (updatedDepartment != null)
                {
                    NameTextBox.Text = updatedDepartment.Name;
                    BudgetTextBox.Text = updatedDepartment.Budget.ToString();
                }
            }
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect back to Departments page
            Response.Redirect("~/Departments.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Use EF to connect to the server
            using (DefaultConnection db = new DefaultConnection())
            {
                // use the Department model to create a new Department object and
                // save a new record
                Department newDepartment = new Department();

                int DepartmentID = 0;

                if (Request.QueryString.Count > 0) // our URL has a DepartmentID in it
                {
                    // get the id from the URL
                    DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                    // get the current Department from EF DB
                    newDepartment = (from Department in db.Departments
                                  where Department.DepartmentID == DepartmentID
                                  select Department).FirstOrDefault();
                }

                // add form data to the new Department record
                newDepartment.Name = NameTextBox.Text;
                newDepartment.Budget = Convert.ToDecimal(BudgetTextBox.Text);

                // use LINQ to ADO.NET to add / insert new Department into the database

                if (DepartmentID == 0)
                {
                    db.Departments.Add(newDepartment);
                }


                // save our changes - also updates and inserts
                db.SaveChanges();

                // Redirect back to the updated Departments page
                Response.Redirect("~/Departments.aspx");
            }
        }
    }
}