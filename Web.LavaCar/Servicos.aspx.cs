using Application.Repositories;
using Application.Services.Servico;
using Infrastructure.LavaCar.Data.Repository.Infrastructure.Repositories;
using Shared.DTOs.Servicos.Create;
using Shared.DTOs.Servicos.Update;
using System;
using System.Configuration;
using System.Web.UI;

namespace Web.LavaCar
{
    public partial class Servicos : System.Web.UI.Page
    {
        private IServiceRepository _serviceRepository;
        protected void Page_Load(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _serviceRepository = new ServiceRepository(connStr);

            if (Session["AuthToken"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadServices();
            }

        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            modalNovoServicoLabel.InnerText = "Criar Serviço";
            ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModal();", true);
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        private void LoadServices()
        {
            var service = new GetServicesByUser(_serviceRepository);

            var listService = service.Execute(Session["AuthToken"].ToString(), 1, 100).Items;

            gvServicos.DataSource = listService;
            gvServicos.DataBind();
        }

        protected void gvServicos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            var isEditing = e.CommandName == "Editar";
            var isRemoving = e.CommandName == "Remover";
            if (isEditing)
            {
                var service = new GetServicesById(_serviceRepository);
                var editService = service.Execute(id);
                txtNome.Text = editService.Name;
                txtDescricao.Text = editService.Description;
                txtMaxAgend.Text = editService.MaximumConcurrentAppointments.ToString();
                hfIdServico.Value = id.ToString();
                modalNovoServicoLabel.InnerText = "Editar Serviço";
                ScriptManager.RegisterStartupScript(this, GetType(), "abrirModal", "abrirModal();", true);
            }
            else if (isRemoving)
            {
                var service = new DeleteService(_serviceRepository);
                service.Execute(id);
                LoadServices();
            }
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text;
            string descricao = txtDescricao.Text;
            int max = int.Parse(txtMaxAgend.Text);
            string user = Session["AuthToken"].ToString();

            var isEditingService = !string.IsNullOrEmpty(hfIdServico.Value);

            if (isEditingService)
            {
                int id = int.Parse(hfIdServico.Value);

                var service = new UpdateService(_serviceRepository);
                var input = new UpdateServiceInput(id, nome, descricao, max, user);
                service.Execute(input);
            }
            else
            {
                var service = new CreateService(_serviceRepository);
                var input = new CreateServiceInput(nome, descricao, max, user);
                service.Execute(input);
            }

            CleanModal();
            LoadServices();
        }

        private void CleanModal()
        {
            txtNome.Text = "";
            txtDescricao.Text = "";
            txtMaxAgend.Text = "";
            hfIdServico.Value = "";
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            CleanModal();
        }
    }
}