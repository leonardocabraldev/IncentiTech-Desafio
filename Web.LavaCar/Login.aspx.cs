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

        protected void CustomValidator1_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            string email = args.Value;
            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            args.IsValid = regex.IsMatch(email);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var loginInput = new LoginUserInput(txtEmail.Text, txtSenha.Text);
            var service = new LoginUser();

            var result = service.Execute(loginInput);
            if (string.IsNullOrEmpty(result.Token))
            {

            }
            else
            {
                Session["AuthToken"] = result.Token;
                Response.Redirect("Home.aspx");
            }
        }
    }
}