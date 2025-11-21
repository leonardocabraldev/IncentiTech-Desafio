using System;
using System.Linq;

namespace Web.LavaCar
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AuthToken"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                string email = Session["AuthToken"].ToString();
                lblUsuario.Text = email.Split('@')[0];
            }
        }

        protected void btnServicos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Servicos.aspx");
        }

        protected void btnAgendamentos_Click(object sender, EventArgs e)
        {
           Response.Redirect("Agendamentos.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }
    }
}