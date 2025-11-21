<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Servicos.aspx.cs" Inherits="Web.LavaCar.Servicos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Serviços - LavaCar Manager</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />

<style>
    body {
        background: #f4f6f9;
        font-family: 'Segoe UI', sans-serif;
        padding-top: 20px;
    }

    /* Card principal */
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

    /* Botões modernos */
    .btn-modern {
        border-radius: 8px !important;
        padding: 0.55rem 1.2rem !important;
        font-weight: 600;
    }

    /* GRID VIEW moderno */
    .table {
        border-radius: 0.75rem !important;
        overflow: hidden;
    }

    .table thead {
        background: #0d6efd;
        color: white;
        font-weight: 600;
        text-transform: uppercase;
        letter-spacing: 0.5px;
    }

    .btn-sm {
        padding: 4px 10px !important;
        border-radius: 6px !important;
        font-size: 0.85rem !important;
        font-weight: 500;
    }

    /* Modal elegante */
    .modal-content {
        border-radius: 1rem;
        box-shadow: 0 6px 25px rgba(0, 0, 0, 0.2);
    }

    .modal-header {
        border-radius: 1rem 1rem 0 0;
    }
</style>

</head>
<body>
<form id="form1" runat="server">
    <div class="container mt-4">

        <div class="dashboard-card p-0 col-12 col-lg-10 mx-auto">

            <!-- HEADER -->
            <div class="dashboard-header">
                <h2><i class="bi bi-tools"></i> Serviços</h2>

                <div class="d-flex gap-2">
                    <button id="btnNovo" runat="server"
                            onserverclick="btnNovo_Click"
                            class="btn btn-primary btn-modern d-flex align-items-center justify-content-center gap-2">
                        <i class="bi bi-plus-lg me-1 text-white fs-6"></i> Novo Serviço
                    </button>

                    <asp:Button ID="btnVoltar" runat="server" Text="Voltar"
                        CssClass="btn btn-outline-secondary btn-modern"
                        OnClick="btnVoltar_Click" />
                </div>
            </div>

            <!-- GRID -->
            <div class="p-4">
                <asp:GridView ID="gvServicos" runat="server"
                    CssClass="table table-striped table-hover"
                    AutoGenerateColumns="False"
                    OnRowCommand="gvServicos_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Nome" />
                        <asp:BoundField DataField="Description" HeaderText="Descrição" />
                        <asp:BoundField DataField="MaximumConcurrentAppointments" HeaderText="Máx. Agendamentos" />

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
    </div>
    <div class="modal fade" id="modalNovoServico" tabindex="-1" aria-labelledby="modalNovoServicoLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="modalNovoServicoLabel" runat="server">Criar Novo Serviço</h5>
                    <asp:Button ID="btnClose" runat="server" class="btn-close btn-close-white"
                                data-bs-dismiss="modal" OnClick="btnCancelar_Click"/>
                </div>

                <div class="modal-body">
                    <asp:HiddenField ID="hfIdServico" runat="server" />

                    <div class="mb-3">
                        <label class="form-label text-black">Nome do Serviço</label>
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"
                                     placeholder="Ex: Lavagem Completa" />
                       <asp:CustomValidator ID="cvNome" runat="server"
                            ControlToValidate="txtNome"
                            CssClass="text-danger small"
                            Display="Dynamic"
                            ValidationGroup="Servicos"
                            ValidateEmptyText="true"
                            ErrorMessage="Informe um nome (mín. 3 caracteres)."
                            OnServerValidate="cvNome_ServerValidate" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label text-black">Descrição</label>
                        <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control"
                                     placeholder="Detalhes do serviço" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label text-black">Máx. Agendamentos</label>
                        <asp:TextBox ID="txtMaxAgend" runat="server" CssClass="form-control"
                                     TextMode="Number" min="1" />
                        <asp:CustomValidator ID="cvMaxAgend" runat="server"
                            ControlToValidate="txtMaxAgend"
                            CssClass="text-danger small"
                            Display="Dynamic"
                            ValidationGroup="Servicos"
                            ValidateEmptyText="true"
                            ErrorMessage="Informe um número inteiro maior ou igual a 1."
                            OnServerValidate="cvMaxAgend_ServerValidate" />
                    </div>
                </div>

                <div class="modal-footer">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-success" OnClick="btnSalvar_Click" CausesValidation="true" ValidationGroup="Servicos" />

                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                                class="btn btn-outline-secondary"
                                data-bs-dismiss="modal" OnClick="btnCancelar_Click" CausesValidation="false" />
                </div>

            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function abrirModal() {
            var modal = new bootstrap.Modal(document.getElementById('modalNovoServico'));
            modal.show();
        }
    </script>
</form>
</body>

</html>
