using Application.Repositories;
using Application.Services.Appointments;
using Infrastructure.LavaCar.Data.Repository;
using Infrastructure.LavaCar.Data.Repository.Infrastructure.Repositories;
using Shared.DTOs.Appointments.Create;
using Shared.DTOs.Appointments.Update;
using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.LavaCar
{
    public partial class Agendamentos : System.Web.UI.Page
    {
        private IAppointmentRepository _appointmentRepository;
        private IServiceRepository _serviceRepository;

        private const int PageSize = 15;

        private int CurrentPage
        {
            get => (int?)ViewState["CurrentPage"] ?? 1;
            set => ViewState["CurrentPage"] = value < 1 ? 1 : value;
        }

        private int TotalPages
        {
            get => (int?)ViewState["TotalPages"] ?? 1;
            set => ViewState["TotalPages"] = value < 1 ? 1 : value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _appointmentRepository = new AppointmentRepository(connStr);
            _serviceRepository = new ServiceRepository(connStr);

            if (Session["AuthToken"] == null)
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
                LoadAppointments(CurrentPage);
        }

        private void LoadAppointments(int page)
        {
            CurrentPage = page;

            var service = new GetAppointmentsByUser(_appointmentRepository, _serviceRepository);
            var result = service.Execute(Session["AuthToken"]?.ToString(), CurrentPage, PageSize);

            gvAgendamentos.DataSource = result.Items;
            gvAgendamentos.DataBind();

            TotalPages = (int)Math.Ceiling(result.TotalCount / (double)PageSize);

            int firstItem = result.TotalCount == 0 ? 0 : ((CurrentPage - 1) * PageSize) + 1;
            int lastItem = Math.Min(CurrentPage * PageSize, result.TotalCount);

            lblPaginaInfo.Text = $"Página {CurrentPage} de {TotalPages} — mostrando {firstItem}-{lastItem} de {result.TotalCount}";
        }
        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            LoadAppointments(CurrentPage);
        }

        protected void btnProximo_Click(object sender, EventArgs e)
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;

            LoadAppointments(CurrentPage);
        }
        
        protected void gvAgendamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LoadAppointmentsInDropDown();
            int id = Convert.ToInt32(e.CommandArgument);

            var isEditing = e.CommandName == "Editar";
            var isRemoving = e.CommandName == "Remover";

            if (isEditing)
            {
                var ap = _appointmentRepository.GetById(id);
                if (ap == null) return;

                ddlServico.SelectedValue = ap.ServiceId.ToString();
                txtCliente.Text = ap.ClientName;
                txtDataHora.Text = ap.ScheduledDateTime.ToString("yyyy-MM-ddTHH:mm");
                hfIdAgendamento.Value = ap.Id.ToString();

                lblError.Visible = false;

                ScriptManager.RegisterStartupScript(
                    this, GetType(), "ShowModal_" + Guid.NewGuid(), "abrirModal();", true);
            }
            else if (isRemoving)
            {
                var service = new DeleteAppointment(_appointmentRepository);
                service.Execute(id);
            }

            LoadAppointments(CurrentPage);
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            CleanModal();
            LoadAppointmentsInDropDown();
            lblError.Visible = false;
            ScriptManager.RegisterStartupScript(
                this, GetType(), "ShowModal_" + Guid.NewGuid(), "abrirModal();", true);
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Page.Validate("Agendamentos");
            if (!Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "ShowModal_" + Guid.NewGuid(), "abrirModal();", true);
                return;
            }

            int serviceId = int.Parse(ddlServico.SelectedValue);
            string clientName = txtCliente.Text.Trim();
            DateTime scheduledDateTime = DateTime.Parse(txtDataHora.Text);
            string responsibleUser = Session["AuthToken"]?.ToString() ?? "Sistema";

            bool isEditing = !string.IsNullOrEmpty(hfIdAgendamento.Value);

            if (isEditing)
            {
                int id = int.Parse(hfIdAgendamento.Value);
                var update = new UpdateAppointment(_appointmentRepository, _serviceRepository);
                var input = new UpdateAppointmentInput(id, serviceId, clientName, scheduledDateTime, responsibleUser);

                bool ok = update.Execute(input);

                if (!ok)
                {
                    lblError.Text = "Máximo de agendamentos por horário  de serviço atingido!";
                    lblError.Visible = true;
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "ShowModal_" + Guid.NewGuid(), "abrirModal();", true);
                    return;
                }

                lblError.Visible = false;
            }
            else
            {
                var create = new CreateAppointments(_appointmentRepository, _serviceRepository);
                var input = new CreateAppointmentInput(serviceId, clientName, scheduledDateTime, responsibleUser);
                var ok = create.Execute(input);

                if (!ok)
                {
                    lblError.Text = "Máximo de agendamentos por horário  de serviço atingido!";
                    lblError.Visible = true;
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "ShowModal_" + Guid.NewGuid(), "abrirModal();", true);
                    return;
                }

                lblError.Visible = false;
            }

            CleanModal();
            LoadAppointments(CurrentPage);
        }

        protected void cvServico_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value);
        }

        protected void cvCliente_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(args.Value) &&
                           args.Value.Trim().Length >= 3;
        }

        protected void cvDataHora_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (DateTime.TryParse(args.Value, out DateTime agendamento))
            {
                DateTime minimoPermitido = DateTime.Now.AddHours(2);
                args.IsValid = agendamento > minimoPermitido;
            }
            else
            {
                args.IsValid = false;
            }
        }

 

        private void LoadAppointmentsInDropDown()
        {
            ddlServico.Items.Clear();

            var servicos = _serviceRepository.GetAllActiveByUser(Session["AuthToken"]?.ToString());

            ddlServico.DataSource = servicos;
            ddlServico.DataTextField = "Name";
            ddlServico.DataValueField = "Id";
            ddlServico.DataBind();

            ddlServico.Items.Insert(0, new ListItem("Selecione", ""));
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            CleanModal();
        }

        private void CleanModal()
        {
            ddlServico.SelectedIndex = 0;
            txtCliente.Text = "";
            txtDataHora.Text = "";
            hfIdAgendamento.Value = "";
        }
    }
}
