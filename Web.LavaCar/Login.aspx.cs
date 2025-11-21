using Application;
using Application.Services.Login;
using Shared.DTOs;
using System;
using System.Text.RegularExpressions;

namespace Web.LavaCar
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void cvUsername_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value);
        }
        protected void cvPassword_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value) && args.Value.Length > 5;
        }


        protected void btLogar_Click(object sender, EventArgs e)
        {
            Page.Validate("Login");
            if (!Page.IsValid)
                return;

            var loginInput = new LoginUserInput(txtUsername.Text, txtPassword.Text);
            var service = new LoginUser();
            var result = service.Execute(loginInput);

            Session["AuthToken"] = result.Token;
            Response.Redirect("Home.aspx");
        }
    }
}