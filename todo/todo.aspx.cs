using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace otp.todo
{
    public partial class todo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        SqlConnection connection = new SqlConnection("Data Source=./;Initial Catalog=aa;Integrated Security=True");

        protected void AddButton_Click(object sender, EventArgs e)
        {
            string description = DescriptionTextBox.Text.Trim();

            if (description == "")
            {
                Response.Write("<script>alert('All fields required.')</script>");
                return;
            }
            if (IsDescriptionExists(description))
            {
                Response.Write("<script>alert('This todo item already exists.')</script>");
                return;
            }




            string query = "INSERT INTO todo (item) VALUES (@Description)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Description", description);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                clear();

            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('An error occurred: {ex.Message}')</script>");
            }
            BindTodoItems();

        }
        protected void BindTodoItems()
        {


            string query = "SELECT todo_id,item from  todo";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            TodoGridView.DataSource = dataTable;
            TodoGridView.DataBind();
        }

        private bool IsDescriptionExists(string description)
        {

            string query = "SELECT COUNT(*) FROM todo WHERE item = @Description";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Description", description);

            connection.Open();
            int count = (int)command.ExecuteScalar();
            connection.Close();

            return count > 0;
        }
        public void clear()
        {
            DescriptionTextBox.Text = "";
        }


        protected void edit(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int rowIndex = row.RowIndex;
            string itemDescription = TodoGridView.Rows[rowIndex].Cells[1].Text;

            // Set the textbox for editing
            DescriptionTextBox.Text = itemDescription;

            // Optionally, store the ID in a hidden field for later use
            ViewState["EditItemID"] = TodoGridView.Rows[rowIndex].Cells[0].Text; // Assuming ID is in the first cell
        }

        // Separate method to save the edited item
        protected void SaveEditButton_Click(object sender, EventArgs e)
        {
            string newItemDescription = DescriptionTextBox.Text.Trim();
            string itemId = ViewState["EditItemID"].ToString();

            if (!string.IsNullOrEmpty(newItemDescription))
            {
                string query = "UPDATE todo SET item = @NewItem WHERE todo_id = @ItemID";
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewItem", newItemDescription);
                        command.Parameters.AddWithValue("@ItemID", itemId);
                        command.ExecuteNonQuery();
                    }
                    BindTodoItems();
                    clear();
                }
                catch (Exception error)
                {
                    Response.Write($"<script>alert('An error occurred: {error.Message}')</script>");
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                Response.Write("<script>alert('Description cannot be empty.')</script>");
            }
        }


        protected void delete(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            GridViewRow row = (GridViewRow)btn.NamingContainer;

            int rowIndex = row.RowIndex;

            string itemDescription = TodoGridView.Rows[rowIndex].Cells[1].Text;

            DeleteTodoItem(itemDescription);

            BindTodoItems();
        }
        protected void DeleteTodoItem(string itemDescription)
        {
            string query = "DELETE FROM todo WHERE item = @ItemDescription";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ItemDescription", itemDescription);

            connection.Open();
            command.ExecuteNonQuery();
        }

     
        protected void TodoGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            TodoGridView.EditIndex = -1;
            BindTodoItems();
        }
        protected void TodoGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            TodoGridView.EditIndex = e.NewEditIndex;
            BindTodoItems();
        }

        protected void TodoGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = TodoGridView.Rows[e.RowIndex];
            string itemDescription = row.Cells[1].Text;

            DeleteTodoItem(itemDescription);

            BindTodoItems();
        }

    }


}
