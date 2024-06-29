<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="todo.aspx.cs" Inherits="otp.todo.todo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>To Do List Application</title>
    <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css" rel="stylesheet">
    <style>
        html, body {
            height: 100%;
            margin: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            background-color: #f0f0f0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="bg-white p-8 rounded shadow-lg">
        <div class="container mx-auto">
            <h1 class="text-2xl font-bold mb-4 text-center">Todo List</h1>

            <div class="mb-4 flex items-center justify-center">
                <asp:TextBox ID="DescriptionTextBox" runat="server" CssClass="border rounded p-2 mr-2" Placeholder="New todo item"></asp:TextBox>
                <asp:Button ID="AddButton" runat="server" Text="Add" CssClass="bg-blue-500 text-white px-4 py-2 rounded" OnClick="AddButton_Click" />
            </div>
            <asp:GridView ID="TodoGridView" runat="server" AutoGenerateColumns="False" CssClass="w-full"
                OnRowDeleting="TodoGridView_RowDeleting" OnRowEditing="TodoGridView_RowEditing">
                <Columns>
                    <asp:BoundField DataField="todo_id" HeaderText="ID" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="item" HeaderText="Description" ItemStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="EditButton" runat="server" Text="Edit" CommandName="Edit" CssClass="bg-blue-500 hover:bg-blue-600 text-white px-4 py-1 rounded" />
                            <asp:Button ID="DeleteButton" runat="server" Text="Delete" CommandName="Delete" CssClass="bg-red-500 hover:bg-red-600 text-white px-4 py-1 rounded" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
                <RowStyle HorizontalAlign="Center" />
            </asp:GridView>






        </div>
    </form>
</body>
</html>
