<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Agendamentos.aspx.cs" Inherits="Web.LavaCar.Agendamentos" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agendamentos - LavaCar Manager</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />
    <style>
        body, html { height: 100%; }
        .page-wrapper {
            min-height: 100vh;
            display: flex;
            flex-direction: column;
            padding-top: 1rem;
            padding-bottom: 1rem;
        }
        .content-flex { flex: 1; display: flex; flex-direction: column; }
        .scroll-area {
            flex: 1;
            max-height: 60vh;
            overflow-y: auto;
            border: 1px solid #e5e5e5;
            border-radius: .75rem;
            background: #fff;
        }
        .pagination-bar button.active {
            background:#0d6efd;
            color:#fff;
            border-color:#0d6efd;
        }

        .dashboard-card {
            border: none;
            border-radius: 1rem;
            background: #ffffff;
            box-shadow: 0 6px 20px rgba(0, 0, 0, 0.08);
        }

        .dashboard-header {
            padding: 1.7rem;
            border-bottom: 1px solid #e5e5e5;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .dashboard-header h2 {
            margin: 0;
            font-weight: 700;
            color: #333;
        }

        .dashboard-header i {
            font-size: 1.9rem;
            color: #0d6efd;
        }

        
        .dashboard-footer {
            border-top: 1px solid #e5e5e5;
            padding: 0.9rem 1.2rem;
            background: #fff;
            border-radius: 0 0 1rem 1rem;
        }
        .dashboard-footer .btn {
            border-radius: 8px;
        }
        .dashboard-footer .page-btn.active {
            background:#0d6efd; color:#fff; border-color:#0d6efd;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="container page-wrapper">
        <div class="content-flex">
            <div class="dashboard-card p-0 mb-3">
                 <div class="dashboard-header">
                     <h2><i class="bi bi-calendar-check"></i> Agendamentos</h2>

                     <div class="d-flex gap-2">
                         <button id="Button1" runat="server"
                                 onserverclick="btnNovo_Click"
                                 class="btn btn-primary btn-modern d-flex align-items-center justify-content-center gap-2">
                             <i class="bi bi-plus-lg me-1 text-white fs-6"></i> Novo Serviço
                         </button>

                         <asp:Button ID="Button2" runat="server" Text="Voltar"
                             CssClass="btn btn-outline-secondary btn-modern"
                             OnClick="btnVoltar_Click" />
                     </div>
                 </div>
                <div class="p-3 scroll-area">
                    <asp:GridView ID="gvAgendamentos" runat="server"
                        CssClass="table table-striped table-hover mb-0"
                        AutoGenerateColumns="False"
                        OnRowCommand="gvAgendamentos_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="ServiceName" HeaderText="Serviço" />
                            <asp:BoundField DataField="ClientName" HeaderText="Cliente" />
                            <asp:BoundField DataField="ScheduledDateTime" HeaderText="Data/Hora" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="ResponsibleUser" HeaderText="Usuário" />
                            <asp:TemplateField HeaderText="Ações">
                                <ItemTemplate>
                                    <div class="d-flex gap-2">
                                        <asp:Button ID="btnEditar" runat="server" Text="Editar"
                                            CssClass="btn btn-sm btn-primary"
                                            CommandName="Editar"
                                            CommandArgument='<%# Eval("Id") %>' />
                                        <asp:Button ID="btnRemover" runat="server" Text="Remover"
                                            CssClass="btn btn-sm btn-danger"
                                            CommandName="Remover"
                                            CommandArgument='<%# Eval("Id") %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <!-- PAGINAÇÃO -->
            <div class="dashboard-footer">
                <div class="d-flex flex-column flex-md-row align-items-md-center justify-content-between gap-2">
                    <asp:Label ID="lblPaginaInfo" runat="server" CssClass="fw-semibold mb-2 mb-md-0"></asp:Label>

                    <div class="d-flex align-items-center gap-1 flex-wrap">
                        <asp:LinkButton ID="lnkFirst" runat="server" CssClass="btn btn-outline-primary btn-sm"
                            OnClick="btnFirst_Click" ToolTip="Primeira">««</asp:LinkButton>

                        <asp:LinkButton ID="lnkPrev" runat="server" CssClass="btn btn-outline-primary btn-sm"
                            OnClick="btnPrev_Click" ToolTip="Anterior">«</asp:LinkButton>

                        <asp:Repeater ID="rptPaginacao" runat="server" OnItemCommand="rptPaginacao_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton runat="server"
                                    CssClass='<%# "btn btn-sm page-btn " + ((bool)Eval("IsCurrent") ? "active" : "btn-light border") %>'
                                    CommandName="GoPage"
                                    CommandArgument='<%# Eval("PageNumber") %>'>
                                    <%# Eval("PageNumber") %>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>

                        <asp:LinkButton ID="lnkNext" runat="server" CssClass="btn btn-outline-primary btn-sm"
                            OnClick="btnNext_Click" ToolTip="Próxima">»</asp:LinkButton>

                        <asp:LinkButton ID="lnkLast" runat="server" CssClass="btn btn-outline-primary btn-sm"
                            OnClick="btnLast_Click" ToolTip="Última">»»</asp:LinkButton>
                    </div>
                </div>
        </div>
    </div>

    <!-- MODAL -->
    <div class="modal fade" id="modalAgendamento" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title"><i class="bi bi-plus-circle fs-5 me-2"></i> Novo Agendamento</h5>
                    <asp:Button ID="btnClose" runat="server" class="btn-close btn-close-white"
                        data-bs-dismiss="modal" OnClick="btnCancelar_Click" CausesValidation="false" UseSubmitBehavior="false" />
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfIdAgendamento" runat="server" />
                    <div class="row g-3">
                        <div class="mb-3">
                            <label class="form-label text-black">Serviço</label>
                            <asp:DropDownList ID="ddlServico" runat="server" CssClass="form-control" />
                            <asp:CustomValidator ID="cvServico" runat="server"
                                ControlToValidate="ddlServico"
                                OnServerValidate="cvServico_ServerValidate"
                                ErrorMessage="Selecione um serviço."
                                Display="Dynamic"
                                CssClass="text-danger" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label text-black">Cliente</label>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="form-control" />
                            <asp:CustomValidator ID="cvCliente" runat="server"
                                ControlToValidate="txtCliente"
                                OnServerValidate="cvCliente_ServerValidate"
                                ErrorMessage="Digite pelo menos 3 caracteres."
                                Display="Dynamic"
                                CssClass="text-danger" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label text-black">Data e Hora</label>
                            <asp:TextBox ID="txtDataHora" runat="server" CssClass="form-control" TextMode="DateTimeLocal" />
                            <asp:CustomValidator ID="cvDataHora" runat="server"
                                ControlToValidate="txtDataHora"
                                OnServerValidate="cvDataHora_ServerValidate"
                                ErrorMessage="Informe uma data válida futura."
                                Display="Dynamic"
                                CssClass="text-danger" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar"
                        CssClass="btn btn-success" OnClick="btnSalvar_Click" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                        CssClass="btn btn-outline-secondary"
                        data-bs-dismiss="modal" OnClick="btnCancelar_Click"
                        CausesValidation="false" UseSubmitBehavior="false" />
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function abrirModal() {
            var modal = new bootstrap.Modal(document.getElementById('modalAgendamento'));
            modal.show();
        }
    </script>
</form>
</body>
</html>