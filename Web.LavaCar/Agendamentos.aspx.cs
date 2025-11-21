using Application.Repositories;
using Application.Services.Appointments;
using Infrastructure.LavaCar.Data.Repository;
using Infrastructure.LavaCar.Data.Repository.Infrastructure.Repositories;
using Shared.DTOs.Appointments.Create;
using Shared.DTOs.Appointments.Update;
using System;
using System.Configuration;
using System.Linq;
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
            var appointments = service.Execute(Session["AuthToken"]?.ToString(), CurrentPage, PageSize);

            gvAgendamentos.DataSource = appointments.Items;
            gvAgendamentos.DataBind();

            BindPagination(appointments.TotalCount);
        }

        private void BindPagination(int totalCount)
        {
            int totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            ViewState["TotalPages"] = totalPages;

            var pages = Enumerable.Range(1, totalPages)
                .Select(p => new { PageNumber = p, IsCurrent = p == CurrentPage })
                .ToList();

            rptPaginacao.DataSource = pages;
            rptPaginacao.DataBind();

            int firstItem = totalCount == 0 ? 0 : ((CurrentPage - 1) * PageSize) + 1;
            int lastItem = Math.Min(CurrentPage * PageSize, totalCount);
            lblPaginaInfo.Text = $"Mostrando {firstItem}-{lastItem} de {totalCount}";


            bool hasPrev = CurrentPage > 1;
            bool hasNext = CurrentPage < totalPages;

            lnkFirst.Enabled = hasPrev;
            lnkPrev.Enabled = hasPrev;
            lnkNext.Enabled = hasNext;
            lnkLast.Enabled = hasNext;

            ToggleDisabledClass(lnkFirst, !hasPrev);
            ToggleDisabledClass(lnkPrev, !hasPrev);
            ToggleDisabledClass(lnkNext, !hasNext);
            ToggleDisabledClass(lnkLast, !hasNext);
        }

        protected void rptPaginacao_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "GoPage")
            {
                int page = int.Parse(e.CommandArgument.ToString());
                LoadAppointments(page);
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            LoadAppointments(CurrentPage - 1);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            LoadAppointments(CurrentPage + 1);
        }

        protected void gvAgendamentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LoadAppointmentsInDropBox();
            int id = Convert.ToInt32(e.CommandArgument);

            var isEditing = e.CommandName == "Editar";
            var isRemoving = e.CommandName == "Remover";
            if (isEditing)
            {
                var ap = _appointmentRepository.GetById(id);
                if (ap == null) return;

                LoadAppointmentsInDropBox();
                ddlServico.SelectedValue = ap.ServiceId.ToString();
                txtCliente.Text = ap.ClientName;
                txtDataHora.Text = ap.ScheduledDateTime.ToString("yyyy-MM-ddTHH:mm");
                hfIdAgendamento.Value = ap.Id.ToString();

                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "ShowModal_" + Guid.NewGuid(),
                    "abrirModal();",
                    true);
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
            LoadAppointmentsInDropBox();
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "ShowModal_" + Guid.NewGuid(),
                "abrirModal();",
                true);
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "ShowModal_" + Guid.NewGuid(),
                    "abrirModal();",
                    true);
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
                var ok = update.Execute(input);

                if (!ok)
                {
                    // Mantém modal aberto se regras falharem
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "ShowModal_" + Guid.NewGuid(),
                        "abrirModal();",
                        true);
                    return;
                }
            }
            else
            {
                var create = new CreateAppointments(_appointmentRepository);
                var input = new CreateAppointmentInput(serviceId, clientName, scheduledDateTime, responsibleUser);
                create.Execute(input);
            }

            hfIdAgendamento.Value = "";
            CleanModal();
            LoadAppointments(CurrentPage);
        }

        protected void cvServico_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrEmpty(ddlServico.SelectedValue) &&
                           ddlServico.SelectedValue != "0";
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
                args.IsValid = agendamento > DateTime.Now;
            }
            else
            {
                args.IsValid = false;
            }
        }

        private void LoadAppointmentsInDropBox()
        {
            ddlServico.Items.Clear();

            var servicos = _serviceRepository.GetByUser(Session["AuthToken"]?.ToString(), 1, 100);

            ddlServico.DataSource = servicos.Items;
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

        private void ToggleDisabledClass(LinkButton btn, bool disabled)
        {
            var cls = btn.CssClass ?? "page-link";
            cls = cls.Replace(" disabled", "");
            if (disabled) cls += " disabled";
            btn.CssClass = cls;
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            LoadAppointments(1);
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            int totalPages = (int)(ViewState["TotalPages"] ?? 1);
            LoadAppointments(totalPages < 1 ? 1 : totalPages);
        }
    }
}