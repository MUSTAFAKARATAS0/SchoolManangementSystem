using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using OkulYönetim.Models;
using static OkulYönetim.Models.CommonFn;

namespace OkulYönetim.Admin
{
    public partial class AddClass : System.Web.UI.Page
    {
        CommonFnx fn = new CommonFnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetClass();
            }
        }

        private void GetClass()
        {
            DataTable dt = fn.Fetch("SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No], ClassId, ClassName FROM Class");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = fn.Fetch("SELECT * FROM Class WHERE ClassName = @ClassName", new SqlParameter("@ClassName", txtClass.Text.Trim()));
                if (dt.Rows.Count == 0)
                {
                    string query = "INSERT INTO Class (ClassName) VALUES (@ClassName)";
                    fn.Query(query, new SqlParameter("@ClassName", txtClass.Text.Trim()));
                    lblMsg.Text = "Inserted successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    txtClass.Text = string.Empty;
                    GetClass();
                }
                else
                {
                    lblMsg.Text = "Entered Class already exists!";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridView1.PageIndex= e.NewPageIndex;
            GetClass() ;
        }

        protected void GridView1_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetClass() ;
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
        }

        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int cId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string ClassName = (row.FindControl("txtClassEdit") as TextBox).Text;
                fn.Query("Update Class set ClassName = '"+ClassName+"' where ClassId ='"+cId+ "' ");
                lblMsg.Text = "Class Updated successfully!";
                lblMsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetClass();

            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");

            }

        }
    }
}
