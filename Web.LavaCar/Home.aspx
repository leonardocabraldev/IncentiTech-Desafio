<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Web.LavaCar.Home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home - LavaCar Manager</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background: linear-gradient(135deg, #007bff, #00c6ff);
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            font-family: 'Segoe UI', sans-serif;
            margin: 0;
        }

        .card {
            background-color: #fff;
            color: #333;
            width: 100%;
            max-width: 420px;
            border: none;
            border-radius: 1rem;
            box-shadow: 0 6px 15px rgba(0, 0, 0, 0.2);
        }

        .card-header {
            background-color: #0d6efd;
            color: #fff;
            border-radius: 1rem 1rem 0 0;
            text-align: center;
            font-size: 1.5rem;
            font-weight: 600;
            padding: 1rem;
        }

        .card-body {
            padding: 2rem;
        }

        .welcome {
            font-size: 1rem;
            color: #333;
        }

        .btn-lg {
            font-size: 1.05rem;
            font-weight: 500;
            padding: 0.75rem 1rem;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card">
            <div class="card-header">
                🚗 LavaCar Manager
            </div>
            <div class="card-body text-center">
                <p class="welcome mb-4">
                    Bem-vindo, <asp:Label ID="lblUsuario" runat="server" CssClass="fw-bold text-primary" />!
                </p>

                <div class="d-grid gap-3">
                    <asp:Button ID="btnServicos" runat="server" CssClass="btn btn-primary btn-lg" Text="Gerenciar Serviços" OnClick="btnServicos_Click" />
                    <asp:Button ID="btnAgendamentos" runat="server" CssClass="btn btn-success btn-lg" Text="Gerenciar Agendamentos" OnClick="btnAgendamentos_Click" />
                    <asp:Button ID="btnLogout" runat="server" CssClass="btn btn-outline-danger btn-lg" Text="Sair" OnClick="btnLogout_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
