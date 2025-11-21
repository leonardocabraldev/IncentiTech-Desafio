<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Agendamentos.aspx.cs" Inherits="Web.LavaCar.Agendamentos" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agendamentos - LavaCar Manager</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />
    <link href="~/Content/Global.css" rel="stylesheet" />
    <style>

    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="container page-wrapper">
        <div class="content-flex">
            <div class="dashboard-card p-0">
                 <div class="dashboard-header">
                     <h2><i class="bi bi-calendar-check"></i> Agendamentos</h2>

                     <div class="d-flex gap-2">
                         <button id="Button1" runat="server"
                                 onserverclick="btnNovo_Click"
                                 class="btn btn-primary btn-modern d-flex align-items-center justify-content-center gap-2">
                             <i class="bi bi-plus-lg me-1 text-white fs-6"></i> Novo Agendamento
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
            <div class="dashboard-footer">
                <div class="d-flex justify-content-between align-items-center">

                    <asp:Label ID="lblPaginaInfo" runat="server"
                        CssClass="fw-semibold"></asp:Label>

                    <div class="d-flex gap-2">
                        <asp:Button ID="btnAnterior" runat="server"
                            Text="Anterior"
                            CssClass="btn btn-outline-primary btn-sm"
                            OnClick="btnAnterior_Click" />

                        <asp:Button ID="btnProximo" runat="server"
                            Text="Próxima"
                            CssClass="btn btn-outline-primary btn-sm"
                            OnClick="btnProximo_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
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
                        <div class="mb-3 text-start form-outline">
                            <label class="form-label text-black">Serviço</label>
                            <asp:DropDownList ID="ddlServico" runat="server" CssClass="form-control" />
                            <asp:CustomValidator ID="cvServico" runat="server"
                                ControlToValidate="ddlServico"
                                OnServerValidate="cvServico_ServerValidate"
                                ErrorMessage="Selecione algum serviço"
                                Display="Dynamic"
                                ValidationGroup="Agendamentos"
                                ValidateEmptyText="true"
                                CssClass="text-danger" />
                        </div>
                        <div class="mb-3 text-start form-outline">
                            <label class="form-label text-black">Cliente</label>
                            <asp:TextBox ID="txtCliente" runat="server" CssClass="form-control" />
                            <asp:CustomValidator ID="cvCliente" runat="server"
                                ControlToValidate="txtCliente"
                                OnServerValidate="cvCliente_ServerValidate"
                                ErrorMessage="Digite pelo menos 3 caracteres."
                                Display="Dynamic"
                                ValidationGroup="Agendamentos"
                                ValidateEmptyText="true"
                                CssClass="text-danger" />
                        </div>
                        <div class="mb-3 text-start form-outline">
                            <label class="form-label text-black">Data e Hora</label>
                            <asp:TextBox ID="txtDataHora" runat="server" CssClass="form-control" TextMode="DateTimeLocal" />
                            <asp:CustomValidator ID="cvDataHora" runat="server"
                                ControlToValidate="txtDataHora"
                                OnServerValidate="cvDataHora_ServerValidate"
                                ErrorMessage="A data e hora devem ser pelo menos 2 horas à frente."
                                Display="Dynamic"
                                ValidationGroup="Agendamentos"
                                ValidateEmptyText="true"
                                CssClass="text-danger" />
                        </div>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="text-danger mb-2" Visible="false"></asp:Label>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar"
                        CssClass="btn btn-success" OnClick="btnSalvar_Click" CausesValidation="true" ValidationGroup="Agendamentos"/>
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